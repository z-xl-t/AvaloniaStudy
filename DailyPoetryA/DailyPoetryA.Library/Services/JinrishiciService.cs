using DailyPoetryA.Library.Helpers;
using DailyPoetryA.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Services
{
    // 实现类可以不跟接口一样的名字
    // 类命名应该凸显是本地实现还是依赖其他的外部信息，或者不同版本实现
    public class JinrishiciService: ITodayPoetryService
    {
        private string _domainName;
        private readonly IAlertService _alertService;
        private readonly IPreferenceStorage _preferenceStorage;
        private readonly IPoetryStorage _poetryStorage;
        private string _token = string.Empty;
        private const string Server = "今日诗词服务器";
        public static readonly string JinrishiciTokenKey = $"{nameof(JinrishiciService)}.Token";
        public JinrishiciService(IAlertService alertService, IPreferenceStorage preferenceStorage, IPoetryStorage poetryStorage, string domainName = "https://v2.jinrishici.com") 
        {
            _alertService = alertService;
            _preferenceStorage = preferenceStorage;
            _poetryStorage = poetryStorage;
            _domainName = domainName;
        }

        public async Task<string> GetTokenAsync()
        {
            // 内存缓存一次
            if (!string.IsNullOrEmpty(_token))
            {
                return _token;
            }

            // 本地文件缓存
            _token = _preferenceStorage.Get(JinrishiciTokenKey, string.Empty);

            if (!string.IsNullOrEmpty(_token))
            {
                return _token;
            }


            using var httpClient = new HttpClient();
            HttpResponseMessage response;
            var url = $"{_domainName}/token";
            try
            {
                response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                // 异常就近处理，新建一个 IAlertService 来处理。
                //Todo Handle this
                //await _alertService.AlertAsync("", e.Message);
                await _alertService.AlertAsync(
                    ErrorMessageHelper.HttpClientErrorTitle,
                    ErrorMessageHelper.GetHttpClientError($"{Server}: {url}", e.Message));
                return string.Empty;
            }

            var jsonStr = await response.Content.ReadAsStringAsync();
            
            try
            {
                // 反序列化, 属性名大小写设置为true（不敏感）
                var jinrishiciToken = JsonSerializer.Deserialize<JinrishiciToken>(jsonStr,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
                _token = jinrishiciToken.Data;
                _preferenceStorage.Set(JinrishiciTokenKey, _token);
                return _token;
            }
            catch (Exception e)
            {
                await _alertService.AlertAsync(
                 ErrorMessageHelper.JsonDeserializationErrorTitle,
                 ErrorMessageHelper.GetJsonDeserializationError($"{Server}: {url}", e.Message));
                return string.Empty;
            }

         
           
        }
        
        public async Task<TodayPoetry> GetTodayPoetryAsync()
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(_token))
            {
                return await GetRandomPoetryAsync();
            }

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-User-Token", token);
            HttpResponseMessage response;
            var url = $"{_domainName}/sentence";
            try
            {
                response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                await _alertService.AlertAsync(
                    ErrorMessageHelper.HttpClientErrorTitle,
                    ErrorMessageHelper.GetHttpClientError($"{Server}: {url}", e.Message));
                return await GetRandomPoetryAsync();
            }
            var jsonStr = await response.Content.ReadAsStringAsync();
            JinrishiciSentence jinrishiciSentence;
            try
            {
               jinrishiciSentence = JsonSerializer.Deserialize<JinrishiciSentence>(jsonStr,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? throw new JsonException();
            }
            catch (Exception e)
            {
                await _alertService.AlertAsync(
                    ErrorMessageHelper.JsonDeserializationErrorTitle,
                    ErrorMessageHelper.GetJsonDeserializationError($"{Server}: {url}", e.Message));
                return await GetRandomPoetryAsync();
            }
            
            try
            {
                var data = jinrishiciSentence.Data;
                return new TodayPoetry
                {
                    Snippet = data?.Content ?? throw new JsonException(),
                    Name = data?.Origin?.Title ?? throw new JsonException(),
                    Dynasty = data?.Origin?.Dynasty ?? throw new JsonException(),
                    Author = data?.Origin?.Author ?? throw new JsonException(),
                    Content = string.Join("\n", data.Origin?.Content) ?? throw new JsonException(),
                    Source = TodayPoetySources.Jinrishici
                };
            } 
            catch (Exception e) 
            {
                await _alertService.AlertAsync(
                   ErrorMessageHelper.JsonDeserializationErrorTitle,
                   ErrorMessageHelper.GetJsonDeserializationError($"{Server}: {url}", e.Message));
                return await GetRandomPoetryAsync();
            }
        }
        public async Task<TodayPoetry> GetRandomPoetryAsync()
        {

            var lambda = Expression.Lambda<Func<Poetry, bool>>(
                Expression.Constant(true),
                Expression.Parameter(typeof(Poetry), "p")
                );
            var poetres = await _poetryStorage.GetPoetryListAsync(
                lambda,
                new Random().Next(PoetryStorage.NumberPoetry), 1
                );
            var poetry = poetres.FirstOrDefault();
            return new TodayPoetry
            {
                Snippet = poetry.Snippet,
                Name = poetry.Name,
                Dynasty = poetry.Dynasty,
                Author = poetry.Author,
                Content = poetry.Content,
                Source = TodayPoetySources.Local,
            };
        }
    }

    public static class TodayPoetySources
    {
        public const string Jinrishici = nameof(Jinrishici);
        public const string Local = nameof(Local);
    }

    public class JinrishiciToken 
    {
        public string Status { get; set; }
        public string Data { get; set; }
    }

    public class JinrishiciSentence
    {
        public JinrishiciData Data { get; set; } = new();
    }
    public class JinrishiciData
    {
        public string Content { get; set; } = string.Empty;
        public JinrishiciOrigin Origin { get; set; } = new();
    }
    public class JinrishiciOrigin
    {
        public string Title { get; set; } = string.Empty;
        public string Dynasty { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public List<string> Content { get; set; } = new();
    }

}

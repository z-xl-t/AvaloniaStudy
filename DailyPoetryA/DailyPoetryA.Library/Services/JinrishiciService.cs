using System;
using System.Collections.Generic;
using System.Linq;
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

        public JinrishiciService(IAlertService alertService, string domainName = "https://v2.jinrishici.com") 
        {
            _alertService = alertService;
            _domainName = domainName;
        }

        public async Task<string> GetTokenAsync()
        {
            var httpClient = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await httpClient.GetAsync(
                                $"{_domainName}/token");
            }
            catch (Exception e)
            {
                // 异常就近处理，新建一个 IAlertService 来处理。
                //Todo Handle this
                await _alertService.AlertAsync("", e.Message);
                return null;
            }

            var jsonStr = await response.Content.ReadAsStringAsync();

            // 反序列化, 属性名大小写设置为true（不敏感）
            var jinrishiciToken = JsonSerializer.Deserialize<JinrishiciToken>(jsonStr,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                });
            return jinrishiciToken.Data;


        }
    }

    public class JinrishiciToken 
    {
        public string Status { get; set; }
        public string Data { get; set; }
    }
}

using DailyPoetryA.Library.Helpers;
using DailyPoetryA.Library.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Services
{
    public class BingImageService : ITodayImageService
    {
        private readonly ITodayImageStorage _todayImageStorage;
        private readonly IAlertService _alertService;
        private static HttpClient _httpClient = new();
        private const string Server = "必应每日图片服务器";
        public BingImageService(ITodayImageStorage todayImageStorage, IAlertService alertService)
        {
            _todayImageStorage = todayImageStorage;
            _alertService = alertService;
                
        }

        public async Task<TodayImageServiceCheckUpdateResult> CheckUpdateAsync()
        {
            var todayImage = await _todayImageStorage.GetTodayImageAsync(false);
            if (todayImage.ExpiresAt > DateTime.Now)
            {
                return new TodayImageServiceCheckUpdateResult { HasUpdate = false };
            }
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync("https://cn.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                await _alertService.AlertAsync(
                    ErrorMessageHelper.HttpClientErrorTitle,
                    ErrorMessageHelper.GetHttpClientError(Server, ex.Message));
                return new TodayImageServiceCheckUpdateResult { HasUpdate = false };
            }

            var json = await response.Content.ReadAsStringAsync();
            string bingImageUrl;
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var bingImage = JsonSerializer.Deserialize<BingImageOfTheDay>(json, options)?.Images?.FirstOrDefault() ?? throw new JsonException("json解析错误");

                var bingImageFullStartDate = DateTime.ParseExact(
                    bingImage.FullStartDate, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
                var todayImageFullStartDate = DateTime.ParseExact(
                    todayImage.FullStartDate, "yyyyMMddHHmm", CultureInfo.InvariantCulture);

                if (bingImageFullStartDate <= todayImageFullStartDate)
                {
                    todayImage.ExpiresAt = DateTime.Now.AddHours(2);
                    await _todayImageStorage.SaveTodayImageAsync(todayImage, true);
                    return new TodayImageServiceCheckUpdateResult
                    {
                        HasUpdate = false
                    };
                }

                todayImage = new TodayImage
                {
                    FullStartDate = bingImage.FullStartDate,
                    ExpiresAt = bingImageFullStartDate.AddDays(1),
                    Copyright = bingImage.Copyright,
                    CopyrightLink = bingImage.CopyrightLink
                };

                bingImageUrl = bingImage.Url;
            }
            catch (Exception ex) 
            {

                await _alertService.AlertAsync(
                    ErrorMessageHelper.JsonDeserializationErrorTitle,
                    ErrorMessageHelper.GetJsonDeserializationError(Server, ex.Message));
                return new TodayImageServiceCheckUpdateResult { HasUpdate = false };
            }

            try
            {
                var url = $"https://www.bing.com{bingImageUrl}";
                response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                await _alertService.AlertAsync(
                   ErrorMessageHelper.HttpClientErrorTitle,
                   ErrorMessageHelper.GetHttpClientError(Server, ex.Message));
                return new TodayImageServiceCheckUpdateResult { HasUpdate = false };
            }

            todayImage.ImageBytes = await response.Content.ReadAsByteArrayAsync();
            await _todayImageStorage.SaveTodayImageAsync(todayImage, false);
            return new TodayImageServiceCheckUpdateResult
            {
                HasUpdate = true,
                TodayImage = todayImage
            };
            

        }

        public async Task<TodayImage> GetTodayImageAsync()
        {
            return await _todayImageStorage.GetTodayImageAsync(true);
        }
    }


    public class BingImageOfTheDay
    {
        public List<BingImageOfTheDayImage> Images { get; set; }
    }
    public class BingImageOfTheDayImage
    {
        public string StartDate { get; set; }
        public string FullStartDate { get; set; }
        public string EndDate { get; set; }
        public string Url { get; set; }
        public string Copyright { get; set; }
        public string CopyrightLink { get; set; }
    }
}

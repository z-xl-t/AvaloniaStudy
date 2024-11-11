using DailyPoetryA.Library.Helpers;
using DailyPoetryA.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Services
{
    public class TodayImageStorage : ITodayImageStorage
    {
        private readonly IPreferenceStorage _preferenceStorage;

        public static readonly string FullStartDateKey = nameof(TodayImageStorage) + "." + nameof(TodayImage.FullStartDate);
        public static readonly string ExpriesAtKey = nameof(TodayImageStorage) + "." + nameof(TodayImage.ExpiresAt);
        public static readonly string CopyrightKey = nameof(TodayImageStorage)  + "." + nameof(TodayImage.Copyright);
        public static readonly string CopyrightLinkKey = nameof(TodayImageStorage) + "." + nameof(TodayImage.CopyrightLink);
        
        public const string FullStartDateDefault = "20241111";
        public static readonly DateTime ExpriesAtDefault = new(2024,11,11);
        public const string CopyrightDefault = "Salt field province vitnam work (@ Quangpraha/Pixabay)";
        public const string CopyrightLinkDefault = "https://pixabay.com/photos/salt-field-province-vietnam-work";
        public const string Filename = "todayImage.bin";
        public static readonly string TodayImagePath = PathHelper.GetLocalFilePath(Filename);
        public TodayImageStorage(IPreferenceStorage preferenceStorage)
        {
            _preferenceStorage = preferenceStorage;
        }

        public async Task<TodayImage> GetTodayImageAsync(bool isIncludingImageStream)
        {
            var todayImage = new TodayImage
            {
                FullStartDate = _preferenceStorage.Get(FullStartDateKey, FullStartDateDefault),
                ExpiresAt = _preferenceStorage.Get(ExpriesAtKey, ExpriesAtDefault),
                Copyright = _preferenceStorage.Get(CopyrightKey, CopyrightDefault),
                CopyrightLink = _preferenceStorage.Get(CopyrightLinkKey, CopyrightLinkDefault),

            };
            if (!File.Exists(TodayImagePath))
            {
                await using var imageAssetFileStream =
                        new FileStream(TodayImagePath, FileMode.Create) ??
                        throw new NullReferenceException("Null file stream");
                await using var imageAssetStream = typeof(TodayImageStorage).Assembly.GetManifestResourceStream(Filename) ??
                    throw new NullReferenceException("Null mainfest resource stream");
                await imageAssetStream.CopyToAsync(imageAssetFileStream);
        
            }
            if (!isIncludingImageStream)
            {
                return todayImage;
            }

            // 文件流 -> 内存流 -> 二进制数据
            var imageMemorySteam = new MemoryStream();
            await using var imageFileStream = new FileStream(TodayImagePath, FileMode.Open);
            await imageFileStream.CopyToAsync(imageMemorySteam);
            todayImage.ImageBytes = imageMemorySteam.ToArray();
            return todayImage;
        }
        public async Task SaveTodayImageAsync(TodayImage todayImage, bool isSavingExpiresAt)
        {
            _preferenceStorage.Set(ExpriesAtKey, ExpriesAtDefault);
            if (isSavingExpiresAt)
            {
                return;
            }
            if (todayImage.ImageBytes == null)
            {
                throw new ArgumentException($"Null image bytes.", nameof(todayImage));

            }

            _preferenceStorage.Set(FullStartDateKey, FullStartDateDefault);
            _preferenceStorage.Set(ExpriesAtKey, ExpriesAtDefault);
            _preferenceStorage.Set(CopyrightKey, CopyrightDefault);
            _preferenceStorage.Set(CopyrightLinkKey, CopyrightLinkDefault);

            await using var imageFileStream = new FileStream(TodayImagePath, FileMode.Create);
            await imageFileStream.WriteAsync(todayImage.ImageBytes,0, todayImage.ImageBytes.Length);
        }
    }
}

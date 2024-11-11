using DailyPoetryA.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Services
{
    public interface ITodayImageStorage
    {
        Task<TodayImage> GetTodayImageAsync(bool isIncludingImageStream);
        Task SaveTodayImageAsync(TodayImage todayImage, bool isSavingExpiresAt);
    }
}

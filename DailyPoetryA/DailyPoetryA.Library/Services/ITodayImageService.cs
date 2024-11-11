using DailyPoetryA.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Services
{
    public interface ITodayImageService
    {
        Task<TodayImage> GetTodayImageAsync();
        Task<TodayImageServiceCheckUpdateResult> CheckUpdateAsync();
    }

    public class TodayImageServiceCheckUpdateResult
    {
        public bool HasUpdate { get; set; }
        public TodayImage TodayImage { get; set; } = new();
    }
}

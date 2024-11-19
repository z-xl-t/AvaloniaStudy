using DailyPoetryA.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Services
{
    public interface IContentNavigationService
    {
        void NavigateTo(string view, object parameter = null);
    }
    public static class ContentNavigationConstant
    {
        public const string TodayDetailView = nameof(TodayDetailView);
        public const string ResultView = nameof(ResultView);
        public const string DetailView = nameof(DetailView);
    }
}

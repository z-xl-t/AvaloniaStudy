using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Services
{
    public interface IMenuNavigationService
    {
    }
    public static class MenuNavigationConstant
    {
        public const string TodayView = nameof(TodayView);
        public const string QueryView = nameof(QueryView);
        public const string FavoriteView = nameof(FavoriteView);
    }

}

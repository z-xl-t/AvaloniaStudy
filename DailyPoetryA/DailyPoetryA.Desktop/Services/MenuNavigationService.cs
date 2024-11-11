using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Desktop.Services
{
    public class MenuNavigationService : IMenuNavigationService
    {
        public void NavigateTo(string view)
        {
            var mainVm = ServiceLocator.Current.ServiceProvider.GetRequiredService<MainViewModel>();

            if (view == MenuNavigationConstant.TodayView)
            {
                var todayVm = ServiceLocator.Current.ServiceProvider.GetRequiredService<TodayViewModel>();
                mainVm.SetMenuAndContent(MenuNavigationConstant.TodayView, todayVm);
            }
            else if (view == MenuNavigationConstant.QueryView)
            {
                var queryVm = ServiceLocator.Current.ServiceProvider.GetRequiredService<QueryViewModel>();
                mainVm.SetMenuAndContent(MenuNavigationConstant.TodayView, queryVm);
            }
            else if (view == MenuNavigationConstant.FavoriteView)
            {
                var favoriteVm= ServiceLocator.Current.ServiceProvider.GetRequiredService<FavoriteViewModel>();
                mainVm.SetMenuAndContent(MenuNavigationConstant.TodayView, favoriteVm);
            }
            else
            {
                throw new Exception("未知视图");
            }
        }
    }
}

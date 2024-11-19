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
    public class ContentNavigationService : IContentNavigationService
    {
        public void NavigateTo(string view, object parameter = null)
        {
            var mainVm = ServiceLocator.Current.ServiceProvider.GetRequiredService<MainViewModel>();

            if (view == ContentNavigationConstant.TodayDetailView)
            {
                var todayDetailVm = ServiceLocator.Current.ServiceProvider.GetRequiredService<TodayDetailViewModel>();
                todayDetailVm.SetParameter(parameter);
                mainVm.SetContent(ContentNavigationConstant.TodayDetailView, todayDetailVm);
            }
        }
    }
}

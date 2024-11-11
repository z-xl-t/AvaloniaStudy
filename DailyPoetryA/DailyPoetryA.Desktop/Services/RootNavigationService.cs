using DailyPoetryA.Desktop.Views;
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
    public class RootNavigationService : IRootNavigationService
    {
        public void NavigateTo(string view)
        {
            if (view == RootNavigationConstant.InitializationView)
            {
                var initVm = ServiceLocator.Current.ServiceProvider.GetRequiredService<InitializationViewModel>();
                ServiceLocator.Current.MainWindowViewModel.Content = initVm;
            }
            else if (view == RootNavigationConstant.MainView)
            {
                var mainVm = ServiceLocator.Current.ServiceProvider.GetRequiredService<MainViewModel>();
                ServiceLocator.Current.MainWindowViewModel.Content = mainVm;
            }
        }
        
    }
}

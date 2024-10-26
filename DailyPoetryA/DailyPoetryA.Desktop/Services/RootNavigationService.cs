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
            if (view == nameof(TodayViewModel))
            {

                var vm = ServiceLocator.Current.ServiceProvider.GetRequiredService<TodayViewModel>();

                ServiceLocator.Current.MainWindowViewModel.Content = vm;
            }
        }
        
    }
}

using DailyPoetryA.Desktop.Services;
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Desktop
{
    public class ServiceLocator
    {
        public IServiceProvider ServiceProvider { get; }

        public ResultViewModel ResultViewModel =>
            ServiceProvider.GetRequiredService<ResultViewModel>();
        public TodayViewModel TodayViewModel =>
           ServiceProvider.GetRequiredService<TodayViewModel>();
        public ServiceLocator()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ResultViewModel>();
            serviceCollection.AddSingleton<TodayViewModel>();

            serviceCollection.AddSingleton<IPreferenceStorage, FilePreferenceStorage>();
            serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
            serviceCollection.AddSingleton<ITodayPoetryService, JinrishiciService>();
            serviceCollection.AddSingleton<IAlertService, AlertService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}

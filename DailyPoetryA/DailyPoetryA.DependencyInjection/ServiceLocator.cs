
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.DependencyInjection
{
    public class ServiceLocator
    {
        public  IServiceProvider ServiceProvider { get; }


        public ResultViewModel ResultViewModel =>
            ServiceProvider.GetRequiredService<ResultViewModel>();
        public ServiceLocator() {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ResultViewModel>();
            serviceCollection.AddSingleton<IPreferenceStorage,FilePreferenceStorage>();
            serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
            ServiceProvider = serviceCollection.BuildServiceProvider();

        }
    }
}

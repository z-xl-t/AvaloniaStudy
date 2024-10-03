
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
        private readonly IServiceProvider _serviceProvider;
        private static ServiceLocator _current;
        public static ServiceLocator Current { get => _current; }
        
        
        public ResultViewModel ResultViewModel =>
            _serviceProvider.GetRequiredService<ResultViewModel>();
        public ServiceLocator() {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ResultViewModel>();
            serviceCollection.AddSingleton<IPreferenceStorage,FilePreferenceStorage>();
            serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();

            _serviceProvider = serviceCollection.BuildServiceProvider();

            _current = this;
        }
    }
}

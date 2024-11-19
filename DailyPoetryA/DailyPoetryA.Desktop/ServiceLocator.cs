using Avalonia;
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
        // 通过这种方式，拿到依赖注入实例
        private static ServiceLocator _current;
        public static ServiceLocator Current
        {
            get
            {
                if (_current != null)
                {
                    return _current;
                }
                // 实际上依赖注入的实例，是在程序初始化时，用资源的形式生成了一个实例
                if (Application.Current.TryGetResource(nameof(ServiceLocator),null, out var value) &&
                    value is ServiceLocator serviceLocator)
                {
                    return _current = serviceLocator;
                }

                throw new Exception("不应该找不到 ServiceLocator 资源实例");
            }
        }
        public IServiceProvider ServiceProvider { get; }

        public MainWindowViewModel MainWindowViewModel => 
            ServiceProvider.GetRequiredService<MainWindowViewModel>();
        public ServiceLocator()
        {
            var serviceCollection = new ServiceCollection();

            // 注册 ViewModel
            serviceCollection.AddSingleton<ResultViewModel>();
            serviceCollection.AddSingleton<TodayViewModel>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<MainViewModel>();
            serviceCollection.AddSingleton<QueryViewModel>();
            serviceCollection.AddSingleton<FavoriteViewModel>();
            serviceCollection.AddSingleton<InitializationViewModel>();
            serviceCollection.AddSingleton<TodayDetailViewModel>();
            // 注册服务
            serviceCollection.AddSingleton<IPreferenceStorage, FilePreferenceStorage>();
            serviceCollection.AddSingleton<IPoetryStorage, PoetryStorage>();
            serviceCollection.AddSingleton<ITodayPoetryService, JinrishiciService>();
            serviceCollection.AddSingleton<IAlertService, AlertService>();

            // 导航服务注册
            serviceCollection.AddSingleton<IRootNavigationService, RootNavigationService>();
            serviceCollection.AddSingleton<IMenuNavigationService, MenuNavigationService>();
            serviceCollection.AddSingleton<IContentNavigationService, ContentNavigationService>();
            serviceCollection.AddSingleton<ITodayImageService, BingImageService>();
            serviceCollection.AddSingleton<ITodayImageStorage, TodayImageStorage>();
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}

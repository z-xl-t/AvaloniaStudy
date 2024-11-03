using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DailyPoetryA.Desktop.Views;
using DailyPoetryA.Library.Services;
using DailyPoetryA.Library.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DailyPoetryA.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);

            //desktop.MainWindow = new MainWindow
            //{
            //    DataContext = new MainWindowViewModel(),
            //};


            // 获取 ServiceLocator 实例
            var serviceLocator = this.Resources["ServiceLocator"] as ServiceLocator;

            // 手动设置主界面的 View 和 ViewModel， View 中的事件触发器会失效
            desktop.MainWindow = new MainWindow();
            desktop.MainWindow.DataContext = serviceLocator.ServiceProvider.GetRequiredService<MainWindowViewModel>();

            // 需要手动触发OnInitializedCommand
            var command = desktop.MainWindow.DataContext as MainWindowViewModel;
            if (command != null && command.OnInitializedCommand.CanExecute(null))
            {
                command.OnInitializedCommand.Execute(null);
            }

            // 初始化数据库
            if (serviceLocator != null)
            {
                var poetryStorage = serviceLocator.ServiceProvider.GetService<IPoetryStorage>();

                if (poetryStorage != null && !poetryStorage.IsInitialized)
                {
                    Task.Run( async () => await poetryStorage.InitializeAsync());
                }
            }


            //// 导航首页测试
            //serviceLocator.ServiceProvider
            //    .GetService<IRootNavigationService>().NavigateTo(nameof(MainView));
        }

        base.OnFrameworkInitializationCompleted();
    }
}
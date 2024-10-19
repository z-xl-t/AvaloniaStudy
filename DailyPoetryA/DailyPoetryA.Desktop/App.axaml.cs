using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using DailyPoetryA.Desktop.Views;
using DailyPoetryA.Library.Services;
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

            desktop.MainWindow = new MainWindow();


            // »ñÈ¡ ServiceLocator ÊµÀý
            var serviceLocator = this.Resources["ServiceLocator"] as ServiceLocator;

            if (serviceLocator != null)
            {
                var poetryStorage = serviceLocator.ServiceProvider.GetService<IPoetryStorage>();

                if (poetryStorage != null && !poetryStorage.IsInitialized)
                {
                    Task.Run( async () => await poetryStorage.InitializeAsync());
                }
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DailyPoetryA.Library.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace DailyPoetryA.Desktop.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Binding(object? sender, Avalonia.Input.TappedEventArgs e)
    {
    }
}
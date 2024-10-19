using DailyPoetryA.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ursa.Controls;

namespace DailyPoetryA.Desktop.Services
{
    // 在 view 层实现接口，然后提供给依赖注入
    public class AlertService : IAlertService
    {
        public async Task AlertAsync(string title, string message)
        {
            await MessageBox.ShowAsync(message, title);
        }
    }
}

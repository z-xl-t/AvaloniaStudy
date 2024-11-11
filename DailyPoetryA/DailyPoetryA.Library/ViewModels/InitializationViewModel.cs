using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DailyPoetryA.Library.ViewModels
{
    public class InitializationViewModel: ViewModelBase
    {
        private readonly IPoetryStorage _poetryStorage;
        private readonly IRootNavigationService _rootNavigationService;
        private readonly IMenuNavigationService _menuNavigationService;

        private ICommand OnInitializedCommand { get; }
        public InitializationViewModel(IPoetryStorage poetryStorage, IRootNavigationService rootNavigationService, IMenuNavigationService menuNavigationService)
        {
            _poetryStorage = poetryStorage;
            _rootNavigationService = rootNavigationService;
            _menuNavigationService = menuNavigationService;
            OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync
                );
        }

        private async Task OnInitializedAsync()
        {
            if (!_poetryStorage.IsInitialized)
            {
                await _poetryStorage.InitializeAsync();
            }

            await Task.Delay(1000);

            _rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
            _menuNavigationService.NavigateTo(MenuNavigationConstant.TodayView);
        }
    }
}

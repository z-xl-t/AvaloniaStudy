using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DailyPoetryA.Library.ViewModels
{
    public class TodayViewModel: ViewModelBase
    {
        private readonly ITodayPoetryService _todayPoetryService;

        private TodayPoetry _todayPoetry;
        public TodayPoetry TodayPoetry
        {
            get => _todayPoetry;
            set => SetProperty(ref _todayPoetry, value);
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand OnInitializedCommand { get;  }

        

        public TodayViewModel(ITodayPoetryService todayPoetryService)
        {
            _todayPoetryService = todayPoetryService;
            OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
        }

        public async Task OnInitializedAsync()
        {
            IsLoading = true;
            TodayPoetry = await _todayPoetryService.GetTodayPoetryAsync();
            IsLoading = false;
        }
    }
}

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
        private readonly ITodayImageService _todayImageService;
        private readonly IContentNavigationService _contentNavigationService;
        
        private TodayPoetry _todayPoetry;
        public TodayPoetry TodayPoetry
        {
            get => _todayPoetry;
            set => SetProperty(ref _todayPoetry, value);
        }

        private TodayImage _todayImage;
        public TodayImage TodayImage 
        { 
            get => _todayImage; 
            set => SetProperty(ref _todayImage, value); 
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        
        public ICommand ShowDetailCommand { get; set; }
        public ICommand OnInitializedCommand { get;  }

        public TodayViewModel()
        {
            
        }

        public TodayViewModel(ITodayPoetryService todayPoetryService, ITodayImageService todayImageService, IContentNavigationService contentNavigationService)
        {
            _todayPoetryService = todayPoetryService;
            _todayImageService = todayImageService;
            _contentNavigationService = contentNavigationService;
            OnInitializedCommand = new RelayCommand(OnInitialized);
            ShowDetailCommand = new RelayCommand(ShowDetail);
        }

        private void ShowDetail()
        {
            _contentNavigationService.NavigateTo(
                ContentNavigationConstant.TodayDetailView, TodayPoetry);
        }

        public void OnInitialized()
        {
            Task.Run(async () =>
            {
                IsLoading = true;
                TodayPoetry = await _todayPoetryService.GetTodayPoetryAsync();
                IsLoading = false;
            });
            Task.Run(async () =>
            {
                TodayImage = await _todayImageService.GetTodayImageAsync();
                var updateResult = await _todayImageService.CheckUpdateAsync();
                if (updateResult.HasUpdate)
                {
                    TodayImage = updateResult.TodayImage;
                }
            });

           
        }
    }
}

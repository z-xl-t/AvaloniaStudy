using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DailyPoetryA.Library.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
    {
        private ViewModelBase _content;
        private readonly IRootNavigationService _rootNavigationService;

        public ViewModelBase Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public ICommand OnInitializedCommand { get; }



        public MainWindowViewModel(IRootNavigationService rootNavigationService)
        {

            _rootNavigationService = rootNavigationService;
            OnInitializedCommand = new RelayCommand(OnInitialized);
        }

        public void OnInitialized()
        {
            _rootNavigationService.NavigateTo(RootNavigationConstant.InitializationView);
        }
    }
}

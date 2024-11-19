using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DailyPoetryA.Library.ViewModels
{
    public class MainViewModel: ViewModelBase
    {


        private readonly IMenuNavigationService _menuNavigationService;
        private ViewModelBase _content;
        public ViewModelBase Content
        {
            get => _content;
            private set
            {

                SetProperty(ref _content, value);
            }
        }
        private string _title;

        public string Title
        {
            get => _title;
            private set => SetProperty(ref _title, value);
        }

        private bool _isPaneOpen;

        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => SetProperty(ref _isPaneOpen, value);
        }
        public ICommand OnInitializedCommand { get; }
        public ICommand OnMenuTappendCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand PaneToOpenCommand { get; }
        public ICommand PaneToCloseCommand { get; }
        public ObservableCollection<ViewModelBase> ContentStack { get; } = [];

        public MenuItem _selectedMenuItem;
        public MenuItem SelectedMenuItem
        {
            get=> _selectedMenuItem;
            set => SetProperty(ref _selectedMenuItem, value);
        }

        public MainViewModel()
        {
            
        }
        public MainViewModel(IMenuNavigationService menuNavigationService)
        {

            _menuNavigationService = menuNavigationService;
            OnInitializedCommand = new RelayCommand(OnInitialized);
            OnMenuTappendCommand = new RelayCommand(OnMenuTapped);
            GoBackCommand = new RelayCommand(GoBack);
            PaneToOpenCommand = new RelayCommand(PaneToOpen);
            PaneToCloseCommand = new RelayCommand(PaneToClose);
        }

        private void OnMenuTapped()
        {
            if (SelectedMenuItem is null) 
            {
                return;
            }
            _menuNavigationService.NavigateTo(SelectedMenuItem.View);
            Title = SelectedMenuItem.Name;
            IsPaneOpen = false;
        }

        private void PaneToClose()
        {
            IsPaneOpen = false;
        }

        private void PaneToOpen()
        {
            IsPaneOpen = true;
        }

        private void OnInitialized()
        {
            _menuNavigationService.NavigateTo(MenuNavigationConstant.TodayView);
        }
        public void PushContent(ViewModelBase content)
        {
            Content = content;
            ContentStack.Add(content);
        }
        public void PopContent()
        {
            if (ContentStack.Count <=1)
            {
                return;
            }
            ContentStack.RemoveAt(ContentStack.Count-1);
            Content = ContentStack[ContentStack.Count-1];
        }
        public void SetMenuAndContent(string view, ViewModelBase content)
        {
            ContentStack.Clear();
            PushContent(content);
            SelectedMenuItem = MenuItem.MenuItems.FirstOrDefault(p => p.View == view);
            Title = SelectedMenuItem.Name;
            IsPaneOpen = false;
        }
        public void SetContent(string view, ViewModelBase content)
        {
            PushContent(content);
        }

        public void GoBack() => PopContent();

    }

    public class MenuItem
    {
        public string View { get; private init; }
        public string Name { get; private init; }

        // 只有自己能初始化自己，用来承载初始化后的静态数据
        private MenuItem() { }

        private static MenuItem TodayView => new MenuItem { Name = "今日推荐", View = MenuNavigationConstant.TodayView };

        private static MenuItem QueryView => new MenuItem { Name = "诗词搜索", View = MenuNavigationConstant.QueryView };

        private static MenuItem FavoriteView => new MenuItem { Name = "诗词收藏", View = MenuNavigationConstant.FavoriteView };

        public static IEnumerable<MenuItem> MenuItems { get; } = new[] { TodayView, QueryView, FavoriteView };
    }
}

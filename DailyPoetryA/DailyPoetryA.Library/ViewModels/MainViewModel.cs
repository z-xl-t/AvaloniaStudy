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
    public class MainViewModel: ViewModelBase
    {
        private ViewModelBase _content;
        public ViewModelBase Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }
        private string _title;
        public string Title
        {
            get => _title;
            private set => SetProperty(ref _title, value);
        }


        public ICommand OnInitializedCommand { get; }

        public MainViewModel()
        {
            Title = "1";
            OnInitializedCommand = new RelayCommand(OnInitialized);
        }

        private void OnInitialized()
        {

        }
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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DailyPoetryA.Library.ViewModels
{
    public class QueryViewModel: ViewModelBase
    {
        public ObservableCollection<FilterViewModel> FilterViewModelCollection { get; }
        public QueryViewModel()
        {
            FilterViewModelCollection = new ObservableCollection<FilterViewModel>();
            FilterViewModelCollection.Add(new FilterViewModel(this));
        }

        public void AddFilterViewModel(FilterViewModel filterViewModel)
        {
            var idx = FilterViewModelCollection.IndexOf(filterViewModel);

            FilterViewModelCollection.Insert(idx+1,new FilterViewModel(this));
        }
        public void DeleteFilterViewModel(FilterViewModel filterViewModel)
        {
            FilterViewModelCollection.Remove(filterViewModel);
            if (FilterViewModelCollection.Count == 0)
            {
                FilterViewModelCollection.Add(new FilterViewModel(this));
            }
        }
    }

    public class FilterViewModel: ObservableObject
    {
        private string _content;
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }

        public readonly QueryViewModel _queryViewModel;

        public FilterViewModel(QueryViewModel queryViewModel)
        {
            _queryViewModel = queryViewModel;
            AddCommand = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete);
        }

        public void Add ()
        {
            _queryViewModel.AddFilterViewModel(this);
        }
        public void Delete()
        {
            _queryViewModel.DeleteFilterViewModel(this);
        }
    }

    public class FilterType
    {
        // 私有化构造函数，只能在此类中进行实例创建。
        private FilterType(string name, string propertyName)
        {
            Name = name;
            PropertyName = propertyName;
        }

        public string Name { get; set; }
        public string PropertyName { get; set; }

        // 类本身只有一份
        // 静态成员跟类是绑定的，这样可以确保数据对象的唯一性
        public static readonly FilterType NameFilter = new("标题", nameof(Poetry.Name));
        public static readonly FilterType AuthorNameFilter = new("作者", nameof(Poetry.Author));
        public static readonly FilterType ContentFilter = new("内容", nameof(Poetry.Content));
        public static List<FilterType> FilterTypes { get; } = new List<FilterType> { NameFilter, AuthorNameFilter, ContentFilter };
    }
}

using AvaloniaInfiniteScrolling;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DailyPoetryA.Library.ViewModels
{
    public class ResultViewModel : ViewModelBase
    {
        private readonly IPoetryStorage _poetryStorage;
        public  int PageSize {get;} = 20;
        private string _status;
        public string Status { 
            get { return _status; }
            private set => SetProperty(ref _status, value);
        
        }

        private bool _canLoadMore = true;
        public AvaloniaInfiniteScrollCollection<Poetry> PoetryCollection { get; }
        public ResultViewModel(IPoetryStorage poetryStorage)
        {
            _poetryStorage = poetryStorage;
            PoetryCollection = new AvaloniaInfiniteScrollCollection<Poetry>();
            PoetryCollection.OnLoadMore = LoadMoreData;
            PoetryCollection.OnCanLoadMore = CanLoadMoreData;
        }

        private bool CanLoadMoreData()
        {
            return _canLoadMore;
        }

        public async Task<IEnumerable<Poetry>> LoadMoreData()
        {
            Status = LoadStatus.Loading;
            var poetryies = await _poetryStorage.GetPoetryListAsync(
                 Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true), Expression.Parameter(typeof(Poetry), "p"))
                 , PoetryCollection.Count, PageSize);
            Status = LoadStatus.Empty;
            if (poetryies.Count < PageSize)
            {
                _canLoadMore = false;
                Status = LoadStatus.NoMoreREsult;
            }
            if (PoetryCollection.Count == 0 && poetryies.Count == 0) {
                Status = LoadStatus.NoResult;
            }
            return poetryies;
        }
    }
    
    public static class LoadStatus
    {
        public const string Empty = "";
        public const string Loading = "正在载入";
        public const string NoResult = "没有满足条件的结果";
        public const string NoMoreREsult = "没有更多结果";
    }
}

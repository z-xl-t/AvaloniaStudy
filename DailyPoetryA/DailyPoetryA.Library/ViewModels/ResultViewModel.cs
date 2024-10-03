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
        public ObservableCollection<Poetry> PoetryCollection { get; } = new();

        // 这种写法会导致每次调用时，生成一个新的对象
        // public ObservableCollection<Poetry> PoetryCollection2 => new();

        public ResultViewModel(IPoetryStorage poetryStorage)
        {
            _poetryStorage = poetryStorage;

            // 构造函数可以赋初始值
            OnInitializedCommad = new AsyncRelayCommand(OnInitializedAsync);

            if (!_poetryStorage.IsInitialized)
            {
                Task.Run(async () => { await _poetryStorage.InitializeAsync(); });
            }
        }

        public ICommand OnInitializedCommad { get;}
        public async Task OnInitializedAsync()
        {
            var poetries = await _poetryStorage.GetPoetryListAsync(
               Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true), Expression.Parameter(typeof(Poetry), "p")),
               0,
               int.MaxValue);

            foreach (var poetry in poetries)
            {
                PoetryCollection.Add(poetry);
            }
        }

    }
}

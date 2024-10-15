using DailyPoetry.UnitTest.Helpers;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.UnitTest.ViewModels
{
    public class ResultVireModelTest : IDisposable
    {
        public ResultVireModelTest()
        {
            PoetryStorageHelper.RemoveDatabaseFile();
        }
        public void Dispose()
        {
            PoetryStorageHelper.RemoveDatabaseFile();
        }

        [Fact]
        public async Task PoetryCollection_Default()
        {
            var where = Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true), Expression.Parameter(typeof(Poetry), "p"));

            var poetryStorage = await PoetryStorageHelper.GetInitializedPoetryStorage();
            var resultViewModel = new ResultViewModel(poetryStorage);

            var statusList = new List<string>();

            resultViewModel.PropertyChanged += (sender, args) =>
            {
                if(args.PropertyName == nameof(resultViewModel.Status))
                {
                    statusList.Add(resultViewModel.Status);
                }
            };


            Assert.Empty(resultViewModel.PoetryCollection);
            await resultViewModel.PoetryCollection.LoadMoreAsync();

            Assert.Equal(resultViewModel.PageSize, resultViewModel.PoetryCollection.Count);
            Assert.Equal("观书有感", resultViewModel.PoetryCollection.Last().Name);
            Assert.True(resultViewModel.PoetryCollection.CanLoadMore);
            Assert.Equal(2, statusList.Count);


            await resultViewModel.PoetryCollection.LoadMoreAsync();

            Assert.Equal(30, resultViewModel.PoetryCollection.Count);
            Assert.Equal("记承天寺夜游", resultViewModel.PoetryCollection[29].Name);
            Assert.False(resultViewModel.PoetryCollection.CanLoadMore);
            Assert.Equal(5, statusList.Count);









            await poetryStorage.CloseAsync();

        }
    }
}

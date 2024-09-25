using DailyPoetryA.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Services
{
    public interface IPoetryStorage
    {
        bool IsInitialized {  get; }
        Task InitializeAsync();
        Task<Poetry> GetPoetryAsync(int id);
        Task<IList<Poetry>> GetPoetryListAsync(
            Expression<Func<Poetry, bool>> where, int skip, int take
            );
    }
}

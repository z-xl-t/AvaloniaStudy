using MVVM.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVVM.Services
{
    public interface IPoetryStorage
    {
        Task InitialzeAsync();
        Task InsertAsync(Poetry poetry);

        Task<IList<Poetry>> ListAsync();                                                                                                                                                          
        Task<IList<Poetry>> QueryAsync(string keyword);
    }
}

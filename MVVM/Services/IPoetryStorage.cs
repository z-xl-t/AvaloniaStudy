using MVVM.Models;
using System.Threading.Tasks;

namespace MVVM.Services
{
    public interface IPoetryStorage
    {
        Task InitialzeAsync();
        Task InsertAsync(Poetry poetry);
    }
}

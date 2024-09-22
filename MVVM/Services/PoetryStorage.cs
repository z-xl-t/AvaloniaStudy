using MVVM.Helpers;
using MVVM.Models;
using SQLite;
using System;
using System.Threading.Tasks;

namespace MVVM.Services
{
    public class PoetryStorage : IPoetryStorage
    {
        public const string DbName = "poetrydb.sqlite3";

        public static readonly string PoetryDbPath = 
            PathHepler.GetLocalFilePath(DbName);


        private SQLiteAsyncConnection _connection;
        private SQLiteAsyncConnection Connection => 
            _connection ??= new SQLiteAsyncConnection(PoetryDbPath);

        public async Task InitialzeAsync()
        {
            await Connection.CreateTableAsync<Poetry>();
        }

        public async Task InsertAsync(Poetry poetry)
        {
            await Connection.InsertAsync(poetry);
        }
    }
}

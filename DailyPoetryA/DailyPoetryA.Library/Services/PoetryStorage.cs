using DailyPoetryA.Library.Helpers;
using DailyPoetryA.Library.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Services
{
    public class PoetryStorage : IPoetryStorage
    {
        // 写死数据库里的数据个数，学习用；
        public const int NumberPoetry = 30;
        public const string DbName = "poetry.sqlite3";
        public static readonly string PoetryDbPath = PathHelper.GetLocalFilePath(DbName);

        private SQLiteAsyncConnection _connection;
        private SQLiteAsyncConnection connection =>
            _connection ??= new SQLiteAsyncConnection(PoetryDbPath);


        public bool IsInitialized { get; }

        public Task<Poetry> GetPoetryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Poetry>> GetPoetryListAsync(Expression<Func<Poetry, bool>> where, int skip, int take)
        {
            throw new NotImplementedException();
        }

        public async Task InitializeAsync()
        {

            // 嵌入的资源，需要以流的形式写入指定路径
            await using var dbFileStream = 
                new FileStream(PoetryDbPath, FileMode.OpenOrCreate);
            await using var dbAssetSteam = 
                typeof(PoetryStorage).Assembly.GetManifestResourceStream(DbName);
            await dbAssetSteam.CopyToAsync(dbFileStream);
            
            
        }
    }
}

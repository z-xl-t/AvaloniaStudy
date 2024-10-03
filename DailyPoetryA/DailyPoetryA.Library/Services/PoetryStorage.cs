using DailyPoetryA.Library.Helpers;
using DailyPoetryA.Library.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public const string DbName = "poetrydb.sqlite3";
        public static readonly string PoetryDbPath = PathHelper.GetLocalFilePath(DbName);

        private SQLiteAsyncConnection _connection;
        private SQLiteAsyncConnection Connection =>
            _connection ??= new SQLiteAsyncConnection(PoetryDbPath);


        private readonly IPreferenceStorage _preferenceStorage;

        public PoetryStorage(IPreferenceStorage preferenceStorage)
        {
            _preferenceStorage = preferenceStorage;

            if (!IsInitialized) {
                Task.Run(async () => { await InitializeAsync(); });
            }
        }


        public bool IsInitialized =>
            _preferenceStorage.Get(PoetryStorageConstant.VersionKey, 
                default(int)) == PoetryStorageConstant.Version;

        public async Task<Poetry> GetPoetryAsync(int id)
        {
            return await Connection.Table<Poetry>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IList<Poetry>> GetPoetryListAsync(Expression<Func<Poetry, bool>> where, int skip, int take)
        {
            return await Connection.Table<Poetry>().Where(where).Skip(skip).Take(take).ToListAsync();
        }

        public async Task InitializeAsync()
        {

            // 嵌入的资源，需要以流的形式写入指定路径
            await using var dbFileStream = 
                new FileStream(PoetryDbPath, FileMode.OpenOrCreate);
            //foreach (string resourceName in typeof(PoetryStorage).Assembly.GetManifestResourceNames())
            //{
            //    Debug.WriteLine(resourceName);
            //}


            await using var dbAssetSteam = 
                typeof(PoetryStorage).Assembly.GetManifestResourceStream(DbName);
            await dbAssetSteam.CopyToAsync(dbFileStream);

            _preferenceStorage.Set(PoetryStorageConstant.VersionKey, PoetryStorageConstant.Version);

        }

        // 不是业务要求，不修改接口，只在业务类中创建
        public async Task CloseAsync() => await Connection.CloseAsync();
    }
    // 静态类充当全局变量包
    public static class PoetryStorageConstant
    {
        public const int Version = 1;

        public const string VersionKey = nameof(PoetryStorageConstant) + "." + nameof(Version);
    }
}

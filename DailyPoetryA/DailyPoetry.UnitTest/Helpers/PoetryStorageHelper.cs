using DailyPoetryA.Library.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.UnitTest.Helpers
{
    public static class PoetryStorageHelper
    {
        public static void RemoveDatabaseFile() =>
            File.Delete(PoetryStorage.PoetryDbPath);
    
        // 准备测试环境的数据库， 本例中其实就是拷贝数据库文件过去。
        public static async Task<PoetryStorage> GetInitializedPoetryStorage()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();

            // 假装版本不一致
            preferenceStorageMock.Setup(P => P.Get(PoetryStorageConstant.VersionKey, -1)).Returns(-1);
            var mockPreferenceStorage = preferenceStorageMock.Object;
            var poetryStorage = new PoetryStorage(mockPreferenceStorage);
            
            // 初始化数据库
            await poetryStorage.InitializeAsync();
            return poetryStorage;


        }
    
    }
}

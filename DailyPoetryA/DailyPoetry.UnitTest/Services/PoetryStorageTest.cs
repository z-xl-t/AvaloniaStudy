using DailyPoetry.UnitTest.Helpers;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.UnitTest.Services
{
    // 继承 IDisposable 接口，c# 的垃圾自动收集机制，会在类销毁前触发 Dispose 函数
    public class PoetryStorageTest: IDisposable
    {

        // 最好在测试之前，也提前清理资源， 防御性编程
        public PoetryStorageTest()
        {
            PoetryStorageHelper.RemoveDatabaseFile();
        }

        // 执行单元测试完成后，要清理痕迹
        public void Dispose()
        {
            PoetryStorageHelper.RemoveDatabaseFile();
        }

        // 各种 test 命名规范
        // TestInit
        // Init_Success
        // Init_Fail
        // cameCase
        // CameCase
        // came_case  

        [Fact]
        public async Task InitialzeAsync_Default()
        {
            // Mock 模拟 IPreferenceStorage，这是类依赖接口，而不去依赖另一个类的原因。
            // 可以生成一个模拟的空的类实现，传入 PoetryStorage ，从而专注 PoetryStorage 的相关测试
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            var mockPreferenceStorage = preferenceStorageMock.Object;


            var poetryStorage = new PoetryStorage(mockPreferenceStorage);
            Assert.False(File.Exists(PoetryStorage.PoetryDbPath));
            await poetryStorage.InitializeAsync();
            Assert.True(File.Exists(PoetryStorage.PoetryDbPath));
        }

        [Fact]
        public void IsInitialized_Default()
        {
            var preferenceStorageMock = new Mock<IPreferenceStorage>();
            // 模拟接口的函数行为
            preferenceStorageMock
                .Setup(p => p.Get(PoetryStorageConstant.VersionKey, default(int)))
                .Returns(PoetryStorageConstant.Version);

            var mockPreferenceStorage = preferenceStorageMock.Object;
            var poetryStorage = new PoetryStorage(mockPreferenceStorage);
            Assert.True(poetryStorage.IsInitialized);

            // 验证 poetryStorage.IsInitialized 是否调用了一次 Get 函数
            preferenceStorageMock.Verify(p =>
            p.Get(PoetryStorageConstant.VersionKey, default(int)), Times.Once());


            //preferenceStorageMock.Verify(p =>
            //p.Get(PoetryStorageConstant.VersionKey, default(int)), Times.Never());
        }

        [Fact]
        public async Task GetPoetryAsync_Default()
        {
            // 涉及到数据库的测试，必须需要连接数据库
            var poetryStorage = await PoetryStorageHelper.GetInitializedPoetryStorage();
            var poetry = await poetryStorage.GetPoetryAsync(10001);
            Assert.Equal("临江仙 · 夜归临皋", poetry.Name);

            // 手动关闭数据库连接
            await poetryStorage.CloseAsync();
        }

        [Fact]
        public async Task GetPoetriesAsync_Default()
        {
            var poetryStorage = await PoetryStorageHelper.GetInitializedPoetryStorage();
            var poetries = await poetryStorage.GetPoetryListAsync(
                Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),Expression.Parameter(typeof(Poetry), "p")), 
                0, 
                int.MaxValue);
            Assert.Equal(PoetryStorage.NumberPoetry, poetries.Count());
            await poetryStorage.CloseAsync();

        }
    }
}

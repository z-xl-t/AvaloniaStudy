using DailyPoetry.UnitTest.Helpers;
using DailyPoetryA.Library.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetry.UnitTest.Services
{
    public class JinrishiciServiceTest : IDisposable
    {
        public JinrishiciServiceTest()
        {
                
        }
        public void Dispose()
        {

        }

        [Fact(Skip = "依赖远程服务的测试")]
        public async Task GetTokenAsync_ReturnIsNotNullOrWhiteSpace()
        {
            var alertServiceMock = new Mock<IAlertService>();
            var mockAlertService = alertServiceMock.Object;

            var preferenceServiceeMock = new Mock<IPreferenceStorage>();
            var mockPreferenceServicee = preferenceServiceeMock.Object;

            var poetryStorageMock = new Mock<IPoetryStorage>();
            var mockPoetryStorage = poetryStorageMock.Object;


            var jinrishici = new JinrishiciService(mockAlertService, mockPreferenceServicee, mockPoetryStorage);
            var token = await jinrishici.GetTokenAsync();

            Assert.False(string.IsNullOrWhiteSpace(token));
            
            // 没有异常
            alertServiceMock.Verify(
                p => p.AlertAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            // 调用过get和set函数，来获取和存储
            preferenceServiceeMock.Verify(p => p.Get(JinrishiciService.JinrishiciTokenKey,string.Empty), Times.Once);
            preferenceServiceeMock.Verify(p => p.Set(JinrishiciService.JinrishiciTokenKey, token), Times.Once);

        }

        [Fact]
        public async Task GetTokenAsync_NetWorkError()
        {
            var alertServiceMock = new Mock<IAlertService>();
            var mockAlertService = alertServiceMock.Object;

            var preferenceServiceeMock = new Mock<IPreferenceStorage>();
            var mockPreferenceServicee = preferenceServiceeMock.Object;

            var poetryStorageMock = new Mock<IPoetryStorage>();
            var mockPoetryStorage = poetryStorageMock.Object;

            // 通过网址错误来模拟网络错误
            var jinrishici = new JinrishiciService(
                mockAlertService, 
                mockPreferenceServicee, mockPoetryStorage, "http://no.such.url");
            var token = await jinrishici.GetTokenAsync();

            // token 为空
            Assert.True(string.IsNullOrWhiteSpace(token));

            // 没有异常
            alertServiceMock.Verify(
                p => p.AlertAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            // 
            preferenceServiceeMock.Verify(p => p.Get(JinrishiciService.JinrishiciTokenKey, string.Empty), Times.Once);
            preferenceServiceeMock.Verify(p => p.Set(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

        }

        [Fact(Skip = "依赖远程服务的测试")]
        public async Task GetTodayPoetryAsync_ReturnFromJinrishici()
        {
            var alertServiceMock = new Mock<IAlertService>();
            var mockAlertService = alertServiceMock.Object;

            var preferenceServiceeMock = new Mock<IPreferenceStorage>();
            var mockPreferenceServicee = preferenceServiceeMock.Object;

            var poetryStorageMock = new Mock<IPoetryStorage>();
            var mockPoetryStorage = poetryStorageMock.Object;

            var jinrishiciService = new JinrishiciService(
                mockAlertService,
                mockPreferenceServicee,
                mockPoetryStorage);

            var todayPoetry = await jinrishiciService.GetTodayPoetryAsync();
            
            // 验证是从服务器获取
            Assert.Equal(TodayPoetySources.Jinrishici, todayPoetry.Source);
            // 验证数据不为空
            Assert.False(string.IsNullOrEmpty(todayPoetry.Snippet));

            // 没有异常
            alertServiceMock.Verify(
                p => p.AlertAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);

            preferenceServiceeMock.Verify(p => p.Get(JinrishiciService.JinrishiciTokenKey, string.Empty), Times.Once);
            preferenceServiceeMock.Verify(p => p.Set(JinrishiciService.JinrishiciTokenKey, It.IsAny<string>()), Times.AtMostOnce);
        }
        [Fact]
        public async Task GetRandomPoetryAsync_Default()
        {
            var poetryStorage = await PoetryStorageHelper.GetInitializedPoetryStorage();
            var jinrishiciService = new JinrishiciService(null, null, poetryStorage);
            var randomPoetry = await jinrishiciService.GetRandomPoetryAsync();
            Assert.NotNull(randomPoetry);
            Assert.False(string.IsNullOrWhiteSpace(randomPoetry.Name));

            await poetryStorage.CloseAsync();
            PoetryStorageHelper.RemoveDatabaseFile();
        }
    }
}

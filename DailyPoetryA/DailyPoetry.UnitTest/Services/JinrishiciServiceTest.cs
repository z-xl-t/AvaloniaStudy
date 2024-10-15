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

        [Fact]
        public async Task GetTokenAsync_Default()
        {
            var alertServiceMock = new Mock<IAlertService>();
            var mockAlertService = alertServiceMock.Object;

            var jinrishici = new JinrishiciService(mockAlertService);
            var jsonStr = await jinrishici.GetTokenAsync();
            Assert.False(string.IsNullOrWhiteSpace(jsonStr));

            // 模拟域名出错
            var jinrishici2 = new JinrishiciService(mockAlertService, "aaa");
            var jsonStr2 = await jinrishici2.GetTokenAsync();

            // 验证是否调用了一次 
            alertServiceMock.Verify(a =>
            a.AlertAsync("", "An invalid request URI was provided. Either the request URI must be an absolute URI or BaseAddress must be set."), Times.Once());

        }
    }
}

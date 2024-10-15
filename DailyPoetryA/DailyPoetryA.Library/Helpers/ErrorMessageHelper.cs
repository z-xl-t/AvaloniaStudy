using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Helpers
{
    public class ErrorMessageHelper
    {
        public const string HttpClientErrorTitle = "连接错误";
        public static string GetHttpClientError(string server, string message) => $"与{server}连接时发生错误\n{message}";

        public const string JsonDeserializationErrorTitle = "读取数据错误";
        public static string GetJsonDeserializationError(string server, string message) => $"{server}读取数据时发生了错误:\n {message}";

        public const string JsonDeserializationErrorButton = "确定";
    
            
    }
}

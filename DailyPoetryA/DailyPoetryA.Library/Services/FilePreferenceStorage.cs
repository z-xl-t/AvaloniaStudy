using DailyPoetryA.Library.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Services
{
    public class FilePreferenceStorage : IPreferenceStorage
    {
        public int Get(string key, int defaultValue)
        {
            return int.TryParse(Get(key, string.Empty), out var value) ? value : defaultValue;
        }

        public string Get(string key, string defaultValue)
        {
            var path = PathHelper.GetLocalFilePath(key);
            return File.Exists(path) ? File.ReadAllText(path) : defaultValue;
        }

        public DateTime Get(string key, DateTime defaultValue)
        {
            return DateTime.TryParse(Get(key, string.Empty), out var value) ? value : defaultValue;
        }

        public void Set(string key, int value)
        {
            Set(key, value.ToString());
        }

        public void Set(string key, string value)
        {
            var path = PathHelper.GetLocalFilePath(key);
            File.WriteAllText(path, value);
        }

        public void Set(string key, DateTime value)
        {
            Set(key, value.ToString());
        }
    }
}

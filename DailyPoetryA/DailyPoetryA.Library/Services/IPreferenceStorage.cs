using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Services
{
    public interface IPreferenceStorage
    {
        void Set(string key, int value);
        void Set(string key, string value);
        void Set(string key, DateTime value);
        int Get(string key, int defaultValue);
        string Get(string key, string defaultValue);
        DateTime Get(string key, DateTime defaultValue);

    }
}

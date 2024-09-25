using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Helpers
{
    public static class PathHelper
    {
        private static string _localFolder = string.Empty;

        private static string LocalFolder
        {
            get
            {
                if (!string.IsNullOrEmpty(_localFolder))
                {
                    return _localFolder;
                }

                _localFolder =
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder
                            .LocalApplicationData), nameof(DailyPoetryA));

                if (!Directory.Exists(_localFolder))
                {
                    Directory.CreateDirectory(_localFolder);
                }

                return _localFolder;
            }
        }

        public static string GetLocalFilePath(string fileName)
        {
            return Path.Combine(LocalFolder, fileName);
        }
    }
}

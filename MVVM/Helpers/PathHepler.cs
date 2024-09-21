using System;
using System.IO;

namespace MVVM.Helpers
{
    public static class PathHepler
    {
        private static string _localFolder = string.Empty;

        public static string LocalFolder
        {
            get
            {
                if (!string.IsNullOrEmpty(_localFolder))
                {
                    return _localFolder;
                }
                _localFolder = Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData), nameof(MVVM));

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

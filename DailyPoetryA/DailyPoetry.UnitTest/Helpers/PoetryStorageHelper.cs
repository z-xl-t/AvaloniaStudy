using DailyPoetryA.Library.Services;
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
    }
}

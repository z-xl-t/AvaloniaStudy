using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.Models
{
    public class TodayImage
    {
        public string FullStartDate { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string Copyright { get; set; } = string.Empty;
        public string CopyrightLink { get; set; } = string.Empty;
        public byte[] ImageBytes { get; set; }
    }
}

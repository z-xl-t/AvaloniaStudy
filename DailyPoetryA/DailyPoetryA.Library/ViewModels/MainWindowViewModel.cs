using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.ViewModels
{
    public class MainWindowViewModel: ViewModelBase
    {
        private ViewModelBase _content;
        public ViewModelBase Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }
    }
}

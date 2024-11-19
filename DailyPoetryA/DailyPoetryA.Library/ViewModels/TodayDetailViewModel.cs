using DailyPoetryA.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyPoetryA.Library.ViewModels
{
    public class TodayDetailViewModel : ViewModelBase
    {
        private TodayPoetry _todayPoetry;
        public TodayPoetry TodayPoetry
        {
            get => _todayPoetry;
            set => SetProperty(ref _todayPoetry, value);
        }
        public override void SetParameter(object parameter)
        {
            TodayPoetry = parameter as TodayPoetry;
            base.SetParameter(parameter);
        }
    }
}

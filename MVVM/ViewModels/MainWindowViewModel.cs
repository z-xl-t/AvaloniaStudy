using CommunityToolkit.Mvvm.Input;
using MVVM.Models;
using MVVM.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
      
        public MainWindowViewModel(IPoetryStorage poetryStorage)
        {
            _poetryStorage = poetryStorage;
            SayHelloCommand = new RelayCommand(SayHello);
            InitialzeCommand = new AsyncRelayCommand(InitialzeAsync);
            InsertCommand = new AsyncRelayCommand(InsertAsync);
            ListCommand = new AsyncRelayCommand(ListAsync);
        }
        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
        private readonly IPoetryStorage _poetryStorage;
        public ObservableCollection<Poetry> Poetries { get; set; } = new();

        public ICommand SayHelloCommand { get; }

        public ICommand InitialzeCommand { get; }

        public ICommand InsertCommand { get; }

        public ICommand ListCommand { get; }

        public void SayHello()
        {
            Message = "Hello Avalonia!";
        }

        public async Task InitialzeAsync()
        {
            // 创建数据库
            await _poetryStorage.InitialzeAsync();
        }


        public async Task InsertAsync()
        {
            var poetry = new Poetry {
                Name = "Name" + new Random().Next(),
            };

            await _poetryStorage.InsertAsync(poetry);
        }
        public async Task ListAsync()
        {
            var poetries = await _poetryStorage.ListAsync();
            Poetries.Clear();
            foreach (var poetry in poetries) {
                Poetries.Add(poetry);
            }

        }
    }
}

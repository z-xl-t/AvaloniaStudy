using CommunityToolkit.Mvvm.Input;
using MVVM.Services;
using System.Windows.Input;

namespace MVVM.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IPoetryStorage _poetryStorage;

        public MainWindowViewModel(IPoetryStorage poetryStorage)
        {
            _poetryStorage = poetryStorage;

            SayHelloCommand = new RelayCommand(SayHello);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }
        public ICommand SayHelloCommand { get; }

        public void SayHello()
        {
            Message = "Hello Avalonia!";
        }
    }
}

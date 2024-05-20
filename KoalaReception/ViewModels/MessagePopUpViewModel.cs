using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KoalaReception.Views;
using System.Windows.Input;

namespace KoalaReception.ViewModels
{
    public partial class MessagePopupViewModel : ObservableObject
    {
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ICommand ReturnToHomeCommand { get; }
        private MessagePopup _popup;

        public MessagePopupViewModel(MessagePopup popup)
        {
            ReturnToHomeCommand = new AsyncRelayCommand(OnReturnToHome);
            _popup = popup;

        }

        public void ShowMessage(string message)
        {
            Message = message;
        }

        private async Task OnReturnToHome()
        {
            _popup.Close();
            await Shell.Current.GoToAsync("..");
        }
    }
}

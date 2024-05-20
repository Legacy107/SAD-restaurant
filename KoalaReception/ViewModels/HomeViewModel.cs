using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using KoalaReception.Models;
using System.Text.Json;

namespace KoalaReception.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private string _changeReservationId;
        private string _checkinReservationId;
        public ICommand MakeReservationCommand { get; private set; }
        public ICommand ChangeReservationCommand { get; private set; }
        public ICommand CheckInCommand { get; private set; }
        public ICommand CheckInWithReservationCommand { get; private set; }
        private Reservation _reservation;
        private bool _invalidReservationId;
        private bool _isChangeReservationEnabled;
        private bool _isCheckInReservationEnabled;
        private bool _invalidCheckInReservationId;

        public string ChangeReservationId
        {
            get => _changeReservationId;
            set
            {
                if (_changeReservationId != value)
                {
                    _changeReservationId = value;
                }
                IsChangeReservationEnabled = _changeReservationId != "";
                InvalidReservationId = false;
            }
        }

        public string CheckInReservationId
        {
            get => _checkinReservationId;
            set
            {
                if (_checkinReservationId != value)
                {
                    _checkinReservationId = value;
                }
                IsCheckInReservationEnabled = _checkinReservationId != "";
                InvalidCheckInReservationId = false;
            }
        }

        public bool InvalidReservationId
        {
            get => _invalidReservationId;
            set
            {
                _invalidReservationId = value;
                OnPropertyChanged();
            }
        }

        public bool InvalidCheckInReservationId
        {
            get => _invalidCheckInReservationId;
            set
            {
                _invalidCheckInReservationId = value;
                OnPropertyChanged();
            }
        }

        public bool IsChangeReservationEnabled
        {
            get => _isChangeReservationEnabled;
            set
            {
                _isChangeReservationEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsCheckInReservationEnabled
        {
            get => _isCheckInReservationEnabled;
            set
            {
                _isCheckInReservationEnabled = value;
                OnPropertyChanged();
            }
        }

        public HomeViewModel(Reservation reservation)
        {
            MakeReservationCommand = new AsyncRelayCommand(MakeReservation);
            ChangeReservationCommand = new AsyncRelayCommand(ChangeReservation);
            CheckInCommand = new AsyncRelayCommand(CheckingIn);
            CheckInWithReservationCommand = new AsyncRelayCommand(CheckInWithReservation);
            _reservation = reservation;
            _isChangeReservationEnabled = false;
            _isCheckInReservationEnabled = false;
            _checkinReservationId = "";
            _changeReservationId = "";
        }

        private async Task MakeReservation()
        {
            await Shell.Current.GoToAsync($"{nameof(Views.ReservationPage)}");
        }

        private async Task ChangeReservation()
        {
            var reservationDetails = await _reservation.CheckReservation(_changeReservationId);
            if (reservationDetails != null)
            {
                await Shell.Current.GoToAsync($"{nameof(Views.ReservationPage)}?reservationDto={Uri.EscapeDataString(JsonSerializer.Serialize(reservationDetails))}");
            } else
            {
                InvalidReservationId = true;
            }
        }

        private async Task CheckingIn()
        {
            await Shell.Current.GoToAsync($"{nameof(Views.CheckInPage)}");
        }

        private async Task CheckInWithReservation()
        {
            Guid id;
            if (Guid.TryParse(CheckInReservationId, out id))
            {
                await Shell.Current.GoToAsync($"{nameof(Views.CheckInPage)}?reservationId={id}");
            } else
            {
                InvalidCheckInReservationId = true;
            }
        }
    }
}

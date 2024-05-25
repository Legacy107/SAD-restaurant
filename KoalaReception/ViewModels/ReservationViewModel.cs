using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KoalaReception.Models;
using KoalaReception.Models.DTO;
using KoalaReception.Views;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json;
using System.Windows.Input;

namespace KoalaReception.ViewModels
{
    public class ReservationViewModel : ObservableObject, IQueryAttributable
    {
        private ObservableCollection<string> _bookingPeriods;
        private string _selectedDateTime;
        private string _customerName = "";
        private Reservation _reservation;
        private ObservableCollection<TableDTO> _tables;
        private List<int> _disabledTableIds;
        private bool _areInputsValid;
        private ReservationDTO _oldReservationDetails;
        private IList<object> _previousSelectedTables;

        public IList<object> SelectedTables { get; set; }
        public ICommand SelectionChangedCommand { get; set; }
        public ICommand SubmitReservationCommand { get; set; }
        public ICommand UpdateTableFilteringCommand { get; set;}

        private void SelectionChanged(object obj)
        {
            if (SelectedTables != null)
            {
                if (SelectedTables.Any())
                {
                    var toBeRemovedList = new List<object>();
                    foreach (var table in SelectedTables)
                    {
                        if (_disabledTableIds.Contains((table as TableDTO)!.TableId))
                        {
                            // Deselect the disabled item
                            toBeRemovedList.Add(table);
                        }
                    }

                    foreach (var table in toBeRemovedList)
                    {
                        SelectedTables.Remove(table);
                    }
                }


                if (SelectedTables.Count > _previousSelectedTables.Count)
                {
                    var selectedTable = SelectedTables.Except(_previousSelectedTables).ToList()[0] as TableDTO;
                    selectedTable!.Status = TableStatusEnum.Selected;
                }
                else if (SelectedTables.Count < _previousSelectedTables.Count)
                {
                    var selectedTable = _previousSelectedTables.Except(SelectedTables).ToList()[0] as TableDTO;
                    selectedTable!.Status = TableStatusEnum.Available;
                }

                _previousSelectedTables = new List<object>(SelectedTables);

                ValidateTables();
                ValidateAll();
                OnPropertyChanged();
                Tables = new ObservableCollection<TableDTO>(Tables);
            }
        }

        private async Task SubmitReservation()
        {
            var tableIds = new List<int>();
            var bookingID = "";
            foreach (var table in SelectedTables)
            {
                tableIds.Add((table as TableDTO)!.TableId);
            }
            if (_oldReservationDetails.TableIds.Count == 0) // New Reservation
            {
                bookingID = await _reservation.MakeReservation(_customerName, ConvertStringToDateTime(_selectedDateTime), tableIds);
            }
            else // Change reservation
            { 
                await _reservation.ChangeReservation(_oldReservationDetails.Id, _customerName, ConvertStringToDateTime(_selectedDateTime), tableIds);
                bookingID = _oldReservationDetails.Id.ToString();
            }
            DisplayFinishPopUp("Tables have been successfully reserved.\nPlease arrive on time.\nIMPORTANT: Please save the Booking ID, which will be used for your check in:\nBOOKING ID: " + bookingID);
        }

        private void DisplayFinishPopUp(string message)
        {
            var messagePopup = new MessagePopup(message);
            if (Application.Current?.Dispatcher != null)
            {
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    Application.Current.MainPage!.ShowPopup(messagePopup);
                });
            }
            else
            {
                // Fallback for non-MAUI environments or if Dispatcher is null
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current!.MainPage!.ShowPopup(messagePopup);
                });
            }
        }


        private void UpdateTableFiltering(object option)
        {
            foreach (var table in Tables)
            {
                table.OnFiltered = table.Traits == option.ToString();
            }
            Tables = new ObservableCollection<TableDTO>(Tables);
        }

        private bool _isSelectionError;
        public bool IsSelectionError
        {
            get => _isSelectionError;
            set
            {
                _isSelectionError = value;
                OnPropertyChanged();
            }
        }

        private void ValidateTables()
        {
            IsSelectionError = SelectedTables.Count == 0;
        }

        private bool _isPeriodSelected;
        public bool IsPeriodSelected
        {
            get => _isPeriodSelected;
            set
            {
                _isPeriodSelected = value;
                OnPropertyChanged();
            }
        }
        private void ValidatePeriodSelection()
        {
            IsPeriodSelected = _selectedDateTime != "";
        }

        public string CustomerName
        {
            get => _customerName;
            set
            {
                _customerName = value;
                ValidateName();
                ValidateAll();
                OnPropertyChanged();
            }
        }

        private bool _isNameError;
        public bool IsNameError
        {
            get => _isNameError;
            set
            {
                _isNameError = value;
                OnPropertyChanged();
            }
        }

        private void ValidateName()
        {
            IsNameError = CustomerName == "";
        }

        public bool AreInputsValid
        {
            get => _areInputsValid;
            set
            {
                _areInputsValid = value;
                OnPropertyChanged();
            }
        }
        private void ValidateAll()
        {
            AreInputsValid = !IsNameError && IsPeriodSelected && !IsSelectionError;
        }

        public ObservableCollection<string> BookingPeriods
        {
            get { return _bookingPeriods; }
            set
            {
                _bookingPeriods = value;
            }
        }

        public ObservableCollection<TableDTO> Tables
        {
            get => _tables;
            set
            {
                if (_tables != value)
                {
                    _tables = value;
                    OnPropertyChanged();
                }
            }
        }

        public ReservationViewModel(Reservation reservation)
        {
            _selectedDateTime = "";
            _bookingPeriods = new ObservableCollection<string>();
            _tables = new ObservableCollection<TableDTO>();
            _disabledTableIds = new List<int>();
            _reservation = reservation;
            _isSelectionError = true;
            _isNameError = true;
            _areInputsValid = false;
            _oldReservationDetails = new ReservationDTO();
            SelectionChangedCommand = new Command(SelectionChanged);
            SubmitReservationCommand = new AsyncRelayCommand(SubmitReservation);
            UpdateTableFilteringCommand = new Command(UpdateTableFiltering);
            _previousSelectedTables = new List<object>();
            if (SelectedTables != null)
            {
                ValidateTables();
            }

            CalculateBookingPeriods();
        }

        private void CalculateBookingPeriods()
        {
            // Clear existing booking periods
            BookingPeriods.Clear();

            // Get current time and add 2 hours
            var currentTime = DateTime.Now.AddHours(2);

            // Round up to the next :00 or :30 minutes
            var startTime = RoundUpTime(currentTime);

            // Get the end time (9:00 PM)
            var endTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 21, 0, 0);

            // Add booking periods with 30-minute interval
            // Generate time slots for the next two days
            var currentDate = currentTime.Date;
            for (int i = 0; i < 3; i++)
            {
                // Add booking periods with 30-minute interval
                while (startTime <= endTime)
                {
                    BookingPeriods.Add($"{currentDate.ToString("dddd, MMMM d")} - {startTime:h:mm tt}-{startTime.AddHours(2):h:mm tt}");
                    startTime = startTime.AddMinutes(30);
                }

                // Move to the next day
                currentDate = currentDate.AddDays(1);

                // Reset start time to 11:00 AM for the next day
                startTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 11, 0, 0);
                endTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 21, 0, 0);
            }
        }

        private DateTime RoundUpTime(DateTime dateTime)
        {
            // Round up to the next :00 or :30 minutes
            var minute = dateTime.Minute;
            var roundedMinute = 0;
            var hour = dateTime.Hour;
            if (minute <= 30)
            {
                roundedMinute = 30;
            }
            else
            {
                hour += 1;
            }

            if (hour >= 24)
            {
                hour = 23;
            }
            else if (hour < 11)
            {
                hour = 11;
                roundedMinute = 0;
            }
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, roundedMinute, 0);
        }

        private bool _noAvailableTable;
        public bool NoAvailableTable
        {
            get => _noAvailableTable;
            set
            {
                _noAvailableTable = value;
                OnPropertyChanged();
            }
        }

        public string SelectedDateTime
        {
            get { return _selectedDateTime; }
            set
            {
                if (_selectedDateTime != value)
                {
                    _selectedDateTime = value;

                    SelectedTables.Clear();
                    Tables = new ObservableCollection<TableDTO>();
                    Task.Run(async () =>
                    {
                        var tableDTOs = await _reservation.GetTables(ConvertStringToDateTime(value));
                        _disabledTableIds.Clear();
                        _previousSelectedTables.Clear();
                        foreach (var tableDTO in tableDTOs)
                        {
                            if (_oldReservationDetails != null && _oldReservationDetails.TableIds.Contains(tableDTO.TableId) && ConvertStringToDateTime(value) == _oldReservationDetails.ReservationStart)
                            {
                                MainThread.BeginInvokeOnMainThread(() => SelectedTables.Add(tableDTO));
                            } 
                            else if (_oldReservationDetails != null && _oldReservationDetails.TableIds.Contains(tableDTO.TableId) &&
                                ConvertStringToDateTime(value) > _oldReservationDetails.ReservationStart.AddHours(-2) && ConvertStringToDateTime(value) < _oldReservationDetails.ReservationStart.AddHours(2)
                            )
                            {
                                tableDTO.Status = TableStatusEnum.Available;
                            }
                            else if (tableDTO.Status != TableStatusEnum.Available)
                            {
                                _disabledTableIds.Add(tableDTO.TableId);
                            }
                        }
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            NoAvailableTable = !tableDTOs.Any(table => table.Status == TableStatusEnum.Available || table.Status == TableStatusEnum.Selected);
                            Tables = new ObservableCollection<TableDTO>(tableDTOs);
                        });

                    }).ConfigureAwait(false);
                    ValidatePeriodSelection();
                    ValidateAll();
                    OnPropertyChanged();
                }
            }
        }

        private DateTime ConvertStringToDateTime(string value)
        {
            string[] dateAndTimeRange = value.Split('-');

            string datePart = dateAndTimeRange[0].Trim();

            string timeRangePart = dateAndTimeRange[1].Trim();

            DateTime date = DateTime.ParseExact(datePart, "dddd, MMMM d", CultureInfo.InvariantCulture);

            string startTimeString = timeRangePart.Split('-')[0].Trim();

            DateTime startTime = DateTime.ParseExact(startTimeString, "h:mm tt", CultureInfo.InvariantCulture);

            DateTime startDateTime = date.Add(startTime.TimeOfDay);

            return startDateTime;
        }

        private string ConvertDateTimeToString(DateTime dateTime)
        {
            // Format the date part as "dddd, MMMM d"
            string datePart = dateTime.ToString("dddd, MMMM d", CultureInfo.InvariantCulture);

            // Format the start time part as "h:mm tt"
            string startTimePart = dateTime.ToString("h:mm tt", CultureInfo.InvariantCulture);

            // Calculate the end time by adding 2 hours to the start time
            DateTime endTime = dateTime.AddHours(2);

            // Format the end time part as "h:mm tt"
            string endTimePart = endTime.ToString("h:mm tt", CultureInfo.InvariantCulture);

            // Combine the date part, start time part, and end time part with hyphens
            string result = $"{datePart} - {startTimePart}-{endTimePart}";

            return result;
        }


        void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("reservationDto"))
            {
                string jsonReservationDetails = Uri.UnescapeDataString(query["reservationDto"].ToString());
                _oldReservationDetails = JsonSerializer.Deserialize<ReservationDTO>(jsonReservationDetails);
                if (_oldReservationDetails != null)
                {
                    SelectedDateTime = ConvertDateTimeToString(_oldReservationDetails.ReservationStart);
                    CustomerName = _oldReservationDetails.CName!;
                }
            }
        }
    }
}

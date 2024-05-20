using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KoalaReception.Models;
using KoalaReception.Models.DTO;
using KoalaReception.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace KoalaReception.ViewModels
{
    public class CheckInViewModel : ObservableObject, IQueryAttributable
    {
        private CheckIn _checkIn;
        private ObservableCollection<TableDTO> _tables;
        public IList<object> SelectedTables { get; set; }
        private IList<object> _previousSelectedTables;
        public ICommand SelectionChangedCommand { get; set; }

        private List<int> _disabledTableIds;
        public ICommand UpdateTableFilteringCommand { get; set; }
        public ICommand SubmitCheckInCommand { get; set; }

        private bool _noAvailableTable;
        public bool IsTableSelected { get; set; }
        public bool IsSelectionError { get; set; }
        private bool _isCheckInWithoutReservation;
        private Guid _reservationId;
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private string _estimateWaitTime = "";
        public bool IsCheckInWithoutReservation
        {
            get => _isCheckInWithoutReservation;
            set
            {
                _isCheckInWithoutReservation = value;
                OnPropertyChanged();

                string message = "";
                if (!value)
                {
                    new Task(async () =>
                    {
                        await _semaphoreSlim.WaitAsync();
                        try
                        {
                            message = await _checkIn.CheckInWithReservation(_reservationId);
                        }
                        finally
                        {
                            _semaphoreSlim.Release();
                            DisplayFinishPopUp(message);
                        }
                    }).Start();
                }
            }
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

        public bool NoAvailableTable
        {
            get => _noAvailableTable;
            set
            {
                _noAvailableTable = value;
                if (value)
                {
                    TimeSpan? minRemainingTime = Tables
                                                .Select(table => table.AffectedStartingTime!.Value.AddHours(2) - DateTime.Now)
                                                .Min();

                    EstimateWaitTime = "Sorry no tables are available at the moment. Please wait for around " + Math.Ceiling(minRemainingTime.Value.TotalMinutes).ToString() + " minutes";
                }
                OnPropertyChanged();
            }
        }

        public CheckInViewModel(CheckIn checkin, IPopupService popupService)
        {
            _checkIn = checkin;
            UpdateTableFilteringCommand = new Command(UpdateTableFiltering);
            SelectionChangedCommand = new Command(SelectionChanged);
            _disabledTableIds = new List<int>();
            _previousSelectedTables = new List<object>();
            SubmitCheckInCommand = new AsyncRelayCommand(SubmitCheckIn);
            LoadTables();
            IsSelectionError = true;
            IsTableSelected = false;
            IsCheckInWithoutReservation = true;
        }

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
                OnPropertyChanged();
                Tables = new ObservableCollection<TableDTO>(Tables);
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

        public string EstimateWaitTime
        {
            get => _estimateWaitTime;
            set
            {
                _estimateWaitTime = value;
                OnPropertyChanged();
            }
        }

        private void LoadTables()
        {
            new Task(async () =>
            {
                await _semaphoreSlim.WaitAsync();
                try
                {
                    var tableDTOs = await _checkIn.GetTables();
                    foreach (var tableDTO in tableDTOs)
                    {
                        if (tableDTO.Status != TableStatusEnum.Available)
                        {
                            _disabledTableIds.Add(tableDTO.TableId);
                        }
                    }
                    Tables = new ObservableCollection<TableDTO>(tableDTOs);
                    NoAvailableTable = !tableDTOs.Any(table => table.Status == TableStatusEnum.Available);
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            }).Start();
        }

        private void ValidateTables()
        {
            IsSelectionError = SelectedTables.Count == 0;
            IsTableSelected = !IsSelectionError;
            OnPropertyChanged(nameof(IsSelectionError));
            OnPropertyChanged(nameof(IsTableSelected));
        }

        private async Task SubmitCheckIn()
        {
            var tableIds = new List<int>();
            foreach (var table in SelectedTables)
            {
                tableIds.Add((table as TableDTO)!.TableId);
            }

            await _checkIn.CheckInWithoutReservation(tableIds);
            DisplayFinishPopUp("Check In success! Please proceed to table " + string.Join(",", tableIds));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("reservationId"))
            {
                _reservationId = Guid.Parse(query["reservationId"].ToString());
                IsCheckInWithoutReservation = false;
            }
        }
    }
}

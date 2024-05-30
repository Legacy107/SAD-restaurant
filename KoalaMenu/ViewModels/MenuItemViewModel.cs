using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Database.Models;
using KoalaMenu.Models;

namespace KoalaMenu.ViewModels;

public partial class MenuItemViewModel : INotifyPropertyChanged
{
    public Database.Models.MenuItem MenuItem { get; private set;}
    private OrderBuilder orderBuilder;
    private MenuItemVariation? _selectedVariation = null;
    private MenuItemOption? _selectedOption = null;
    private int _quantity = 0;
    private string _note = "";

    public Command AddCommand { get; }
    public ICommand SelectVariationCommand { get; private set; }
    public ICommand SelectOptionCommand { get; private set; }
    public ICommand NoteChangedCommand { get; private set; }
    public ICommand QuantityChangedCommand { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public MenuItemVariation? SelectedVariation
    {
        get => _selectedVariation;
        set
        {
            if (_selectedVariation != value)
            {
                _selectedVariation = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAddButtonEnabled));
            }
        }
    }

    public MenuItemOption? SelectedOption
    {
        get => _selectedOption;
        set
        {
            if (_selectedOption != value)
            {
                _selectedOption = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAddButtonEnabled));
            }
        }
    }

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAddButtonEnabled));
            }
        }
    }

    public string Note
    {
        get => _note;
        set
        {
            if (_note != value)
            {
                _note = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsAddButtonEnabled));
            }
        }
    }

    public bool IsAddButtonEnabled => (
        SelectedOption is not null &&
        SelectedVariation is not null &&
        Quantity > 0 &&
        Quantity < 100
    );

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public MenuItemViewModel(Database.Models.MenuItem menuItem, OrderBuilder _orderBuilder)
    {
        MenuItem = menuItem;
        orderBuilder = _orderBuilder;
        SelectVariationCommand = new RelayCommand<object>(SelectVariation);
        SelectOptionCommand = new RelayCommand<object>(SelectOption);
        NoteChangedCommand = new RelayCommand<object>(NoteChanged);
        QuantityChangedCommand = new RelayCommand<object>(QuantityChanged);
        AddCommand = new Command(Add, () => IsAddButtonEnabled);
        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(IsAddButtonEnabled))
            {
                AddCommand.ChangeCanExecute();
            }
        };
    }

    private void Add()
    {
        if (
            IsAddButtonEnabled &&
            SelectedVariation is not null &&
            SelectedOption is not null
        )
        {
            orderBuilder.AddOrderItem(
                SelectedVariation,
                SelectedOption,
                Quantity,
                Note
            );

            SelectedVariation = null;
            SelectedOption = null;
            Quantity = 0;
            Note = "";
        }
    }

    private void SelectVariation(object? menuVariation)
    {
        if (menuVariation is MenuItemVariation menuItemVariation)
        {
            _selectedVariation = menuItemVariation;
        }
    }

    private void SelectOption(object? menuOption)
    {
        if (menuOption is MenuItemOption menuItemOption)
        {
            SelectedOption = menuItemOption;
        }
    }

    private void NoteChanged(object? note)
    {
        if (note is Entry entry)
        {
            Note = entry.Text;
        }
        else Note = "";
    }

    private void QuantityChanged(object? quantity)
    {
        if (quantity is Entry entry && int.TryParse(entry.Text, out int quantityInt))
        {
            Quantity = quantityInt;
        }
        else Quantity = 0;
    }
}

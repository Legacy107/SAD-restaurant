using Microsoft.Extensions.Configuration;
using Database;
using Database.Data;
using KoalaMenu.Base;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Database.Models;

namespace KoalaMenu.ViewModels;

internal partial class MenuViewModel : ObservableObject
{
	MenuContext _context;
	private Menu menu;
	private Dictionary<int, MenuItemVariation> selectedVariations = new();
	private Dictionary<int, MenuItemOption> selectedOptions = new();
	private Dictionary<int, string> notes = new();
	private Dictionary<int, int> quantities = new();
	public ObservableCollection<Database.Models.MenuItem> MenuItems { get; private set; }
	public int TableNumber { get; private set; }

	private OrderBuilder orderBuilder;
	public OrderCartViewModel OrderCart { get; private set; }

    public ICommand ClickCommand { get; private set; }
	public ICommand SelectVariationCommand { get; private set; }
	public ICommand SelectOptionCommand { get; private set; }
	public ICommand NoteChangedCommand { get; private set; }
	public ICommand QuantityChangedCommand { get; private set; }

	public MenuViewModel()
	{
		var settings = new Database.Settings();
		var connectionString = $"Server={settings.DbHost};Port={settings.DbPort};User={settings.DbUser};Password={settings.DbPassword};Database={settings.DbDatabase}";

		_context = new MenuContext(connectionString);
		menu = new Menu(_context);
        MenuItems = new ObservableCollection<Database.Models.MenuItem>(menu.Items);

		var appSettings = new Settings();
        TableNumber = appSettings.TableId;
        orderBuilder = new OrderBuilder(_context, new Table { Id = TableNumber });
		OrderCart = new OrderCartViewModel(orderBuilder);

        ClickCommand = new RelayCommand<Database.Models.MenuItem>(Click);
        SelectVariationCommand = new RelayCommand<object>(SelectVariation);
		SelectOptionCommand = new RelayCommand<object>(SelectOption);
		NoteChangedCommand = new RelayCommand<object>(NoteChanged);
		QuantityChangedCommand = new RelayCommand<object>(QuantityChanged);
	}

    private void SelectVariation(object? menuVariation)
    {
		if (menuVariation is MenuItemVariation menuItemVariation)
		{
			Console.WriteLine(
				menuItemVariation.MenuItemId + " " +
				menuItemVariation.Name
			);

			selectedVariations[menuItemVariation.MenuItemId] = menuItemVariation;
		}
    }

    private void SelectOption(object? menuOption)
    {
		if (menuOption is MenuItemOption menuItemOption)
		{
			Console.WriteLine(
				menuItemOption.MenuItemId + " " +
				menuItemOption.Name
			);

			selectedOptions[menuItemOption.MenuItemId] = menuItemOption;
		}
    }

	private void NoteChanged(object? note)
	{
		if (note is Entry entry && entry.BindingContext is Database.Models.MenuItem menuItem)
		{
			Console.WriteLine(menuItem.Name);
			Console.WriteLine(entry.Text);
			notes[menuItem.Id] = entry.Text;
		}
	}

	private void QuantityChanged(object? quantity)
	{
		if (quantity is Entry entry && entry.BindingContext is Database.Models.MenuItem menuItem && int.TryParse(entry.Text, out int quantityInt))
		{
			Console.WriteLine(menuItem.Name);
			Console.WriteLine(quantityInt);
			quantities[menuItem.Id] = quantityInt;
		}
	}

    private void Click(Database.Models.MenuItem? menuItem)
	{
		if (menuItem is not null)
		{
			Console.WriteLine(menuItem.Name);

			if (selectedVariations.ContainsKey(menuItem.Id))
			{
				Console.WriteLine(selectedVariations[menuItem.Id].Name);
			}

			if (selectedOptions.ContainsKey(menuItem.Id))
			{
				Console.WriteLine(selectedOptions[menuItem.Id].Name);
			}

			if (notes.ContainsKey(menuItem.Id))
			{
				Console.WriteLine(notes[menuItem.Id]);
			}

			if (quantities.ContainsKey(menuItem.Id))
			{
				Console.WriteLine(quantities[menuItem.Id]);
			}

			if (
				selectedVariations.ContainsKey(menuItem.Id) &&
				selectedOptions.ContainsKey(menuItem.Id) &&
				quantities.ContainsKey(menuItem.Id)
			)
			{
				orderBuilder.AddOrderItem(
					selectedVariations[menuItem.Id],
					selectedOptions[menuItem.Id],
					quantities[menuItem.Id],
					notes.ContainsKey(menuItem.Id) ? notes[menuItem.Id] : ""
				);
			}
		}
	}
}

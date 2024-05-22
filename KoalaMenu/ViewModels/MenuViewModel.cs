using Database.Data;
using KoalaMenu.Models;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using Database.Models;
using System.ComponentModel;

namespace KoalaMenu.ViewModels;

internal partial class MenuViewModel : ObservableObject, INotifyPropertyChanged
{
	private Menu menu;
	private ObservableCollection<MenuItemViewModel> _menuItems;
    public ObservableCollection<MenuItemViewModel> MenuItems {
        get => _menuItems;
		private set
		{
			_menuItems = value;
			OnPropertyChanged();
		}
	}

	private string _searchText = "";
	private string _optionFilter = "";
	public string SearchText 
	{ 
		get => _searchText; 
		set 
		{ 
			_searchText = value; 
			OnPropertyChanged();
			FilterMenuItems();
		} 
	}
	public string OptionFilter 
	{ 
		get => _optionFilter; 
		set 
		{ 
			_optionFilter = value; 
			OnPropertyChanged();
			FilterMenuItems();
		} 
	}
    public ObservableCollection<string> OptionNames { get; set; }

	public int TableNumber { get; private set; }
	private OrderBuilder orderBuilder;
	public OrderCartViewModel OrderCart { get; private set; }

	public MenuViewModel()
	{
		var context = new DataContext();
		menu = new Menu(context);

		var appSettings = new Settings();
        TableNumber = appSettings.TableId;
        orderBuilder = new OrderBuilder(context, new Table { Id = TableNumber });
		OrderCart = new OrderCartViewModel(orderBuilder);

        _menuItems = new ObservableCollection<MenuItemViewModel>();
		MenuItems = _menuItems;
		foreach (var menuItem in menu.MenuItems)
		{
			MenuItems.Add(new MenuItemViewModel(menuItem, orderBuilder));
		}

		OptionNames = new ObservableCollection<string>(menu.GetOptionNames());
	}

	private void FilterMenuItems()
	{
		var filteredMenuItems = menu.Filter(SearchText, OptionFilter);
        MenuItems = new ObservableCollection<MenuItemViewModel>(filteredMenuItems.Select(item => new MenuItemViewModel(item, orderBuilder)));
	}
}

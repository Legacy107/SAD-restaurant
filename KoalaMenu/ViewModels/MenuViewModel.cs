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
	private ObservableCollection<MenuItemGroup> _menuItems;
    public ObservableCollection<MenuItemGroup> MenuItems {
        get => _menuItems;
		private set
		{
			_menuItems = value;
			OnPropertyChanged();
		}
	}

	private string _searchText = "";
	private string _optionFilter = "";
	private string _categoryFilter = "";
	public string SearchText 
	{ 
		get => _searchText; 
		set 
		{ 
			_searchText = value; 
			OnPropertyChanged();
		} 
	}
	public string OptionFilter 
	{ 
		get => _optionFilter; 
		set 
		{ 
			_optionFilter = value; 
			OnPropertyChanged();
		} 
	}
	public string CategoryFilter 
	{ 
		get => _categoryFilter; 
		set 
		{
			if (value == "All")
				_categoryFilter = "";
            else
				_categoryFilter = value; 
			OnPropertyChanged();
		} 
	}
    public ObservableCollection<string> OptionNames { get; private set; }
	public ObservableCollection<string> CategoryNames { get; private set; }

	public int TableNumber { get; private set; }
	private OrderBuilder orderBuilder;
	public OrderCartViewModel OrderCart { get; private set; }

	public ICommand FilterMenuItemsCommand { get; private set; }

	public MenuViewModel()
	{
		var context = new DataContext();
		menu = new Menu(context);

		var appSettings = new Settings();
        TableNumber = appSettings.TableId;
        orderBuilder = new OrderBuilder(context, new Table { Id = TableNumber });
		OrderCart = new OrderCartViewModel(orderBuilder);

		OptionNames = new ObservableCollection<string>(menu.GetOptionNames());
		var Categories = menu.Categories.Select(category => category.Name).ToList();
		Categories.Insert(0, "All");
        CategoryNames = new ObservableCollection<string>(Categories);


        _menuItems = new ObservableCollection<MenuItemGroup>();
		MenuItems = _menuItems;
		// foreach (var menuItem in menu.MenuItems)
		// {
		// 	MenuItems.Add(new MenuItemViewModel(menuItem, orderBuilder));
		// }
		UpdateMenuItems(menu.MenuItems);

		FilterMenuItemsCommand = new RelayCommand(FilterMenuItems);
    }

	private void UpdateMenuItems(ObservableCollection<Database.Models.MenuItem> menuItems)
	{
		MenuItems.Clear();
		foreach (var category in menu.Categories)
		{
			var categoryMenuItems = new ObservableCollection<MenuItemViewModel>(
				menuItems
					.Where(item => item.Category.Name == category.Name)
					.Select(item => new MenuItemViewModel(item, orderBuilder))
					.ToList()
			);
			MenuItems.Add(new MenuItemGroup(category.Name, categoryMenuItems));
		}
	}

	private void FilterMenuItems()
	{
		var filteredMenuItems = menu.Filter(SearchText, OptionFilter, CategoryFilter);
		UpdateMenuItems(filteredMenuItems);
	}
}

public class MenuItemGroup : ObservableCollection<MenuItemViewModel>
{
	public string Name { get; private set; }

	public MenuItemGroup(string category, ObservableCollection<MenuItemViewModel> menuItems) : base(menuItems)
	{
		Name = category;
	}
}

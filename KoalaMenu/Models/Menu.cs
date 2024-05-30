using System.Collections.ObjectModel;
using Database.Data;
using KoalaMenu.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace KoalaMenu.Models;

public class Menu
{
    public ObservableCollection<Database.Models.MenuItem> MenuItems { get; private set; }
    public ObservableCollection<Database.Models.MenuItemCategory> Categories { get; private set; }

    public Menu(DataContext context)
    {
        MenuItems = new ObservableCollection<Database.Models.MenuItem>(
            context.MenuItem
                .Include(menuItem => menuItem.Options)
                .Include(menuItem => menuItem.Variations)
                .Include(menuItem => menuItem.Category)
                .OrderBy(menuItem => menuItem.Category.Name)
                .ThenBy(menuItem => menuItem.Name)
                .ToList()
        );
        Categories = new ObservableCollection<Database.Models.MenuItemCategory>(
            context.MenuItemCategory.ToList()
        );
    }

    public ObservableCollection<Database.Models.MenuItem> Filter(string searchText, string optionText, string categoryText)
    {
        if (string.IsNullOrEmpty(searchText) && string.IsNullOrEmpty(optionText) && string.IsNullOrEmpty(categoryText))
        {
            return MenuItems;
        }
        else
        {
            return new ObservableCollection<Database.Models.MenuItem>(
                MenuItems
                    .Where(item =>
                        (string.IsNullOrEmpty(searchText) || item.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrEmpty(optionText) || item.Options.Any(option => option.Name == optionText)) &&
                        (string.IsNullOrEmpty(categoryText) || item.Category.Name == categoryText))
                    .ToList()
            );
        }
    }

    public List<string> GetOptionNames()
    {
        return MenuItems
            .SelectMany(x => x.Options)
            .Select(x => x.Name)
            .Distinct()
            .ToList();
    }
}

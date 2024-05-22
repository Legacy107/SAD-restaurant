using System.Collections.ObjectModel;
using Database.Data;
using KoalaMenu.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KoalaMenu.Models;

public class Menu
{
    private DataContext _context;
    public ObservableCollection<Database.Models.MenuItem> MenuItems { get; private set; }

    public int TableNumber { get; private set; }

    public Menu(DataContext context)
    {
        _context = context;
        MenuItems = new ObservableCollection<Database.Models.MenuItem>(
            _context.MenuItem.Include(menuItem => menuItem.Options).Include(menuItem => menuItem.Variations).ToList()
        );
    }

    public ObservableCollection<Database.Models.MenuItem> Filter(string searchText, string optionText)
    {
        if (string.IsNullOrEmpty(searchText) && string.IsNullOrEmpty(optionText))
        {
            return MenuItems;
        }
        else
        {
            return new ObservableCollection<Database.Models.MenuItem>(
                MenuItems
                    .Where(item =>
                        (string.IsNullOrEmpty(searchText) || item.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)) &&
                        (string.IsNullOrEmpty(optionText) || item.Options.Any(option => option.Name == optionText)))
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

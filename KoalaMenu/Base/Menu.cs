using Database.Data;
using Microsoft.EntityFrameworkCore;

namespace KoalaMenu.Base;

public class Menu
{
    public List<Database.Models.MenuItem> Items { get; private set; } = new List<Database.Models.MenuItem>();
    private MenuContext _context;

    public Menu(MenuContext context)
    {
        _context = context;
        Items = context.MenuItem.Include(menuItem => menuItem.Options).Include(menuItem => menuItem.Variations).ToList();
    }

    public List<Database.Models.MenuItem> Search(string query)
    {
        return Items.Where(x => x.Name.Contains(query)).ToList();
    }

    public List<Database.Models.MenuItem> Filter(string option, float maxPrice)
    {
        return Items
            .Where(
                x => x.Options.Any(y => y.Name == option) ||
                x.Variations.Any(y => y.Price <= maxPrice)
            )
            .ToList();
    } 
}

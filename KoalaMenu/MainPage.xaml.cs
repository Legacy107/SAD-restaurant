using Microsoft.Extensions.Configuration;
using Database;
using Database.Data;

namespace KoalaMenu;

public partial class MainPage : ContentPage
{
	int count = 0;
	DataContext _context;

	public MainPage()
	{
		var settings = new Settings();
		var connectionString = $"Server={settings.DbHost};Port={settings.DbPort};User={settings.DbUser};Password={settings.DbPassword};Database={settings.DbDatabase}";
		Console.WriteLine(connectionString);

		_context = new DataContext(connectionString);
		InitializeComponent();
	}


    private void OnCounterClicked(object sender, EventArgs e)
	{
		var items = _context.MenuItem.ToList();
		Console.WriteLine(items[0].Name);

		var chip = _context.MenuItem.Where(x => x.Name == "Chips").FirstOrDefault();
		if (chip is not null)
			Console.WriteLine(chip.Description);

		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

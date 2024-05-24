using Database.Data;

namespace KoalaMenu;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

	protected override async void OnStart()
	{
		base.OnStart();
		using (var context = new DataContext())
		{
			await Seed.SeedMenuItemCategory(context);
		}
	}
}


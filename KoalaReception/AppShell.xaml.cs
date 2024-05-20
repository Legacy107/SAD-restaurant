namespace KoalaReception;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(Views.ReservationPage), typeof(Views.ReservationPage));
        Routing.RegisterRoute(nameof(Views.CheckInPage), typeof(Views.CheckInPage));
    }
}


using KoalaReception.ViewModels;

namespace KoalaReception.Views;

public partial class ReservationPage : ContentPage
{
	public ReservationPage(ReservationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}
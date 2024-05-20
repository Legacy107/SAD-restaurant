using KoalaReception.ViewModels;

namespace KoalaReception.Views;

public partial class CheckInPage : ContentPage
{
	public CheckInPage(CheckInViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
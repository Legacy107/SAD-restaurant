using KoalaPayment.ViewModels;
using Windows.UI.WebUI;

namespace KoalaPayment.Views;

public partial class CheckoutViews : ContentPage
{
	public CheckoutViews()
	{
		InitializeComponent();
		var CheckoutviewModel = new CheckoutViewModels();
        BindingContext = CheckoutviewModel;
    }
}
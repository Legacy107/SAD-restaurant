using KoalaPayment.ViewModels;

namespace KoalaPayment.Views;

public partial class PaymentMethodViews : ContentPage
{
	public PaymentMethodViews()
	{
		InitializeComponent();
        var PaymentMethodViewModel = new PaymentMethodViewModels();
        BindingContext = PaymentMethodViewModel;
    }
}
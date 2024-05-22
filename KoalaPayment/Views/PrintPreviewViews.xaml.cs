using KoalaPayment.ViewModels;

namespace KoalaPayment.Views;

public partial class PrintPreviewViews : ContentPage
{
	public PrintPreviewViews()
	{
		InitializeComponent();
        var printPreview = new PrintPreviewViewModels();
        BindingContext = printPreview;
    }
}
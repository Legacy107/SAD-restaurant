using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using KoalaReception.ViewModels;

namespace KoalaReception.Views;

public partial class MessagePopup: Popup
{
    public MessagePopup(string message)
    {
        InitializeComponent();
        MessageLabel.Text = message;
    }

    private void OnReturnToHomeClicked(object sender, EventArgs e)
    {
        Close();
    }

    protected override Task OnClosed(object? result, bool wasDismissedByTappingOutsideOfPopup, CancellationToken token = default)
    {
        base.OnClosed(result, wasDismissedByTappingOutsideOfPopup, token);
        return NavigateToHomePage();
    }


    private async Task NavigateToHomePage()
    {
        if (Application.Current?.Dispatcher != null)
        {
            await Application.Current.Dispatcher.DispatchAsync(async () =>
            {
                if (Application.Current.MainPage != null)
                {
                    await Shell.Current.GoToAsync("..");
                }
            });
        }
    }
}
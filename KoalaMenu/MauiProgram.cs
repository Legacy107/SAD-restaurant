using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace KoalaMenu;

public static class MauiProgram
{
	public static MauiApp CreateMauiAppAsync()
	{
		var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}


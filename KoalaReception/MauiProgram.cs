using Database;
using Database.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KoalaReception.ViewModels;
using KoalaReception.Models;
using KoalaReception.Views;
using CommunityToolkit.Maui;


namespace KoalaReception;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        var settings = new Settings();
        var mySqlConnectionStr = $"Server={settings.DbHost};Port={settings.DbPort};User={settings.DbUser};Password={settings.DbPassword};Database={settings.DbDatabase}";

        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddDbContext<DataContext>(options =>
        {
            var settings = new Settings();
            var mySqlConnectionStr = $"Server={settings.DbHost};Port={settings.DbPort};User={settings.DbUser};Password={settings.DbPassword};Database={settings.DbDatabase}";

            options.UseMySql(
                mySqlConnectionStr,
                ServerVersion.AutoDetect(mySqlConnectionStr),
                options => options.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
                )
            );
        });

        builder.Services.AddTransient<TableWrapper>();
        builder.Services.AddSingleton<ReservationHandler>();
        builder.Services.AddSingleton<Reservation>();
        builder.Services.AddSingleton<CheckInHandler>();
        builder.Services.AddSingleton<CheckIn>();

        builder.Services.AddTransient<ReservationViewModel>();
        builder.Services.AddTransient<ReservationPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<CheckInViewModel>();
        builder.Services.AddTransient<CheckInPage>();

        builder.Services.AddTransientPopup<MessagePopup, MessagePopupViewModel>();


#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}


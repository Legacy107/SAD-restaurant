using Database.Data;
using KoalaReception.Views;
using Microsoft.EntityFrameworkCore;

namespace KoalaReception;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;
    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _serviceProvider = serviceProvider;

        MainPage = new AppShell();
    }

    protected override async void OnStart()
    {
        base.OnStart();
        await PopulateData();
    }

    private async Task PopulateData()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            using var ctx = scope.ServiceProvider.GetRequiredService<DataContext>();
            await ctx.Database.MigrateAsync();
            await Seed.SeedData(ctx);
        }
    }
}


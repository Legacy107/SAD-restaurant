using System;
using CommunityToolkit.Mvvm.Input;
using Database;
using Database.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using OrderCommon;
using KoalaWaiter.Base;

namespace KoalaWaiter.ViewModels;

public partial class FailedItemsViewModel : ObservableObject
{

    private MenuContext context;
    private OrderItemQueue failedItemQueue;
    public ObservableCollection<OrderItemCommand> OrderItemQueue => failedItemQueue.Queue;

    public ICommand DismissOrderItemCommand { get; private set; }

    public FailedItemsViewModel()
    {
        var settings = new Settings();
        var connectionString = $"Server={settings.DbHost};Port={settings.DbPort};User={settings.DbUser};Password={settings.DbPassword};Database={settings.DbDatabase}";
        Console.WriteLine(connectionString);

        context = new MenuContext(connectionString);
        failedItemQueue = new FailedItemQueue(context);

        DismissOrderItemCommand = new RelayCommand<OrderItemCommand>(DismissOrderItem);

        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(5);
        timer.Tick += (sender, e) => failedItemQueue.FetchOrderItems();
        timer.Start();
    }

    public void DismissOrderItem(OrderItemCommand? orderItemCommand)
    {
        if (orderItemCommand is not null)
            failedItemQueue.Remove(orderItemCommand);
    }
}


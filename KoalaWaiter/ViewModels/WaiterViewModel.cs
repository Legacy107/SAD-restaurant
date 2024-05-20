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

internal partial class WaiterViewModel : ObservableObject
{

    private MenuContext context;
    private OrderItemQueue servingItemQueue;
    public ObservableCollection<OrderItemCommand> OrderItemQueue => servingItemQueue.Queue;

    public FailedItemsViewModel FailedItemsViewModel { get; private set; }

    public ICommand CompleteOrderItemCommand { get; private set; }
    public ICommand CancelOrderItemCommand { get; private set; }

    public WaiterViewModel()
    {
        var settings = new Settings();
        var connectionString = $"Server={settings.DbHost};Port={settings.DbPort};User={settings.DbUser};Password={settings.DbPassword};Database={settings.DbDatabase}";
        Console.WriteLine(connectionString);

        context = new MenuContext(connectionString);
        servingItemQueue = new ServingItemQueue(context);

        CompleteOrderItemCommand = new RelayCommand<OrderItemCommand>(CompleteOrderItem);
        CancelOrderItemCommand = new RelayCommand<OrderItemCommand>(CancelOrderItem);

        FailedItemsViewModel = new FailedItemsViewModel();

        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(5);
        timer.Tick += (sender, e) => servingItemQueue.FetchOrderItems();
        timer.Start();
    }

    public void CompleteOrderItem(OrderItemCommand? orderItemCommand)
    {
        if (orderItemCommand is not null)
            servingItemQueue.Execute(orderItemCommand);
    }

    public void CancelOrderItem(OrderItemCommand? orderItemCommand)
    {
        if (orderItemCommand is not null)
            servingItemQueue.Remove(orderItemCommand);
    }
}


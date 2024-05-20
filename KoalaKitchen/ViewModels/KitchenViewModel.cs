using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Database;
using Database.Data;
using KoalaKitchen.Base;

namespace KoalaKitchen.ViewModels;

internal partial class KitchenViewModel : ObservableObject
{
    private MenuContext context;
    private OrderItemQueue kitchenItemQueue;
    public ObservableCollection<OrderItemCommand> OrderItemQueue => kitchenItemQueue.Queue;

    public ICommand CompleteOrderItemCommand { get; private set; }
    public ICommand CancelOrderItemCommand { get; private set; }

    public KitchenViewModel()
    {
        var settings = new Settings();
        var connectionString = $"Server={settings.DbHost};Port={settings.DbPort};User={settings.DbUser};Password={settings.DbPassword};Database={settings.DbDatabase}";
        Console.WriteLine(connectionString);

        context = new MenuContext(connectionString);
        kitchenItemQueue = new KitchenItemQueue(context);

        CompleteOrderItemCommand = new RelayCommand<OrderItemCommand>(CompleteOrderItem);
        CancelOrderItemCommand = new RelayCommand<OrderItemCommand>(CancelOrderItem);
    }

    public void CompleteOrderItem(OrderItemCommand? orderItemCommand)
    {
        if (orderItemCommand is not null)
            kitchenItemQueue.Execute(orderItemCommand);
    }

    public void CancelOrderItem(OrderItemCommand? orderItemCommand)
    {
        if (orderItemCommand is not null)
            kitchenItemQueue.Remove(orderItemCommand);
    }
}


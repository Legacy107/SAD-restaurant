using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Database;
using Database.Data;
using KoalaKitchen.Base;
using OrderCommon;

namespace KoalaKitchen.ViewModels;

internal partial class KitchenViewModel : ObservableObject
{
    private OrderItemQueue kitchenItemQueue;
    public ObservableCollection<OrderItemCommand> OrderItemQueue => kitchenItemQueue.Queue;

    public ICommand CompleteOrderItemCommand { get; private set; }
    public ICommand CancelOrderItemCommand { get; private set; }

    public KitchenViewModel()
    {
        var context = new DataContext();
        kitchenItemQueue = new KitchenItemQueue(context);

        CompleteOrderItemCommand = new RelayCommand<OrderItemCommand>(CompleteOrderItem);
        CancelOrderItemCommand = new RelayCommand<OrderItemCommand>(CancelOrderItem);

        var timer = Application.Current.Dispatcher.CreateTimer();
        timer.Interval = TimeSpan.FromSeconds(5);
        timer.Tick += (sender, e) => kitchenItemQueue.FetchOrderItems();
        timer.Start();
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


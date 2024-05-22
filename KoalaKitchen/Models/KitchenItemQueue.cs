using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using OrderCommon;

namespace KoalaKitchen.Base;

public class KitchenItemQueue : OrderItemQueue
{
    public KitchenItemQueue(DataContext context) : base(context)
    {
    }

    public override void FetchOrderItems()
    {
        var orderItems = Context.OrderItem
            .Include(orderItem => orderItem.Order)
            .Include(orderItem => orderItem.MenuItemOption)
            .Include(orderItem => orderItem.MenuItemVariation)
            .Include(orderItem => orderItem.MenuItemVariation.MenuItem)
            .Where(orderItem => orderItem.Status == OrderItemStatus.Pending)
            .OrderBy(orderItem => orderItem.Order.Created)
            .OrderBy(orderItem => orderItem.OrderId)
            .ToList();
        foreach (var orderItem in orderItems)
        {
            Add(new DineinCommand(Context, orderItem));
        }
    }
}
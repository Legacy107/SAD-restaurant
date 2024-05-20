using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using OrderCommon;

namespace KoalaWaiter.Base;

public class ServingItemQueue : OrderItemQueue
{
    public ServingItemQueue(MenuContext context) : base(context)
    {
    }

    public override void FetchOrderItems()
    {
        var orderItems = Context.OrderItem
            .Include(orderItem => orderItem.Order)
            .Include(orderItem => orderItem.MenuItemOption)
            .Include(orderItem => orderItem.MenuItemVariation)
            .Include(orderItem => orderItem.MenuItemVariation.MenuItem)
            .Where(orderItem => orderItem.Status == OrderItemStatus.Ready)
            .OrderBy(orderItem => orderItem.Order.Created)
            .OrderBy(orderItem => orderItem.OrderId)
            .ToList();
        foreach (var orderItem in orderItems)
        {
            Add(new ServingCommand(Context, orderItem));
        }
    }
}
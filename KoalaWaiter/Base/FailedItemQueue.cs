using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using OrderCommon;

namespace KoalaWaiter.Base;

public class FailedItemQueue : OrderItemQueue
{
    public FailedItemQueue(MenuContext context) : base(context)
    {
        // query all order items in read status and add them to the queue
        var orderItems = Context.OrderItem
            .Include(orderItem => orderItem.Order)
            .Include(orderItem => orderItem.MenuItemOption)
            .Include(orderItem => orderItem.MenuItemVariation)
            .Include(orderItem => orderItem.MenuItemVariation.MenuItem)
            .Where(orderItem => orderItem.Status == OrderItemStatus.Cancelled)
            .OrderBy(orderItem => orderItem.Order.Created)
            .OrderBy(orderItem => orderItem.OrderId)
            .ToList();
        foreach (var orderItem in orderItems)
        {
            Add(new FailedItemCommand(context, orderItem));
        }
    }
}
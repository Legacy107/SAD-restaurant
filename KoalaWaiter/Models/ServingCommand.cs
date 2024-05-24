using Database.Data;
using Database.Models;
using OrderCommon;

namespace KoalaWaiter.Base;

public class ServingCommand : OrderItemCommand
{
    public ServingCommand(DataContext context, OrderItem orderItem) : base(context, orderItem)
    {
    }

    public override void Execute()
    {
        OrderItem.Status = OrderItemStatus.Completed;
        Context.OrderItem.Update(OrderItem);
        Context.SaveChanges();
    }

    public override void Cancel()
    {
        OrderItem.Status = OrderItemStatus.Archived;
        Context.OrderItem.Update(OrderItem);
        Context.SaveChanges();
    }
}


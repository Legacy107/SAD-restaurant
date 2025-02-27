using Database.Data;
using Database.Models;
using OrderCommon;

namespace KoalaKitchen.Base;

public class DineinCommand : OrderItemCommand
{
    public DineinCommand(DataContext context, OrderItem orderItem) : base(context, orderItem)
    {
    }

    public override void Execute()
    {
        OrderItem.Status = OrderItemStatus.Ready;
        Context.OrderItem.Update(OrderItem);
        Context.SaveChanges();
    }

    public override void Cancel()
    {
        OrderItem.Status = OrderItemStatus.Cancelled;
        Context.OrderItem.Update(OrderItem);
        Context.SaveChanges();
    }
}


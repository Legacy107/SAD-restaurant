using Database.Data;
using Database.Models;
using OrderCommon;

namespace KoalaWaiter.Base;

public class FailedItemCommand : OrderItemCommand
{
    public FailedItemCommand(MenuContext context, OrderItem orderItem) : base(context, orderItem)
    {
    }

    public override void Execute()
    {
        OrderItem.Status = OrderItemStatus.Archived;
        Context.OrderItem.Update(OrderItem);
        Context.SaveChanges();
    }

    public override void Cancel()
    {
        Execute();
    }
}


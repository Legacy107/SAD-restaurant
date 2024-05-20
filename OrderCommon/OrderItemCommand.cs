using Database.Data;
using Database.Models;

namespace OrderCommon;

public abstract class OrderItemCommand
{
    protected MenuContext Context;
	public OrderItem OrderItem { get; private set; }

    public OrderItemCommand(MenuContext context, OrderItem orderItem)
    {
        OrderItem = orderItem;
        Context = context;
    }

    public abstract void Execute();

    public abstract void Cancel();
}


using Database.Data;
using Database.Models;

namespace OrderCommon;

public abstract class OrderItemCommand
{
    protected DataContext Context;
	public OrderItem OrderItem { get; private set; }

    public OrderItemCommand(DataContext context, OrderItem orderItem)
    {
        OrderItem = orderItem;
        Context = context;
    }

    public abstract void Execute();

    public abstract void Cancel();
}


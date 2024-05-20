using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Database.Data;

namespace OrderCommon;

public abstract class OrderItemQueue : ObservableObject
{
	protected MenuContext Context { get; private set; }
	public ObservableCollection<OrderItemCommand> Queue { get; private set; }

	public OrderItemQueue(MenuContext context)
	{
		Context = context;
		Queue = new ObservableCollection<OrderItemCommand>();
		FetchOrderItems();
	}

	public abstract void FetchOrderItems();

	public bool Contains(OrderItemCommand command)
	{
		foreach (var item in Queue)
		{
			if (item.OrderItem.Id == command.OrderItem.Id)
			{
				return true;
			}
		}
		return false;
	}

	public void Add(OrderItemCommand command)
	{
		if (!Contains(command))
			Queue.Add(command);
	}

	public void Remove(OrderItemCommand command)
	{
		command.Cancel();
		Queue.Remove(command);
	}

	public void Execute(OrderItemCommand command)
	{
		command.Execute();
		Queue.Remove(command);
	}
}


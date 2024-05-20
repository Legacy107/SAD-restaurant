using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Database.Data;

namespace OrderCommon;

public class OrderItemQueue : ObservableObject
{
	protected MenuContext Context { get; private set; }
	public ObservableCollection<OrderItemCommand> Queue { get; private set; }

	public OrderItemQueue(MenuContext context)
	{
		Context = context;
		Queue = new ObservableCollection<OrderItemCommand>();
	}

	public void Add(OrderItemCommand command)
	{
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


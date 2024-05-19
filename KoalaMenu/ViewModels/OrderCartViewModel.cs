using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KoalaMenu.Base;

namespace KoalaMenu.ViewModels;

public partial class OrderCartViewModel : ObservableObject
{
	private OrderBuilder orderBuilder;
	public ObservableCollection<OrderRequest> OrderRequests => orderBuilder.OrderRequests;

	public ICommand RemoveItemCommand { get; private set; }
	public ICommand SubmitOrderCommand { get; private set; }

	public OrderCartViewModel(OrderBuilder _orderBuilder)
	{
		orderBuilder = _orderBuilder;
		RemoveItemCommand = new RelayCommand<OrderRequest>(RemoveItem);
        SubmitOrderCommand = new RelayCommand(SubmitOrder);
	}

	private void RemoveItem(OrderRequest? orderRequest)
	{
		if (orderRequest is not null)
			orderBuilder.OrderRequests.Remove(orderRequest);
	}

	public void SubmitOrder()
	{
		orderBuilder.SubmitOrder();
	}
}


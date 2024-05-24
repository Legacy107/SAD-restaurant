using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KoalaMenu.Models;

namespace KoalaMenu.ViewModels;

public partial class OrderCartViewModel : ObservableObject
{
	public ICommand RemoveItemCommand { get; private set; }
	public ICommand SubmitOrderCommand { get; private set; }
	private OrderBuilder orderBuilder;
	public ObservableCollection<OrderRequest> OrderRequests => orderBuilder.OrderRequests;
	private float totalPrice;
	public float TotalPrice
	{
		get => totalPrice;
		set => SetProperty(ref totalPrice, value);
	}
	
	private void OrderBuilder_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(orderBuilder.TotalPrice))
		{
			TotalPrice = orderBuilder.TotalPrice;
		}
	}

	public OrderCartViewModel(OrderBuilder _orderBuilder)
	{
		orderBuilder = _orderBuilder;
		TotalPrice = orderBuilder.TotalPrice;
		orderBuilder.PropertyChanged += OrderBuilder_PropertyChanged;
		RemoveItemCommand = new RelayCommand<OrderRequest>(RemoveItem);
        SubmitOrderCommand = new RelayCommand(SubmitOrder);
	}

	private void RemoveItem(OrderRequest? orderRequest)
	{
		if (orderRequest is not null)
			orderBuilder.RemoveOrderItem(orderRequest);
	}

	public void SubmitOrder()
	{
		orderBuilder.SubmitOrder();
	}
}


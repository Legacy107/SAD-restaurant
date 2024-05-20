using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Database.Data;
using Database.Models;

namespace KoalaMenu.Base
{
    public class OrderBuilder : ObservableObject
    {
        public ObservableCollection<OrderRequest> OrderRequests { get; set; }

        private Table table;
        MenuContext _context;

        public OrderBuilder(MenuContext context, Table _table)
        {
            OrderRequests = new ObservableCollection<OrderRequest>();
            table = _table;
            _context = context;
        }

        public void AddOrderItem(MenuItemVariation variation, MenuItemOption option, int quantity, string note)
        {
            OrderRequests.Add(new OrderRequest(variation, option, quantity, note));
        }

        public void SubmitOrder()
        {
            // Save orderItems to the database
            Order order = new Order{ TableId = table.Id };
            foreach (OrderRequest request in OrderRequests)
            {
                for (int i = 0; i < request.Quantity; i++)
                {
                    OrderItem orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        MenuItemOptionId = request.Option.Id,
                        MenuItemVariationId = request.Variation.Id,
                        Note = request.Note
                    };
                    order.Items.Add(orderItem);
                }
            }

            // Clear the orderItems list
            OrderRequests.Clear();

            // Save the order to the database
            _context.Order.Add(order);
            _context.SaveChanges();
        }

        public void CancelOrder()
        {
            OrderRequests.Clear();
        }
    }

    public class OrderRequest
    {
        public MenuItemVariation Variation { get; set; }
        public MenuItemOption Option { get; set; }
        public int Quantity { get; set; }
        public string Note { get; set; }

        public OrderRequest(MenuItemVariation _variation, MenuItemOption _option, int _quantity, string _note)
        {
            Variation = _variation;
            Option = _option;
            Quantity = _quantity;
            Note = _note;
        }
    }
}
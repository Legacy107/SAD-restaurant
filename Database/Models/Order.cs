using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public enum OrderStatus
    {
        Pending,
        Ready,
        Completed,
        Cancelled,
    }

    public class Order
    {
        public int Id { get; set; }
        public required int TableId { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public DateTime Created { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
    }
}


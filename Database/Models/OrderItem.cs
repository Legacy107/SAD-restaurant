using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public enum OrderItemStatus
    {
        Pending,
        Ready,
        Completed,
        Cancelled,
        Archived,
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public required int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public required int MenuItemOptionId { get; set; }
        public MenuItemOption MenuItemOption { get; set; } = null!;
        public required int MenuItemVariationId { get; set; }
        public MenuItemVariation MenuItemVariation { get; set; } = null!;
        public string Note { get; set; } = "";
        public OrderItemStatus Status { get; set; } = OrderItemStatus.Pending;
    }
}
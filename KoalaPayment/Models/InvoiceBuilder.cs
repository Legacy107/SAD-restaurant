using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace KoalaPayment.Models
{
    internal class InvoiceBuilder
    {
        internal Invoice? BuildInvoice(DataContext context, string tableNumber, ref string errorStatus)
        {
            var table = context.Tables
                .SingleOrDefault(t => t.Id == int.Parse(tableNumber));

            if (table == null)
            {
                errorStatus += $"Table not found with number {tableNumber}";
                return null;
            }

            var order = context.Order
                .Include(o => o.Items)
                .SingleOrDefault(o => o.TableId == table.Id);

            if (order == null)
            {
                errorStatus += $"Order not found for table {tableNumber}";
                return null;
            }

            var invoice = context.Invoice.SingleOrDefault(iv => iv.OrderId == order.Id);

            if (invoice != null)
            {
                return invoice;
            }

            invoice = new Invoice
            {
                OrderId = order.Id,
                InvoiceTime = DateTime.Now
            };
            context.Invoice.Add(invoice);
            context.SaveChanges();

            var orderItems = order.Items;
            foreach (var orderItem in orderItems)
            {
                var MenuItemVariation = context.MenuItemVariation
                    .SingleOrDefault(iv => iv.Id == orderItem.MenuItemVariationId);
                if (MenuItemVariation == null)
                {
                    errorStatus += $"Menu item not found with id {orderItem.Id}";
                    return null;
                }

                var MenuItem = context.MenuItem
                    .SingleOrDefault(mi => mi.Id == MenuItemVariation.MenuItemId);
                if (MenuItem == null)
                {
                    errorStatus += $"Menu item not found with id {MenuItemVariation.MenuItemId}";
                    return null;
                }
                
                var invoiceItem = new InvoiceItem
                {
                    InvoiceId = invoice.Id,
                    ItemName = MenuItem.Name,
                    Price = MenuItemVariation.Price
                };
                context.InvoiceItem.Add(invoiceItem);
            }

            context.SaveChanges();
            return invoice;
        }
    }
}

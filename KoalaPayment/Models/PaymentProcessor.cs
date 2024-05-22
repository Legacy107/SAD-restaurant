

using Database.Data;
using Database.Models;

namespace KoalaPayment.Models
{
    internal class PaymentProcessor
    {
        public static bool ProcessPayment(DataContext context, Payment payment, Invoice invoice)
        {
            var order = context.Orders
                .FirstOrDefault(o => o.Id == invoice.OrderId);
            if (order == null)
            {
                return false;
            }
            order.Status = OrderStatus.Completed;
            context.Orders.Update(order);

            double totalAmount = 0;
            
            var invoiceItems = context.InvoiceItem
                .Where(ii => ii.InvoiceId == invoice.Id)
                .ToList();
            foreach (var invoiceItem in invoiceItems)
            {
                totalAmount += invoiceItem.Price;
            }

            if (!payment.Process(totalAmount))
            {
                return false;
            }

            var receipt = new Receipt
            {
                InvoiceId = invoice.Id,
                PaymentType = payment.PaymentType,
                CustomerPay = totalAmount,
                PaymentTime = DateTime.Now,
                Change = 0
            };

            context.Receipt.Add(receipt);
            context.SaveChanges();
            return true;
        }
    }
}

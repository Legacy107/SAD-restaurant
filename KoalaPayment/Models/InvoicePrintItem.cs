using CommunityToolkit.Mvvm.ComponentModel;
using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaPayment.Models
{
    internal partial class InvoicePrintItem(Invoice invoice) : IPrintableItem
    {
        private readonly Invoice invoice = invoice;
        public string GetPrintContent(DataContext context)
        {
            var table = context.Tables
                .FromSql($@"
                    SELECT Tables.* FROM Tables
                    INNER JOIN `Order` ON Tables.Id = `Order`.TableId
                    INNER JOIN Invoice ON `Order`.Id = Invoice.OrderId
                    WHERE Invoice.id = {invoice.Id}")
                .SingleOrDefault()?? throw new Exception("Cannot find table");

            var printContent = $"Invoice of table {table.Id}: \n";
            
            var invoiceItems = context.InvoiceItem
                .FromSql($@"
                    SELECT InvoiceItem.* FROM InvoiceItem
                    INNER JOIN Invoice ON InvoiceItem.InvoiceId = Invoice.Id
                    WHERE Invoice.Id = {invoice.Id}")
                .ToList() ?? throw new Exception("Cannot find invoice items");

            double total = 0;
            foreach (var invoiceItem in invoiceItems)
            {
                printContent += $"{invoiceItem.ItemName} - {invoiceItem.Price} \n";
                total += invoiceItem.Price;
            }
            printContent += $"Total: {total}\n";
            printContent += $"Time: {invoice.InvoiceTime} \n";

            return printContent;
        }
    }
}

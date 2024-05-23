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
    class ReceiptPrintItem(Receipt receipt) : IPrintableItem
    {
        private readonly Receipt receipt = receipt;

        public string GetPrintContent(DataContext context)
        {
            var table = context.Tables
                .FromSql($@"
                    SELECT Tables.* FROM Tables
                    INNER JOIN Orders ON Tables.Id = Orders.TableId
                    INNER JOIN Invoice ON Orders.Id = Invoice.OrderId
                    INNER JOIN Receipt ON Invoice.Id = Receipt.InvoiceId 
                    WHERE Receipt.id = {receipt.Id}")
                .SingleOrDefault() ?? throw new Exception("Cannot find table");

            var printContent = $"Receipt of table {table.Id}: \n";

            var invoiceItems = context.InvoiceItem
                .FromSql($@"
                    SELECT InvoiceItem.* FROM InvoiceItem
                    INNER JOIN Invoice ON InvoiceItem.InvoiceId = Invoice.Id
                    WHERE Invoice.Id = {receipt.InvoiceId}")
                .ToList() ?? throw new Exception("Cannot find invoice items");

            double total = 0;
            foreach (var invoiceItem in invoiceItems)
            {
                printContent += $"{invoiceItem.ItemName} - {invoiceItem.Price} \n";
                total += invoiceItem.Price;
            }
            printContent += $"Total: {total}\n";
            printContent += $"Payment type: {receipt.PaymentType} \n";
            printContent += $"Amount: {receipt.CustomerPay} \n";
            printContent += $"Change: {receipt.Change} \n";
            printContent += $"Payment time: {receipt.PaymentTime} \n";

            return printContent;
        }
    }
}

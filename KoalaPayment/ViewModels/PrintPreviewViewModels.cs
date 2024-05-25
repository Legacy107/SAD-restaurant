using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using KoalaPayment.Models;
using Database;
using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace KoalaPayment.ViewModels
{
    internal partial class PrintPreviewViewModels : ObservableObject, IPrinter
    {
        [ObservableProperty]
        private string? tableNumber;

        [ObservableProperty]
        private string? printContent;

        private DataContext _context;

        public PrintPreviewViewModels()
        {
            _context = new DataContext();
        }
        [RelayCommand]
        public void PrintInvoice()
        {
            if (string.IsNullOrEmpty(TableNumber))
            {
                PrintContent = "Table number is required";
                return;
            }

            var invoice = _context.Invoice
                .FromSql($@"
                    SELECT Invoice.* 
                    FROM Invoice
                    INNER JOIN `Order` ON Invoice.OrderId = `Order`.Id
                    INNER JOIN Tables ON `Order`.TableId = Tables.Id
                    WHERE Tables.Id = {TableNumber}")
                .SingleOrDefault();

            if (invoice == null)
            {
                PrintContent = "Invoice not found";
                return;
            }
            Print(new InvoicePrintItem(invoice));
        }

        [RelayCommand]
        public void PrintReceipt()
        {
            if (string.IsNullOrEmpty(TableNumber))
            {
                PrintContent = "Table number is required";
                return;
            }

            var receipt = _context.Receipt
                .FromSql($@"
                    SELECT Receipt.* 
                    FROM Receipt
                    INNER JOIN Invoice ON Receipt.InvoiceId = Invoice.Id
                    INNER JOIN Orders ON Invoice.OrderId = Orders.Id
                    INNER JOIN Tables ON Orders.TableId = Tables.Id
                    WHERE Tables.Id = {TableNumber}")
                .SingleOrDefault();

            if (receipt == null)
            {
                PrintContent = "Receipt not found";
                return;
            }

            Print(new ReceiptPrintItem(receipt));
        }

        public void Print(IPrintableItem printableItem)
        {
            PrintContent = "Printing: \n" + printableItem.GetPrintContent(_context);
        }
    }
}

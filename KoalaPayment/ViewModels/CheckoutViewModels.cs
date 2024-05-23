using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Database.Data;
using KoalaPayment.Models;
using KoalaPayment.Views;

namespace KoalaPayment.ViewModels
{
    internal partial class CheckoutViewModels: ObservableObject
    {
        [ObservableProperty]
        private string? tableNumber;

        [ObservableProperty]
        private string errorStatus = "";

        private DataContext _context;

        public CheckoutViewModels()
        {
            _context = new DataContext();
        }

        [RelayCommand]
        private void CreateInvoice()
        {
            if (!string.IsNullOrEmpty(TableNumber))
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var invoiceBuilder = new InvoiceBuilder();
                        var errorString = "";
                        invoiceBuilder.BuildInvoice(_context, TableNumber, ref errorString);
                        ErrorStatus = errorString;
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                    }

                }
            }
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Database.Data;
using KoalaPayment.Models;

namespace KoalaPayment.ViewModels
{
    enum PaymentTypeEnum
    {
        Cash,
        Card
    }
    enum PaymentStatusEnum
    {
        Success,
        Failed
    }
    internal partial class PaymentMethodViewModels: ObservableObject, IPaymentReader
    {
        [ObservableProperty]
        private string? tableNumber;

        [ObservableProperty]
        private List<PaymentTypeEnum> paymentTypes;

        [ObservableProperty]
        private PaymentTypeEnum selectedPaymentType;

        [ObservableProperty]
        private List<CardType> cardTypes;

        [ObservableProperty]
        private CardType selectedCardType;

        [ObservableProperty]
        private PaymentStatusEnum? paymentStatus;

        [ObservableProperty]
        private string? errorStatus = "";

        private DataContext _context;

        public PaymentMethodViewModels()
        {
            PaymentTypes = [PaymentTypeEnum.Cash, PaymentTypeEnum.Card];
            CardTypes = [CardType.Visa, CardType.MasterCard, CardType.AmericanExpress];
            _context = new DataContext();
        }

        [RelayCommand]
        public void ProcessPayment()
        {
            if (TableNumber is not null)
            {
                var errorString = "";
                var invoice = new InvoiceBuilder().BuildInvoice(_context, TableNumber, ref errorString);
                
                if(errorString != "" || invoice == null)
                {
                    ErrorStatus = errorString;
                    return;
                }

                var payment = ReadPayment();
                if (PaymentProcessor.ProcessPayment(_context, payment, invoice))
                {
                    PaymentStatus = PaymentStatusEnum.Success;
                }
                else
                {
                    PaymentStatus = PaymentStatusEnum.Failed;
                }
            }
        }

        public Payment ReadPayment()
        {
            if (SelectedPaymentType == PaymentTypeEnum.Cash)
            {
                return new CashPayment();
            }
            else
            {
                return new CardPayment(SelectedCardType);
            }
        }
    }
}

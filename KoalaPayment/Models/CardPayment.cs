namespace KoalaPayment.Models
{
    enum CardType
    {
        Visa,
        MasterCard,
        AmericanExpress
    }
    internal class CardPayment : Payment
    {
        private readonly IPaymentGateway paymentGateway;

        public override string PaymentType => "Card";

        public CardPayment(CardType cardType)
        {
            if(cardType == CardType.Visa)
            {
                paymentGateway = new VisaGateway();
            }
            else if(cardType == CardType.MasterCard)
            {
                paymentGateway = new MasterCardGateway();
            }
            else if(cardType == CardType.AmericanExpress)
            {
                paymentGateway = new AmericanExpressGateway();
            }
            else
            {
                paymentGateway = new VisaGateway();
            }
        }

        public override bool Process(double totalAmount)
        {
            return paymentGateway.validatePayment("validation string");
        }
    }
}

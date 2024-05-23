using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaPayment.Models
{
    internal class CashPayment: Payment
    {
        public override string PaymentType => "Cash";
        public override bool Process(double totalAmount)
        {
            return true;
        }
    }
}

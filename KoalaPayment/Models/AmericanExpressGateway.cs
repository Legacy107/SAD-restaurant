using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaPayment.Models
{
    internal class AmericanExpressGateway: IPaymentGateway
    {
        public bool validatePayment(string validationString)
        {
            return false;
        }
    }
}

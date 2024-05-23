using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaPayment.Models
{
    internal interface IPaymentReader
    {
        public Payment ReadPayment();
    }
}

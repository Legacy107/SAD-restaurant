using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoalaPayment.Models
{
    internal abstract class Payment
    {
        public abstract string PaymentType { get; }
        public abstract bool Process(double totalAmount);
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class Receipt
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string PaymentType { get; set; } = "";
        public double CustomerPay { get; set; } = 0;
        public double Change { get; set; } = 0;
        public DateTime PaymentTime { get; set; } = DateTime.Now;
        public virtual Invoice Invoice { get; set; } = null!;
    }
}

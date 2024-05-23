using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime InvoiceTime { get; set; } = DateTime.Now;
        public virtual Order Order { get; set; } = null!;

        public ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();
    }
}

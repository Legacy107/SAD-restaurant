using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public string ItemName { get; set; } = "";
        public double Price { get; set; }
        public virtual Invoice Invoice { get; set; } = null!;
    }
}

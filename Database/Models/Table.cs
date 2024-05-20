namespace Database.Models
{
    public class Table
    {
        public int Id { get; set; }
        public string Traits { get; set; }
        public ICollection<TableReservation> Reservations { get; set; } = new List<TableReservation>();
        public ICollection<TableCheckIn> CheckIns { get; set; } = new List<TableCheckIn>();
    }
}

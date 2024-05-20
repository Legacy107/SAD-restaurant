namespace Database.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public string CName { get; set; }
        public DateTime ReservationStart { get; set; }
        public bool HasShownUp { get; set; } = false;
        public ICollection<TableReservation> Tables { get; set; } = new List<TableReservation>();
    }
}

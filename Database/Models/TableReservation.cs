namespace Database.Models
{
    public class TableReservation
    {
        public int TableId { get; set; }
        public Table Table { get; set; }
        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; }
    }
}

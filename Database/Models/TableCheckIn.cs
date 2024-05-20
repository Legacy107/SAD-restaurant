namespace Database.Models
{
    public class TableCheckIn
    {
        public int TableId { get; set; }
        public Table Table { get; set; }
        public Guid CheckInId { get; set; }
        public CheckIn CheckIn { get; set; }
    }
}

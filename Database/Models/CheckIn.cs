namespace Database.Models
{
    public class CheckIn
    {
        public Guid Id { get; set; }
        public DateTime CheckInStart { get; set; } = DateTime.Now;
        public bool IsFinished { get; set; } = false;
        public ICollection<TableCheckIn> Tables { get; set; } = new List<TableCheckIn>();
    }
}

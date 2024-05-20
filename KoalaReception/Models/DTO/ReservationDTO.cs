
namespace KoalaReception.Models.DTO
{
    public class ReservationDTO
    {
        public Guid Id { get; set; }
        public string? CName { get; set; }
        public DateTime ReservationStart { get; set; }

        public List<int> TableIds { get; set; } = new List<int>();
    }
}

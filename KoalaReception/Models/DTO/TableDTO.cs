
namespace KoalaReception.Models.DTO
{
    public class TableDTO
    {
        public int TableId { get; set; }
        public required string Traits { get; set; }
        public TableStatusEnum Status { get; set; }
        public DateTime? AffectedStartingTime { get; set; }
        public bool OnFiltered { get; set; } = false;

        public override bool Equals(object? obj)
        {
            if (obj is TableDTO other)
            {
                // Compare the properties that uniquely identify a TableDTO
                return TableId == other.TableId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            // Implement GetHashCode for consistency with Equals
            return TableId.GetHashCode();
        }
    }
}

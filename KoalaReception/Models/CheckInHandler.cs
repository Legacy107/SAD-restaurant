using Database.Data;
using Microsoft.EntityFrameworkCore;

namespace KoalaReception.Models
{
    public class CheckInHandler
    {
        private readonly DataContext _context;
        public CheckInHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<string> CheckInWithReservation(Guid reservationId)
        {
            var currentTime = DateTime.Now;
            var reservationExist = await _context.Reservations
                                    .Include(r => r.Tables)
                                    .AnyAsync(r => r.Id == reservationId && !r.HasShownUp
                                        && r.ReservationStart > currentTime.AddMinutes(-30)
                                        && r.ReservationStart < currentTime.AddMinutes(30));
            
            if (!reservationExist) return "Sorry the reservation is invalid";

            var reservation = await _context.Reservations
                                    .Include(r => r.Tables)
                                    .FirstAsync(r => r.Id == reservationId
                                        && r.ReservationStart > currentTime.AddMinutes(-30)
                                        && r.ReservationStart < currentTime.AddMinutes(30));

            var reservedTableIds = new List<int>();
            foreach (var tableReservation in reservation.Tables)
            {
                reservedTableIds.Add(tableReservation.TableId);
            }

            var areReservedTablesBeingOccupied = await _context.CheckIns
                          .Include(r => r.Tables)
                          .AnyAsync(c => !c.IsFinished && c.Tables.Any(tc => reservedTableIds.Contains(tc.TableId)));

            if (areReservedTablesBeingOccupied) return "Sorry the tables are currently being occupied. Please wait for at most 30 minutes";

            reservation.HasShownUp = true;

            await CheckInWithoutReservation(reservedTableIds);
            return "Check in success! Please proceed to tables: " + string.Join(",", reservedTableIds);
        }

        public async Task<bool> CheckInWithoutReservation(List<int> tableIds)
        {
            var checkIn = new Database.Models.CheckIn();

            var tables = await _context.Tables.ToListAsync();

            foreach (var tableId in tableIds)
            {
                var table = tables.FirstOrDefault(t => t.Id == tableId);
                checkIn.Tables.Add(new Database.Models.TableCheckIn
                {
                    Table = table,
                    CheckIn = checkIn
                });
            }
            _context.CheckIns.Add(checkIn);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

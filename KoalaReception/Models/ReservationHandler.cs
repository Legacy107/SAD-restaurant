using Database.Data;
using Microsoft.EntityFrameworkCore;

namespace KoalaReception.Models
{
    public class ReservationHandler
    {
        private readonly DataContext _context;
        public ReservationHandler(DataContext context)
        {
            _context = context;
        }

        public async Task<string> MakeReservation(string name, DateTime period, List<int> tableIds)
        {
            var reservation = new Database.Models.Reservation
            {
                CName = name,
                ReservationStart = period
            };

            var tables = await _context.Tables.ToListAsync();

            foreach (var tableId in tableIds)
            {
                var table = tables.FirstOrDefault(t => t.Id == tableId);
                reservation.Tables.Add(new Database.Models.TableReservation
                {
                    Table = table,
                    Reservation = reservation
                });
            }
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation.Id.ToString();
        }

        public async Task<bool> ChangeReservation(Guid reservationId, string newName, DateTime newPeriod, List<int> newTableIds)
        {
            var reservation = await _context.Reservations.FirstAsync(r => r.Id == reservationId && r.ReservationStart > DateTime.Now.AddHours(2));
            if (reservation == null) return false;

            var tables = await _context.Tables.ToListAsync();
            var tableReservations = new List<Database.Models.TableReservation>();

            foreach (var tableId in newTableIds)
            {
                var table = tables.FirstOrDefault(t => t.Id == tableId);
                tableReservations.Add(new Database.Models.TableReservation
                {
                    Table = table,
                    TableId = tableId,
                    ReservationId = reservation.Id,
                    Reservation = reservation
                });
            }
            reservation.ReservationStart = newPeriod;
            reservation.CName = newName;
            reservation.Tables = tableReservations;

            return await _context.SaveChangesAsync() > 0;
        }
    }
}

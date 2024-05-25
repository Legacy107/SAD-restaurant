using Database.Data;
using KoalaReception.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace KoalaReception.Models
{
    public class TableWrapper
    {
        private readonly DataContext _context;
        public List<TableDTO> Tables { get; set; }

        public TableWrapper(DataContext context)
        {
            _context = context;
            Tables = new List<TableDTO>();
        }

        public async Task GetTables(TaskEnum task, DateTime period = default)
        {
            if (task == TaskEnum.Reservation)
            {
                Tables = await GetTablesForReservation(period);
            } else
            {
                Tables = await GetTablesForCheckIn();
            }
        }

        private async Task<List<TableDTO>> GetTablesForCheckIn()
        {
            var result = new List<TableDTO>();
            var reservedTableIds = new List<int>();
            var checkedInTableIds = new List<int>();
            var currentTime = DateTime.Now;

            var reservedTables = await _context.Tables
                                               .Include(t => t.Reservations)
                                               .ThenInclude(tr => tr.Reservation)
                                               .Where(t => t.Reservations.Any(
                                                    tr => !tr.Reservation.HasShownUp && (tr.Reservation.ReservationStart > currentTime.AddMinutes(-30) && tr.Reservation.ReservationStart < currentTime.AddHours(2))
                                                   )
                                               )
                                               .ToListAsync();
            foreach (var reservedTable in reservedTables)
            {
                result.Add(new TableDTO
                {
                    TableId = reservedTable.Id,
                    Traits = reservedTable.Traits,
                    Status = TableStatusEnum.Reserved,
                    AffectedStartingTime = reservedTable.Reservations
                                                        .FirstOrDefault(
                                                            tr => !tr.Reservation.HasShownUp &&
                                                                (tr.Reservation.ReservationStart > currentTime.AddMinutes(-30) && tr.Reservation.ReservationStart < currentTime.AddHours(2))
                                                        )!.Reservation.ReservationStart
                });
                reservedTableIds.Add(reservedTable.Id);
            }

            var occupiedTables = await _context.Tables
                                        .Include(t => t.CheckIns)
                                        .ThenInclude(tc => tc.CheckIn)
                                        .Where(t => t.CheckIns.Any(tc => !tc.CheckIn.IsFinished))
                                        .ToListAsync();
            foreach (var occupiedTable in occupiedTables)
            {
                result.Add(new TableDTO
                {
                    TableId = occupiedTable.Id,
                    Traits = occupiedTable.Traits,
                    Status = TableStatusEnum.Occupied,
                    AffectedStartingTime = occupiedTable.CheckIns.FirstOrDefault(tc => !tc.CheckIn.IsFinished)!.CheckIn.CheckInStart
                });
                checkedInTableIds.Add(occupiedTable.Id);
            }

            var allTables = await _context.Tables.ToListAsync();
            foreach (var table in allTables)
            {
                if (!reservedTableIds.Contains(table.Id) && !checkedInTableIds.Contains(table.Id))
                {
                    result.Add(new TableDTO
                    {
                        TableId = table.Id,
                        Traits = table.Traits,
                        Status = TableStatusEnum.Available,
                        AffectedStartingTime = null
                    });
                }
            }

            return result.OrderBy(r => r.TableId).ToList(); ;
        }

        private async Task<List<TableDTO>> GetTablesForReservation(DateTime period)
        {
            var result = new List<TableDTO>();
            var reservedTableIds = new List<int>();

            var reservedTables = await _context.Tables
                                               .Include(t => t.Reservations)
                                               .ThenInclude(tr => tr.Reservation)
                                               .Where(t => t.Reservations.Any(
                                                    tr => !tr.Reservation.HasShownUp && 
                                                    (tr.Reservation.ReservationStart > period.AddHours(-2) && tr.Reservation.ReservationStart < period.AddHours(2))
                                                   )
                                               )
                                               .ToListAsync();

            foreach ( var reservedTable in reservedTables )
            {
                result.Add(new TableDTO
                {
                    TableId = reservedTable.Id,
                    Traits = reservedTable.Traits,
                    Status = TableStatusEnum.Reserved,
                    AffectedStartingTime = reservedTable.Reservations
                                                        .FirstOrDefault(
                                                            tr => !tr.Reservation.HasShownUp &&
                                                                (tr.Reservation.ReservationStart > period.AddHours(-2) && tr.Reservation.ReservationStart < period.AddHours(2))
                                                        )!.Reservation.ReservationStart
                });
                reservedTableIds.Add(reservedTable.Id);
            }

            var allTables = await _context.Tables.ToListAsync();
            foreach ( var table in allTables ) 
            {
                if (!reservedTableIds.Contains(table.Id))
                {
                    result.Add(new TableDTO
                    {
                        TableId = table.Id,
                        Traits = table.Traits,
                        Status = TableStatusEnum.Available,
                        AffectedStartingTime = null
                    });
                }
            }

            return result.OrderBy(r => r.TableId).ToList();
        }

        public async Task<ReservationDTO?> CheckReservationExists(Guid reservationId)
        {
            var currentTime = DateTime.Now;
            var validReservationExists = await _context.Reservations
                                    .AnyAsync(r => r.Id == reservationId && r.ReservationStart > currentTime.AddHours(2));
            if (validReservationExists)
            {
                var reservation = await _context.Reservations
                                    .Include(r => r.Tables)
                                    .FirstAsync(r => r.Id == reservationId);
                var tableIds = new List<int>();
                foreach ( var table in reservation!.Tables )
                {
                    tableIds.Add(table.TableId);
                }

                return new ReservationDTO
                {
                    Id = reservationId,
                    CName = reservation.CName,
                    ReservationStart = reservation.ReservationStart,
                    TableIds = tableIds
                };
            }

            return null;
        }
    }
}

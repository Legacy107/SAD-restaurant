using KoalaReception.Models.DTO;

namespace KoalaReception.Models
{
    public class Reservation
    {
        private TableWrapper _tableWrapper;
        private ReservationHandler _handler;

        public Reservation(TableWrapper tableWrapper, ReservationHandler handler)
        {
            _tableWrapper = tableWrapper;
            _handler = handler;
        }

        public async Task<List<TableDTO>> GetTables(DateTime period)
        {
            // Populate tables in the wrapper
            await _tableWrapper.GetTables(TaskEnum.Reservation, period);
            return _tableWrapper.Tables;
        }

        public async Task<string> MakeReservation(string name, DateTime period, List<int> tableIds)
        {
            return await _handler.MakeReservation(name, period, tableIds);
        }
        public async Task<bool> ChangeReservation(Guid reservationId, string newName, DateTime newPeriod, List<int> newTableIds)
        {
            return await _handler.ChangeReservation(reservationId, newName, newPeriod, newTableIds);
        }

        public async Task<ReservationDTO?> CheckReservation(string reservationId)
        {
            Guid guid;
            if (Guid.TryParse(reservationId, out guid))
            {
                return await _tableWrapper.CheckReservationExists(guid);
            } else
            {
                return null;
            }
        }
    }
}

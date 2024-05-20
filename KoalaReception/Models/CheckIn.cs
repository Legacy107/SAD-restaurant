using KoalaReception.Models.DTO;

namespace KoalaReception.Models
{
    public class CheckIn
    {
        private TableWrapper _tableWrapper;
        private CheckInHandler _handler;

        public CheckIn(TableWrapper tableWrapper, CheckInHandler handler)
        {
            _tableWrapper = tableWrapper;
            _handler = handler;
        }

        public async Task<List<TableDTO>> GetTables()
        {
            // Populate tables in the wrapper
            await _tableWrapper.GetTables(TaskEnum.CheckIn);
            return _tableWrapper.Tables;
        }

        public async Task<string> CheckInWithReservation(Guid reservationId) 
        {
            return await _handler.CheckInWithReservation(reservationId);
        }

        public async Task<bool> CheckInWithoutReservation(List<int> tableIds)
        {
            return await _handler.CheckInWithoutReservation(tableIds);
        }
    }
}

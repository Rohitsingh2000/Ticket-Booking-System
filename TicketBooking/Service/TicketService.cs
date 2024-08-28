using TicketBooking.Data;

namespace TicketBooking.Service
{
    public class TicketService
    {
        private readonly ApplicationDbContext _context;
        public TicketService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public List<string> GetAvailableSeatsCodes(int numberOfRows, int seatPerRows, int showId)
        {
            var seats = new List<string>();
            int seatCounter = 0;

            for (int i = 0; i < numberOfRows; i++)
            {
                char rowLabel = (char)('A' + i);
                for (int j = 1; j <= seatPerRows; j++)
                {
                    if (!_context.Seats.Where(x => x.SeatNumber == $"{rowLabel}{j}" && x.ShowID == showId).Any())
                    {
                        seatCounter++;
                        seats.Add($"{rowLabel}{j}");
                    }
                }
            }
            return seats;
        }
    }
}

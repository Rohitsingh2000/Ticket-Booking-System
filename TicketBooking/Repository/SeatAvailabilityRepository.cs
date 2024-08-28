using Microsoft.EntityFrameworkCore;
using TicketBooking.Data;
using TicketBooking.Models;

namespace TicketBooking.Repository
{
    public class SeatAvailabilityRepository
    {
        private readonly ApplicationDbContext _context;

        public SeatAvailabilityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Show> GetShows(int showId)
        {
            var show = await _context.Shows
                .Include(s => s.Theatre)
                .Include(s => s.Movie)
                .Include(s => s.Seats)
                .FirstOrDefaultAsync(s => s.Id == showId);

            return show;
        }
    }
}

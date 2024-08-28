using Microsoft.EntityFrameworkCore;
using TicketBooking.Data;
using TicketBooking.Models;

namespace TicketBooking.Repository
{
    public class TicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> DeleteTicket(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null)
            {
                return "Ticket not found";
            }

            var show = await (_context.Shows.FindAsync(ticket.ShowID));
            var seat = await (_context.Seats.Where(x => x.TicketId == ticketId).ToListAsync());
            if (show != null)
            {
                show.AvailableSeats += seat.Count;
                _context.RemoveRange(seat);
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return "Ticket canceled successfully";
        }

        public async Task<Show> GetShow(int showId)
        {
            var show = await _context.Shows
                .Include(s => s.Tickets)
                .Include(s => s.Movie)
                .Include(s => s.Theatre)
                .FirstOrDefaultAsync(s => s.Id == showId);

            return show;
        }

        public async Task AddTicket(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task AddSeats(List<Seat> seat) {
            await _context.Seats.AddRangeAsync(seat);
            await _context.SaveChangesAsync();
        }
    }
}

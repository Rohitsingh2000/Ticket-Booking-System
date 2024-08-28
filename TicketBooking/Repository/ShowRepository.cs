using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Data;
using TicketBooking.DTO;
using TicketBooking.Models;

namespace TicketBooking.Repository
{
    public class ShowRepository
    {
        private readonly ApplicationDbContext _context;

        public ShowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Show> AddShowRepository(ShowDto showDto)
        {
            var show = new Show
            {
                StartTime = showDto.StartTime,
                TheatreId = showDto.TheatreId,
                MovieId = showDto.MovieId,
                TotalSeats = showDto.TotalSeats,
                AvailableSeats = showDto.AvailableSeats,
                NumberOfRows = showDto.NumberOfRows,
                SeatsPerRow = showDto.SeatsPerRow
            };

            _context.Shows.Add(show);
            await _context.SaveChangesAsync();
            return show;
        }

        public async Task<List<Show>> GetAllShows(string location)
        {
            var shows = await _context.Shows
               .Include(t => t.Theatre)
               .Where(t => t.Theatre.Location.ToLower() == location.ToLower())
               .ToListAsync();

            return shows;
        }

        public async Task<string> UpdateShowRepo(int showId, ShowDto showDto)
        {
            var show = await _context.Shows.FindAsync(showId);
            if (show == null)
            {
                return "Show not found";
            }

            show.StartTime = showDto.StartTime;
            show.TotalSeats = showDto.TotalSeats;
            show.AvailableSeats = showDto.AvailableSeats;
            show.MovieId = showDto.MovieId;
            show.TheatreId = showDto.TheatreId;

            await _context.SaveChangesAsync();

            return "Show updated successfully";
        }

        public async Task<string> DeleteShowByIdRepo(int showID)
        {
            var show = await _context.Shows.FindAsync(showID);
            if (show == null)
            {
                return "Show not found";
            }

            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();

            return "Show deleted successfully";
        }
    }
}

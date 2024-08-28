using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Data;
using TicketBooking.DTO;
using TicketBooking.Models;

namespace TicketBooking.Repository
{
    public class TheatreRepository
    {
        private readonly ApplicationDbContext _context;

        public TheatreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Theatre> AddTheatreUsingRepository(TheatreDto theatreDto)
        {
            var theatre = new Theatre
            {
                Name = theatreDto.Name,
                Location = theatreDto.Location
            };

            _context.Theatres.Add(theatre);
            await _context.SaveChangesAsync();

            return theatre;
        }

        public async Task<List<Show>> GetShowUsingTheatreRepository(int theatreId)
        {
            var shows = await _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Theatre)
                .Where(s => s.TheatreId == theatreId)
                .ToListAsync();

            return shows;
        }

        public async Task<List<Theatre>> GetTheatres(string location)
        {
            var theatres = await _context.Theatres
               .Include(t => t.Shows)
               .Where(t => t.Location.ToLower() == location.ToLower())
               .ToListAsync();

            return theatres;
        }

        public async Task<string> UpdateTheatre(int theatreId, TheatreDto theatreDto)
        {
            var theatre = await _context.Theatres.FindAsync(theatreId);
            if (theatre == null)
            {
                return "Show not found";
            }

            theatre.Location = theatreDto.Location;
            theatre.Name = theatreDto.Name;

            await _context.SaveChangesAsync();

            return "Theatre updated successfully";
        }

        public async Task<string> DeleteTheatre(int theatreId)
        {
            var theatre = await _context.Theatres.FindAsync(theatreId);
            if (theatre == null)
            {
                return "Theatre not found";
            }

            _context.Theatres.Remove(theatre);
            await _context.SaveChangesAsync();

            return "Theatre deleted successfully";
        }
    }
}

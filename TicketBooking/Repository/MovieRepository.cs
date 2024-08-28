using iText.Svg.Renderers.Path.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Data;
using TicketBooking.DTO;
using TicketBooking.Models;

namespace TicketBooking.Repository
{
    public class MovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MovieDto>> GetMovies(String location)
        {
            var movies = await _context.Movies
               .Where(m => m.Shows.Any(s => s.Theatre.Location == location))
               .Select(m => new MovieDto
               {
                   Title = m.Title,
                   Genre = m.Genre,
                   Duration = m.Duration
               })
               .ToListAsync();

            return movies;
        }

        public async Task<Movie> AddMovies(MovieDto movieDto)
        {
            var movie = new Movie
            {
                Title = movieDto.Title,
                Genre = movieDto.Genre,
                Duration = movieDto.Duration,
                Shows = new List<Show>()
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<Movie> GetMovieUsingId(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            return movie;
        }
    }
}

using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Data;
using TicketBooking.DTO;
using TicketBooking.Models;
using TicketBooking.Repository;

namespace TicketBooking.Controllers
{
    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        private readonly MovieRepository _movieRepository;

        public MovieController(ILogger<MovieController> logger, MovieRepository movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> SearchMovies(String location)
        {
            var movies = await _movieRepository.GetMovies(location);

            _logger.LogTrace($"Fetched movies which are present at location: {location}");

            return Ok(movies);
        }

        [HttpPost("Add")]
        [Authorize(Roles = "TheatreOwner")]
        public async Task<IActionResult> AddMovie([FromBody] MovieDto movieDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = await _movieRepository.AddMovies(movieDto);

            _logger.LogTrace("Added Movie");

            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        [HttpGet("id")]
        [Authorize]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieRepository.GetMovieUsingId(id);

            if(movie == null)
            {
                _logger.LogTrace("Movie not present in database");
                return NotFound();
            }

            _logger.LogTrace($"Movie id {id}");
            return Ok(movie);
        }
    }
}

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
    [ApiController]
    [Route("api/[controller]")]
    public class TheatreController : Controller
    {
        private readonly ILogger<TheatreController> _logger;
        private readonly TheatreRepository _repository;
        public TheatreController(ILogger<TheatreController> logger, TheatreRepository theatreRepository)
        {
            _logger = logger;
            _repository = theatreRepository;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTheatre(TheatreDto theatreDto)
        {
            var theatre = await _repository.AddTheatreUsingRepository(theatreDto);

            _logger.LogTrace("New theatre added in database");

            return Ok("Theatre added succesfully");
        }

        [HttpGet("Theatre/{theatreId}")]
        [Authorize]
        public async Task<IActionResult> GetShowByTheatreId(int theatreId)
        {
            var shows = await _repository.GetShowUsingTheatreRepository(theatreId);

            if (shows == null || !shows.Any())
            {
                _logger.LogTrace("Shows not found for specific theatre id");
                return NotFound("No shows found for the specified theatre");
            }

            return Ok(shows);
        }

        [HttpGet("Location/{location}")]
        [Authorize]
        public async Task<IActionResult> GetTheatresByLocation(string location)
        {

            var theatres = await _repository.GetTheatres(location);

            if (theatres == null || !theatres.Any())
                return NotFound("No theatres found in the specified location.");

            _logger.LogTrace($"Fetched theatres which are present at location: {location}");
            return Ok(theatres);
        }

        [HttpPost("Update{theatreId}")]
        [Authorize(Roles = "TheatreOwner")]
        public async Task<IActionResult> UpdateTheatre(int theatreId, TheatreDto theatreDto)
        {
            var theatre = await _repository.UpdateTheatre(theatreId, theatreDto);

            _logger.LogTrace("Theatre details updated successfully");
            return Ok(theatre);
        }

        [HttpDelete("Delete/{theatreId}")]
        [Authorize(Roles = "TheatreOwner")]
        public async Task<IActionResult> DeleteShowById(int theatreId)
        {
            var theatre = await _repository.DeleteTheatre(theatreId);

            _logger.LogTrace("Theatre removed from database");
            return Ok(theatre);
        }
    }
}

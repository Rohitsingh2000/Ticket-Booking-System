using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketBooking.Data;
using TicketBooking.DTO;
using TicketBooking.Models;
using TicketBooking.Repository;

namespace TicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "TheatreOwner")]
    public class ShowController : Controller
    {
        private readonly ILogger<ShowController> _logger;
        private readonly ShowRepository _showRepository;
        public ShowController(ILogger<ShowController> logger, ShowRepository showRepository)
        {
            _logger = logger;
            _showRepository = showRepository;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddShow(ShowDto showDto)
        {
            var show =await _showRepository.AddShowRepository(showDto);
            _logger.LogTrace("New show added successfully");
            return Ok(show);
        }

        [HttpPut("update/{showId}")]
        public async Task<IActionResult> UpdateShow(int showId, ShowDto showDto)
        {
            var show = await _showRepository.UpdateShowRepo(showId, showDto);

            return Ok(show);
        }

        [HttpGet("Location/{location}")]
        [Authorize]
        public async Task<IActionResult> GetShowsByLocation(string location)
        {

            var shows = await _showRepository.GetAllShows(location);

            if (shows == null || !shows.Any())
                return NotFound("No theatres found in the specified location.");

            _logger.LogTrace($"Fetched theatres which are present at location: {location}");
            return Ok(shows);
        }

        [HttpDelete("Delete/{showID}")]
        [Authorize(Roles ="TheatreOwner")]
        public async Task<IActionResult> DeleteShowById(int showID)
        {
            var show = await _showRepository.DeleteShowByIdRepo(showID);

            _logger.LogTrace($"Show Deleted Successfully: {showID}");
            return Ok(show);
        }
    }
}

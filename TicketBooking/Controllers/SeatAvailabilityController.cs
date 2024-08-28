using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Data;
using TicketBooking.Repository;
using TicketBooking.Service;

namespace TicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SeatAvailabilityController : Controller
    {
        private readonly TicketService _ticketService;
        private readonly ILogger<SeatAvailabilityController> _logger;
        private readonly SeatAvailabilityRepository _seatAvailabilityRepository;

        public SeatAvailabilityController(SeatAvailabilityRepository seatAvailabilityRepository, TicketService ticketService, ILogger<SeatAvailabilityController> logger)
        {
            _ticketService = ticketService;
            _logger = logger;
            _seatAvailabilityRepository = seatAvailabilityRepository;
        }

        [HttpGet("Show/{showId}")]
        public async Task<IActionResult> GetAvailableSeats(int showId)
        {
            var show = await _seatAvailabilityRepository.GetShows(showId);

            if (show == null)
                return NotFound("Show not found");

            var availableSeats = _ticketService.GetAvailableSeatsCodes(show.NumberOfRows, show.SeatsPerRow, showId);

            _logger.LogTrace("Available seats fetched from database and provided as result");
            return Ok(new
            {
                showId = show.Id,
                MovieTitle = show.Movie.Title,
                TheatreName = show.Theatre.Name,
                AvailableSeats = availableSeats
            });
        }
    }
}

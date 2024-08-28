using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using TicketBooking.Data;
using TicketBooking.DTO;
using TicketBooking.Models;
using TicketBooking.Repository;
using TicketBooking.Service;

namespace TicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "TheatreOwner,Customer")]
    public class TicketController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IPdfService _pdfService;
        private readonly TicketService _ticketService;
        private readonly ILogger<TicketController> _logger;
        private readonly TicketRepository _ticketRepository;

        public TicketController(TicketRepository ticketRepository, UserManager<User> userManager, IPdfService pdfService, TicketService ticketService, ILogger<TicketController> logger)
        {
            _ticketRepository = ticketRepository;
            _userManager = userManager;
            _pdfService = pdfService;
            _ticketService = ticketService;
            _logger = logger;
        }

        [HttpPost("book")]
        public async Task<IActionResult> BookTicket(int showId, int numberOfSeats,[FromBody] List<string> SeatNumbers)
        {
            if (numberOfSeats < 1 || numberOfSeats > 5)
            {
                _logger.LogTrace("Incorrect number of seat selected");
                return BadRequest("You can book a minimun of 1 and a maximun of 5 seats a time");
            }

            var show = await _ticketRepository.GetShow(showId);

            if (show == null || show.AvailableSeats < numberOfSeats)
            {
                _logger.LogTrace("Selected seats are already booked");
                return BadRequest("Seats not Available");
            }

            var userId = User.FindFirstValue("user_id");
            if (userId == null)
            {
                _logger.LogTrace("User doesnot have required acesss to book tickets");
                return Unauthorized();
            }

            var tickets = new List<Ticket>();
            var seats = new List<Seat>();

            var availableSeats = _ticketService.GetAvailableSeatsCodes(show.NumberOfRows, show.SeatsPerRow, show.Id);

            foreach(var seat in SeatNumbers)
            {
                if(!availableSeats.Contains(seat))
                {
                    return BadRequest();
                }
            }

            Ticket ticket = new Ticket()
            {
                ShowID = showId,
                UserId = userId,
            };

            await _ticketRepository.AddTicket(ticket);

            foreach (var seat in SeatNumbers)
            {
                seats.Add(new Seat()
                {
                    IsBooked = true,
                    SeatNumber = seat,
                    ShowID = showId,
                    TicketId = ticket.Id
                });
            }
            show.AvailableSeats -= numberOfSeats;

            await _ticketRepository.AddSeats(seats);

            var pdfBytes = _pdfService.GenerateTicketPdf(ticket, show, userId);

            _logger.LogTrace("Ticket booked successfully");
            return File(pdfBytes, "application/pdf", "Ticket.pdf");
                }


        [HttpDelete("cancel/{ticketId}")]
        public async Task<IActionResult> CancelTicket(int ticketId)
        {
            var ticket = await _ticketRepository.DeleteTicket(ticketId);

            _logger.LogTrace($"Cancel ticket: {ticketId}");
            return Ok(new { Message = "Ticket canceled successfully" });
        }
    }
}
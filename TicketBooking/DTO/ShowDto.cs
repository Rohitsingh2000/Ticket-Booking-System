using TicketBooking.Models;

namespace TicketBooking.DTO
{
    public class ShowDto
    {
        public DateTime StartTime { get; set; }
        public int TotalSeats { get; set; }
        public int MovieId { get; set; }
        public int TheatreId { get; set; }
        public int AvailableSeats { get; set; }
        public int NumberOfRows { get; set; }
        public int SeatsPerRow { get;set; }

    }
}

using System.Text.Json.Serialization;

namespace TicketBooking.Models
{
    public class Show
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public int TotalSeats { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int TheatreId { get; set; }
        public Theatre Theatre { get; set; }
        public int AvailableSeats { get; set; }
        public int NumberOfRows { get; set; }
        public int SeatsPerRow { get; set; }
        public ICollection<Seat>? Seats { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }

    }
}

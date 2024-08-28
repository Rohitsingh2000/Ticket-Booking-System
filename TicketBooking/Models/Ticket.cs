namespace TicketBooking.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int ShowID { get; set; }
        public Show Show { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}

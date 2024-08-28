namespace TicketBooking.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }   
        public bool IsBooked { get; set; }
        public int ShowID { get; set; }
        public Show Show { get; set; }
        public Ticket? Ticket { get; set; }
        public int? TicketId { get; set; }
    }
}

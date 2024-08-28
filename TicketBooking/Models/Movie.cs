namespace TicketBooking.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String Genre { get; set; }
        public int Duration { get; set; }
        public IEnumerable<Show> Shows { get; set; }
    }
}

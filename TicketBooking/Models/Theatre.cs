namespace TicketBooking.Models
{
    public class Theatre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public ICollection<Show>? Shows { get; set; }

    }
}

using Microsoft.AspNetCore.Identity;

namespace TicketBooking.Models
{
    public class User : IdentityUser
    {
        public IQueryable<Ticket>? Tickets { get; set; }
    }
}

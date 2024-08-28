using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketBooking.Models;
using static iText.IO.Util.IntHashtable;

namespace TicketBooking.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Seat> Seats { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Show>()
                .HasMany(s => s.Tickets)
                .WithOne(ticket => ticket.Show)
                .HasForeignKey(ticket => ticket.ShowID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(u => u.Tickets)
                .WithOne(s => s.User)
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Movie>()
                .HasMany(m => m.Shows)
                .WithOne(mo => mo.Movie)
                .HasForeignKey(x=> x.MovieId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Theatre>()
                .HasMany(t => t.Shows)
                .WithOne(s => s.Theatre)
                .HasForeignKey(t => t.TheatreId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Show>()
                .HasMany(s => s.Seats)
                .WithOne(st => st.Show)
                .HasForeignKey(t => t.ShowID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Ticket>()
                .HasMany(x => x.Seats)
                .WithOne(x => x.Ticket)
                .HasForeignKey(x => x.TicketId)
                .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);


            base.OnModelCreating(builder);
        }
    }
}

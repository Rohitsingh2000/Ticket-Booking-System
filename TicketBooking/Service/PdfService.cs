using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using TicketBooking.Models;

namespace TicketBooking.Service
{
    public interface IPdfService
    {
        byte[] GenerateTicketPdf(Ticket ticket, Show show, string userID);
    }

    public class PdfService : IPdfService
    {
        public byte[] GenerateTicketPdf(Ticket ticket, Show show, string userId)
        {
            using (var memoryStream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(memoryStream);
                PdfDocument pdf = new PdfDocument(writer);
                Document doucument = new Document(pdf);

                var seats = String.Join(", ", ticket.Seats.Select(x => x.SeatNumber).ToList());

                doucument.Add(new Paragraph($"Ticket Confirmation for {show.Movie.Title}"));
                doucument.Add(new Paragraph($"Theatre: {show.Theatre.Name}"));
                doucument.Add(new Paragraph($"User ID: {userId}"));
                doucument.Add(new Paragraph("Seats: " + seats));

                doucument.Close();
                return memoryStream.ToArray();

            }
        }
    }
}

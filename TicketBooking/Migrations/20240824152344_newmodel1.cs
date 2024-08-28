using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketBooking.Migrations
{
    /// <inheritdoc />
    public partial class newmodel1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Shows_ShowID",
                table: "Seat");

            migrationBuilder.DropForeignKey(
                name: "FK_Seat_Tickets_TicketId",
                table: "Seat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seat",
                table: "Seat");

            migrationBuilder.RenameTable(
                name: "Seat",
                newName: "Seats");

            migrationBuilder.RenameIndex(
                name: "IX_Seat_TicketId",
                table: "Seats",
                newName: "IX_Seats_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Seat_ShowID",
                table: "Seats",
                newName: "IX_Seats_ShowID");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRows",
                table: "Shows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeatsPerRow",
                table: "Shows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seats",
                table: "Seats",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Shows_ShowID",
                table: "Seats",
                column: "ShowID",
                principalTable: "Shows",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Tickets_TicketId",
                table: "Seats",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Shows_ShowID",
                table: "Seats");

            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Tickets_TicketId",
                table: "Seats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seats",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "NumberOfRows",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "SeatsPerRow",
                table: "Shows");

            migrationBuilder.RenameTable(
                name: "Seats",
                newName: "Seat");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_TicketId",
                table: "Seat",
                newName: "IX_Seat_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Seats_ShowID",
                table: "Seat",
                newName: "IX_Seat_ShowID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seat",
                table: "Seat",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Shows_ShowID",
                table: "Seat",
                column: "ShowID",
                principalTable: "Shows",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Seat_Tickets_TicketId",
                table: "Seat",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class includePeferredName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreferredName",
                table: "ReservationPeople",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredName",
                table: "ReservationPeople");
        }
    }
}

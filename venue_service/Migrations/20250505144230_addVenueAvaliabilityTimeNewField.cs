using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace venue_service.Migrations
{
    /// <inheritdoc />
    public partial class addVenueAvaliabilityTimeNewField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "reserved_by",
                table: "venue_availability",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "venue_availability",
                keyColumn: "id",
                keyValue: 1,
                column: "reserved_by",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "reserved_by",
                table: "venue_availability");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace venue_service.Migrations
{
    /// <inheritdoc />
    public partial class FixStatusToVenueStatusId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_location_availability_times_venue_status_StatusId",
                table: "location_availability_times");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "location_availability_times",
                newName: "venue_status_id");

            migrationBuilder.RenameIndex(
                name: "IX_location_availability_times_StatusId",
                table: "location_availability_times",
                newName: "IX_location_availability_times_venue_status_id");

            migrationBuilder.AddForeignKey(
                name: "FK_location_availability_times_venue_status_venue_status_id",
                table: "location_availability_times",
                column: "venue_status_id",
                principalTable: "venue_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_location_availability_times_venue_status_venue_status_id",
                table: "location_availability_times");

            migrationBuilder.RenameColumn(
                name: "venue_status_id",
                table: "location_availability_times",
                newName: "StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_location_availability_times_venue_status_id",
                table: "location_availability_times",
                newName: "IX_location_availability_times_StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_location_availability_times_venue_status_StatusId",
                table: "location_availability_times",
                column: "StatusId",
                principalTable: "venue_status",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

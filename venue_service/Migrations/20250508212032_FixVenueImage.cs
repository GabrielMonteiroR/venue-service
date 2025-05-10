using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace venue_service.Migrations
{
    /// <inheritdoc />
    public partial class FixVenueImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "file_name",
                table: "venue_images",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                column: "ProfileImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                column: "ProfileImageUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "venue_images",
                keyColumn: "id",
                keyValue: 1,
                column: "file_name",
                value: "image-venue.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "file_name",
                table: "venue_images");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "users");
        }
    }
}

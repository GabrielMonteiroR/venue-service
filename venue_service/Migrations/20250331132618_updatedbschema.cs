using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace venue_service.Migrations
{
    /// <inheritdoc />
    public partial class updatedbschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "equipament_brands",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    brand_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipament_brands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "equipament_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_equipament_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_methods",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_methods", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sports",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sports", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venue_status",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venue_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "venue_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venue_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false),
                    is_banned = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "venues",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    allow_local_payment = table.Column<bool>(type: "boolean", nullable: false),
                    venue_type_id = table.Column<int>(type: "integer", nullable: false),
                    rules = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    owner_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venues", x => x.id);
                    table.ForeignKey(
                        name: "FK_venues_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_venues_venue_types_venue_type_id",
                        column: x => x.venue_type_id,
                        principalTable: "venue_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    venue_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    venue_availability_time_id = table.Column<int>(type: "integer", nullable: false),
                    payment_method_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reservations", x => x.id);
                    table.ForeignKey(
                        name: "FK_reservations_payment_methods_payment_method_id",
                        column: x => x.payment_method_id,
                        principalTable: "payment_methods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_reservations_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reservations_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_venues",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    venue_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_venues", x => new { x.user_id, x.venue_id });
                    table.ForeignKey(
                        name: "FK_user_venues_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_venues_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "venue_availability",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    venue_id = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    time_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venue_availability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_venue_availability_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "venue_contact_infos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    venue_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venue_contact_infos", x => x.id);
                    table.ForeignKey(
                        name: "FK_venue_contact_infos_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "venue_equipaments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    equipament_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    equipament_brand_id = table.Column<int>(type: "integer", nullable: false),
                    equipament_type_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    venue_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venue_equipaments", x => x.id);
                    table.ForeignKey(
                        name: "FK_venue_equipaments_equipament_brands_equipament_brand_id",
                        column: x => x.equipament_brand_id,
                        principalTable: "equipament_brands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_venue_equipaments_equipament_types_equipament_type_id",
                        column: x => x.equipament_type_id,
                        principalTable: "equipament_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_venue_equipaments_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "venue_images",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    image_url = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    venue_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venue_images", x => x.id);
                    table.ForeignKey(
                        name: "FK_venue_images_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "venue_sports",
                columns: table => new
                {
                    venue_id = table.Column<int>(type: "integer", nullable: false),
                    sport_id = table.Column<int>(type: "integer", nullable: false),
                    SportId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_venue_sports", x => new { x.venue_id, x.sport_id });
                    table.ForeignKey(
                        name: "FK_venue_sports_sports_SportId1",
                        column: x => x.SportId1,
                        principalTable: "sports",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_venue_sports_sports_sport_id",
                        column: x => x.sport_id,
                        principalTable: "sports",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_venue_sports_venues_venue_id",
                        column: x => x.venue_id,
                        principalTable: "venues",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "equipament_brands",
                columns: new[] { "id", "brand_name" },
                values: new object[,]
                {
                    { 1, "Nike" },
                    { 2, "Adidas" }
                });

            migrationBuilder.InsertData(
                table: "equipament_types",
                columns: new[] { "id", "type_name" },
                values: new object[,]
                {
                    { 1, "Ball" },
                    { 2, "Net" },
                    { 3, "Racket" }
                });

            migrationBuilder.InsertData(
                table: "payment_methods",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1, "Credit Card", "CreditCard" },
                    { 2, "Pix Payment", "Pix" },
                    { 3, "Payment at Venue", "LocalPayment" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Owner" },
                    { 3, "Athlete" }
                });

            migrationBuilder.InsertData(
                table: "sports",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Football" },
                    { 2, "Basketball" },
                    { 3, "Tennis" }
                });

            migrationBuilder.InsertData(
                table: "venue_status",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1, "Venue is available", "Available" },
                    { 2, "Under maintenance", "Maintenance" },
                    { 3, "Not available", "Unavailable" }
                });

            migrationBuilder.InsertData(
                table: "venue_types",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1, "Indoor court", "Indoor court" },
                    { 2, "Outdoor field", "Outdoor field" },
                    { 3, "Gymnasium", "Gymnasium" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "deleted_at", "email", "first_name", "is_banned", "last_name", "password", "phone", "role_id", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "gabriel@example.com", "Gabriel", false, "Ricardo", "hashedpassword", "123456789", 2, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "joao@example.com", "João", false, "Silva", "hashedpassword", "987654321", 3, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "venues",
                columns: new[] { "id", "address", "allow_local_payment", "capacity", "created_at", "deleted_at", "description", "latitude", "longitude", "name", "owner_id", "rules", "updated_at", "venue_type_id" },
                values: new object[] { 1, "123 Main St", true, 100, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "Main sports court", -23.5505, -46.633299999999998, "Central Court", 1, "No smoking", new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1 });

            migrationBuilder.InsertData(
                table: "reservations",
                columns: new[] { "id", "created_at", "deleted_at", "payment_method_id", "status", "updated_at", "user_id", "venue_availability_time_id", "venue_id" },
                values: new object[] { 1, new DateTime(2025, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, 1, "Pending", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, 1 });

            migrationBuilder.InsertData(
                table: "user_venues",
                columns: new[] { "user_id", "venue_id" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "venue_availability",
                columns: new[] { "Id", "end_date", "start_date", "time_status", "venue_id", "price" },
                values: new object[] { 1, new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 3, 20, 8, 0, 0, 0, DateTimeKind.Utc), "PENDING", 1, 100.0 });

            migrationBuilder.InsertData(
                table: "venue_contact_infos",
                columns: new[] { "id", "email", "phone", "venue_id" },
                values: new object[] { 1, "contact@centralcourt.com", "999999999", 1 });

            migrationBuilder.InsertData(
                table: "venue_equipaments",
                columns: new[] { "id", "equipament_brand_id", "equipament_name", "equipament_type_id", "quantity", "venue_id" },
                values: new object[,]
                {
                    { 1, 1, "Nike Football", 1, 10, 1 },
                    { 2, 2, "Adidas Net", 2, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "venue_images",
                columns: new[] { "id", "image_url", "venue_id" },
                values: new object[] { 1, "https://example.com/image1.jpg", 1 });

            migrationBuilder.InsertData(
                table: "venue_sports",
                columns: new[] { "sport_id", "venue_id", "SportId1" },
                values: new object[,]
                {
                    { 1, 1, null },
                    { 2, 1, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_reservations_payment_method_id",
                table: "reservations",
                column: "payment_method_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_user_id",
                table: "reservations",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_venue_id",
                table: "reservations",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_venues_venue_id",
                table: "user_venues",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_venue_availability_venue_id",
                table: "venue_availability",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "IX_venue_contact_infos_venue_id",
                table: "venue_contact_infos",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "IX_venue_equipaments_equipament_brand_id",
                table: "venue_equipaments",
                column: "equipament_brand_id");

            migrationBuilder.CreateIndex(
                name: "IX_venue_equipaments_equipament_type_id",
                table: "venue_equipaments",
                column: "equipament_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_venue_equipaments_venue_id",
                table: "venue_equipaments",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "IX_venue_images_venue_id",
                table: "venue_images",
                column: "venue_id");

            migrationBuilder.CreateIndex(
                name: "IX_venue_sports_sport_id",
                table: "venue_sports",
                column: "sport_id");

            migrationBuilder.CreateIndex(
                name: "IX_venue_sports_SportId1",
                table: "venue_sports",
                column: "SportId1");

            migrationBuilder.CreateIndex(
                name: "IX_venues_owner_id",
                table: "venues",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_venues_venue_type_id",
                table: "venues",
                column: "venue_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservations");

            migrationBuilder.DropTable(
                name: "user_venues");

            migrationBuilder.DropTable(
                name: "venue_availability");

            migrationBuilder.DropTable(
                name: "venue_contact_infos");

            migrationBuilder.DropTable(
                name: "venue_equipaments");

            migrationBuilder.DropTable(
                name: "venue_images");

            migrationBuilder.DropTable(
                name: "venue_sports");

            migrationBuilder.DropTable(
                name: "venue_status");

            migrationBuilder.DropTable(
                name: "payment_methods");

            migrationBuilder.DropTable(
                name: "equipament_brands");

            migrationBuilder.DropTable(
                name: "equipament_types");

            migrationBuilder.DropTable(
                name: "sports");

            migrationBuilder.DropTable(
                name: "venues");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "venue_types");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}

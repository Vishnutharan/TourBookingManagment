using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourBookingManagment.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingDetailsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingsDetails",
                table: "BookingsDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "BookingsDetails");

            migrationBuilder.RenameTable(
                name: "BookingsDetails",
                newName: "BookingDetails");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "BookingDetails",
                newName: "BookingDate");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "BookingDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<decimal>(
                name: "FinalAmount",
                table: "BookingDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "BookingDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingDetails",
                table: "BookingDetails",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BookedPlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDetailsId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookedPlace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookedPlace_BookingDetails_BookingDetailsId",
                        column: x => x.BookingDetailsId,
                        principalTable: "BookingDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookedPlace_BookingDetailsId",
                table: "BookedPlace",
                column: "BookingDetailsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookedPlace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingDetails",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "FinalAmount",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "BookingDetails");

            migrationBuilder.RenameTable(
                name: "BookingDetails",
                newName: "BookingsDetails");

            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "BookingsDetails",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "BookingsDetails",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "BookingsDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingsDetails",
                table: "BookingsDetails",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BookingItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDetailsId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CountryId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlaceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingItems_BookingsDetails_BookingDetailsId",
                        column: x => x.BookingDetailsId,
                        principalTable: "BookingsDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingItems_BookingDetailsId",
                table: "BookingItems",
                column: "BookingDetailsId");
        }
    }
}

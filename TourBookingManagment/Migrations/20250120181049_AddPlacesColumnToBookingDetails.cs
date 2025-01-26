using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourBookingManagment.Migrations
{
    /// <inheritdoc />
    public partial class AddPlacesColumnToBookingDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookedPlace");

            migrationBuilder.DropColumn(
                name: "DateOfTravel",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "FinalAmount",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "NumberOfPeople",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "Tax",
                table: "BookingDetails");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "BookingDetails");

            migrationBuilder.AddColumn<string>(
                name: "Places",
                table: "BookingDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Places",
                table: "BookingDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfTravel",
                table: "BookingDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "BookingDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "FinalAmount",
                table: "BookingDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BookingDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPeople",
                table: "BookingDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "BookingDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "BookingDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "BookingDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BookedPlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDetailsId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CountryId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlaceId = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
    }
}

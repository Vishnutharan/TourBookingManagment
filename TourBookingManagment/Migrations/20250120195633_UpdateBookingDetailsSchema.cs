using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourBookingManagment.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingDetailsSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "BookingDetails",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfTravel",
                table: "BookingDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "BookingDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FinalAmount",
                table: "BookingDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BookingDetails",
                type: "nvarchar(max)",
                nullable: true);

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
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "BookingDetails",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "BookingDetails",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "BookingDetails",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}

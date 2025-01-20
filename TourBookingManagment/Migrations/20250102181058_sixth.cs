using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourBookingManagment.Migrations
{
    /// <inheritdoc />
    public partial class sixth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_BookingsDetails_BookingDetailsId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "BookingItems");

            migrationBuilder.RenameIndex(
                name: "IX_Items_BookingDetailsId",
                table: "BookingItems",
                newName: "IX_BookingItems_BookingDetailsId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BookingsDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BookingsDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "BookingsDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "BookingsDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingItems",
                table: "BookingItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingItems_BookingsDetails_BookingDetailsId",
                table: "BookingItems",
                column: "BookingDetailsId",
                principalTable: "BookingsDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingItems_BookingsDetails_BookingDetailsId",
                table: "BookingItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingItems",
                table: "BookingItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BookingsDetails");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BookingsDetails");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "BookingsDetails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookingsDetails");

            migrationBuilder.RenameTable(
                name: "BookingItems",
                newName: "Items");

            migrationBuilder.RenameIndex(
                name: "IX_BookingItems_BookingDetailsId",
                table: "Items",
                newName: "IX_Items_BookingDetailsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                table: "Items",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_BookingsDetails_BookingDetailsId",
                table: "Items",
                column: "BookingDetailsId",
                principalTable: "BookingsDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

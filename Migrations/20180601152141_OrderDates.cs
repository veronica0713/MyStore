using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyStore.Migrations
{
    public partial class OrderDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dates",
                table: "OrderItems",
                newName: "DatesTo");

            migrationBuilder.RenameColumn(
                name: "Dates",
                table: "CartItems",
                newName: "DatesTo");

            migrationBuilder.AddColumn<DateTime>(
                name: "DatesFrom",
                table: "OrderItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DatesFrom",
                table: "CartItems",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatesFrom",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "DatesFrom",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "DatesTo",
                table: "OrderItems",
                newName: "Dates");

            migrationBuilder.RenameColumn(
                name: "DatesTo",
                table: "CartItems",
                newName: "Dates");
        }
    }
}

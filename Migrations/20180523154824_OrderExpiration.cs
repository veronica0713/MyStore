using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MyStore.Migrations
{
    public partial class OrderExpiration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expiration",
                table: "Orders",
                newName: "ExpirationYear");

            migrationBuilder.AddColumn<string>(
                name: "ExpirationMonth",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpirationMonth",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ExpirationYear",
                table: "Orders",
                newName: "Expiration");
        }
    }
}

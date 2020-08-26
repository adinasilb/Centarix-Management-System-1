using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedVendorOrderDateToNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "RequestNotifications",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "RequestNotifications",
                nullable: true);

            //migrationBuilder.AddColumn<double>(
            //    name: "Sum",
            //    table: "Payments",
            //    nullable: false,
            //    defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "RequestNotifications");

            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "RequestNotifications");

            //migrationBuilder.DropColumn(
            //    name: "Sum",
            //    table: "Payments");
        }
    }
}

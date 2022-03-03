using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddProductOrderTypeDiscriminator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Products",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasEnd",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Occurances",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimePeriodAmount",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimePeriodID",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_TimePeriodID",
                table: "Products",
                column: "TimePeriodID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TimePeriods_TimePeriodID",
                table: "Products",
                column: "TimePeriodID",
                principalTable: "TimePeriods",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("UPDATE Products SET Discriminator='" + AppData.AppUtility.OrderType.SingleOrder.ToString() + "' WHERE OrderTypeID =1");
            migrationBuilder.Sql("UPDATE Products SET Discriminator='" + AppData.AppUtility.OrderType.RecurringOrder.ToString() + "' WHERE OrderTypeID =2");
            migrationBuilder.Sql("UPDATE Products SET Discriminator='" + AppData.AppUtility.OrderType.StandingOrder.ToString() + "' WHERE OrderTypeID =3");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Products SET OrderTypeID= 1 WHERE Discriminator ='" + AppData.AppUtility.OrderType.SingleOrder.ToString() + "'");
            migrationBuilder.Sql("UPDATE Products SET OrderTypeID= 2 WHERE Discriminator ='" + AppData.AppUtility.OrderType.RecurringOrder.ToString() + "'");
            migrationBuilder.Sql("UPDATE Products SET OrderTypeID= 3 WHERE Discriminator ='" + AppData.AppUtility.OrderType.StandingOrder.ToString() + "'");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_TimePeriods_TimePeriodID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_TimePeriodID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "HasEnd",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Occurances",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TimePeriodAmount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TimePeriodID",
                table: "Products");
        }
    }
}

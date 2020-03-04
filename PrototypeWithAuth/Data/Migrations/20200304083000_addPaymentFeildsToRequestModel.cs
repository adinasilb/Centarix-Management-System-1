using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addPaymentFeildsToRequestModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Requests",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "Payed",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Payments",
                table: "Requests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Payed",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Payments",
                table: "Requests");
        }
    }
}

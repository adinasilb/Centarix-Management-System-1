using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class updateParentRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Payed",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "WithOrder",
                table: "Requests");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "ParentRequests",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "Payed",
                table: "ParentRequests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WithOrder",
                table: "ParentRequests",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "ParentRequests");

            migrationBuilder.DropColumn(
                name: "Payed",
                table: "ParentRequests");

            migrationBuilder.DropColumn(
                name: "WithOrder",
                table: "ParentRequests");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Requests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "Payed",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WithOrder",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

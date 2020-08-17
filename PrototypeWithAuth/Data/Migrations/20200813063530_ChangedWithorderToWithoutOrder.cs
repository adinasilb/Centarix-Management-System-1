using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangedWithorderToWithoutOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WithOrder",
                table: "ParentRequests");

            migrationBuilder.AddColumn<bool>(
                name: "WithOutOrder",
                table: "ParentRequests",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WithOutOrder",
                table: "ParentRequests");

            migrationBuilder.AddColumn<bool>(
                name: "WithOrder",
                table: "ParentRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

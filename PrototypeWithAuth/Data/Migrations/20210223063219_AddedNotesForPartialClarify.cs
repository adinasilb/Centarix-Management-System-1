using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedNotesForPartialClarify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoteForClarifyDelivery",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NoteForPartialDelivery",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupplierOrderNumber",
                table: "ParentRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoteForClarifyDelivery",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "NoteForPartialDelivery",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SupplierOrderNumber",
                table: "ParentRequests");
        }
    }
}

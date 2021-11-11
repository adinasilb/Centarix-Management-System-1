using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MoveNoteToSupplierToParentRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
              name: "NoteToSupplier",
              table: "ParentRequests",
              nullable: true);

  
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropColumn(
            name: "NoteToSupplier",
            table: "ParentRequests");

        }
    }
}

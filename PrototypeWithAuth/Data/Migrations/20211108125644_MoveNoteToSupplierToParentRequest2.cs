using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MoveNoteToSupplierToParentRequest2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("UPDATE ParentRequests set NoteToSupplier = r.NoteToSupplier from ParentRequests p join Requests r on r.ParentRequestID = p.ParentRequestID");

            migrationBuilder.DropColumn(
                name: "NoteToSupplier",
                table: "Requests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
               name: "NoteToSupplier",
               table: "Requests",
               type: "nvarchar(max)",
               nullable: true);

            migrationBuilder.Sql("UPDATE Requests set NoteToSupplier = r.NoteToSupplier from Requests p join ParentRequests r on r.ParentRequestID = p.ParentRequestID");

       

        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddFkRequestBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RequestComments_ObjectID",
                table: "RequestComments",
                column: "ObjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComments_Requests_ObjectID",
                table: "RequestComments",
                column: "ObjectID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestComments_Requests_ObjectID",
                table: "RequestComments");

            migrationBuilder.DropIndex(
                name: "IX_RequestComments_ObjectID",
                table: "RequestComments");
        }
    }
}

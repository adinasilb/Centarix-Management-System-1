using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RemoveFkRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestComments_Requests_RequestID",
                table: "RequestComments");

            migrationBuilder.DropIndex(
                name: "IX_RequestComments_RequestID",
                table: "RequestComments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RequestComments_RequestID",
                table: "RequestComments",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComments_Requests_RequestID",
                table: "RequestComments",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

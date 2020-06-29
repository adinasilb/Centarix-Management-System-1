using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addPoductTypeToRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Requests_ProductID",
                table: "Requests",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Products_ProductID",
                table: "Requests",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Products_ProductID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_ProductID",
                table: "Requests");
        }
    }
}

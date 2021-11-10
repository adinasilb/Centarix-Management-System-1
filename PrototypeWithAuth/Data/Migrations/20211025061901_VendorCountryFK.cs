using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class VendorCountryFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryID",
                table: "Vendors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_CountryID",
                table: "Vendors",
                column: "CountryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Countries_CountryID",
                table: "Vendors",
                column: "CountryID",
                principalTable: "Countries",
                principalColumn: "CountryID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Countries_CountryID",
                table: "Vendors");

            migrationBuilder.DropIndex(
                name: "IX_Vendors_CountryID",
                table: "Vendors");

            migrationBuilder.DropColumn(
                name: "CountryID",
                table: "Vendors");
        }
    }
}

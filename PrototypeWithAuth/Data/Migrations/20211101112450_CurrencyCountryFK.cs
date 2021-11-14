using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class CurrencyCountryFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyID",
                table: "Countries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CurrencyID",
                table: "Countries",
                column: "CurrencyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Countries_Currencies_CurrencyID",
                table: "Countries",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "CurrencyID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Countries_Currencies_CurrencyID",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_CurrencyID",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CurrencyID",
                table: "Countries");
        }
    }
}

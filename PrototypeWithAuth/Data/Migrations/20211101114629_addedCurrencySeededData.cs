using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedCurrencySeededData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "CurrencyID", "CurrencyName" },
                values: new object[] { -1, "None" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "CurrencyID", "CurrencyName" },
                values: new object[] { 1, "USD" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "CurrencyID", "CurrencyName" },
                values: new object[] { 2, "NIS" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "CurrencyID",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "CurrencyID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "CurrencyID",
                keyValue: 2);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddCompanyAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CompanyAccounts",
                columns: new[] { "CompanyAccountID", "CompanyBankName", "CompanyBankNum", "CompanyBranchNum" },
                values: new object[,]
                {
                    { 1, "Discount", null, null },
                    { 2, "Mercantile", null, null },
                    { 3, "Leumi", null, null },
                    { 4, "Payoneer", null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompanyAccounts",
                keyColumn: "CompanyAccountID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CompanyAccounts",
                keyColumn: "CompanyAccountID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CompanyAccounts",
                keyColumn: "CompanyAccountID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CompanyAccounts",
                keyColumn: "CompanyAccountID",
                keyValue: 4);
        }
    }
}

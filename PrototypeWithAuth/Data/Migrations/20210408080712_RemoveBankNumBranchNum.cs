using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RemoveBankNumBranchNum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyBankNum",
                table: "CompanyAccounts");

            migrationBuilder.DropColumn(
                name: "CompanyBranchNum",
                table: "CompanyAccounts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyBankNum",
                table: "CompanyAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyBranchNum",
                table: "CompanyAccounts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

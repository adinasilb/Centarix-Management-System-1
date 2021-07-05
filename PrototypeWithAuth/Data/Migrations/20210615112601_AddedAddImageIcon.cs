using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedAddImageIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 1,
                column: "Icon",
                value: "icon-account_box-24px1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 1,
                column: "Icon",
                value: "");
        }
    }
}

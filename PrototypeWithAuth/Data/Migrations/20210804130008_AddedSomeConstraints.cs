using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedSomeConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "AmountWithInLocation",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "AmountWithOutLocation",
                table: "Requests");


            migrationBuilder.AlterColumn<long>(
                name: "OrderNumber",
                table: "ParentRequests",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuartzyOrderNumber",
                table: "ParentRequests",
                nullable: true);

            migrationBuilder.InsertData(
                table: "CompanyAccounts",
                columns: new[] { "CompanyAccountID", "CompanyBankName" },
                values: new object[] { 5, "Quartzy Bank" });


            migrationBuilder.CreateIndex(
                name: "IX_Products_SerialNumber_VendorID",
                table: "Products",
                columns: new[] { "SerialNumber", "VendorID" },
                unique: true,
                filter: "[SerialNumber] IS NOT NULL AND [VendorID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ParentRequests_QuartzyOrderNumber",
                table: "ParentRequests",
                column: "QuartzyOrderNumber",
                unique: true,
                filter: "[QuartzyOrderNumber] IS NOT NULL");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropIndex(
                name: "IX_Products_SerialNumber_VendorID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ParentRequests_QuartzyOrderNumber",
                table: "ParentRequests");

            migrationBuilder.DeleteData(
                table: "CompanyAccounts",
                keyColumn: "CompanyAccountID",
                keyValue: 5);

           

            migrationBuilder.DropColumn(
                name: "QuartzyOrderNumber",
                table: "ParentRequests");

            migrationBuilder.AddColumn<long>(
                name: "AmountWithInLocation",
                table: "Requests",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AmountWithOutLocation",
                table: "Requests",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderNumber",
                table: "ParentRequests",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

         
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MoveUnitToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_UnitTypes_SubSubUnitTypeID",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_UnitTypes_SubUnitTypeID",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_UnitTypes_UnitTypeID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_SubSubUnitTypeID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_SubUnitTypeID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_UnitTypeID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SubSubUnit",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SubSubUnitTypeID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SubUnit",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SubUnitTypeID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "UnitTypeID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Handeling",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "QuantityPerUnit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ReorderLevel",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitsInOrder",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitsInStock",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Products",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SubSubUnit",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubSubUnitTypeID",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SubUnit",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubUnitTypeID",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitTypeID",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SerialNumber",
                table: "Products",
                column: "SerialNumber",
                unique: true,
                filter: "[SerialNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubSubUnitTypeID",
                table: "Products",
                column: "SubSubUnitTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubUnitTypeID",
                table: "Products",
                column: "SubUnitTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitTypeID",
                table: "Products",
                column: "UnitTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ParentRequests_OrderNumber",
                table: "ParentRequests",
                column: "OrderNumber",
                unique: true,
                filter: "[OrderNumber] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitTypes_SubSubUnitTypeID",
                table: "Products",
                column: "SubSubUnitTypeID",
                principalTable: "UnitTypes",
                principalColumn: "UnitTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitTypes_SubUnitTypeID",
                table: "Products",
                column: "SubUnitTypeID",
                principalTable: "UnitTypes",
                principalColumn: "UnitTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitTypes_UnitTypeID",
                table: "Products",
                column: "UnitTypeID",
                principalTable: "UnitTypes",
                principalColumn: "UnitTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitTypes_SubSubUnitTypeID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitTypes_SubUnitTypeID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitTypes_UnitTypeID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SerialNumber",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubSubUnitTypeID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubUnitTypeID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UnitTypeID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ParentRequests_OrderNumber",
                table: "ParentRequests");

            migrationBuilder.DropColumn(
                name: "SubSubUnit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubSubUnitTypeID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubUnit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SubUnitTypeID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "UnitTypeID",
                table: "Products");

            migrationBuilder.AddColumn<long>(
                name: "SubSubUnit",
                table: "Requests",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubSubUnitTypeID",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubUnit",
                table: "Requests",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubUnitTypeID",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitTypeID",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Handeling",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityPerUnit",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReorderLevel",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitsInOrder",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitsInStock",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SubSubUnitTypeID",
                table: "Requests",
                column: "SubSubUnitTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SubUnitTypeID",
                table: "Requests",
                column: "SubUnitTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UnitTypeID",
                table: "Requests",
                column: "UnitTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_UnitTypes_SubSubUnitTypeID",
                table: "Requests",
                column: "SubSubUnitTypeID",
                principalTable: "UnitTypes",
                principalColumn: "UnitTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_UnitTypes_SubUnitTypeID",
                table: "Requests",
                column: "SubUnitTypeID",
                principalTable: "UnitTypes",
                principalColumn: "UnitTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_UnitTypes_UnitTypeID",
                table: "Requests",
                column: "UnitTypeID",
                principalTable: "UnitTypes",
                principalColumn: "UnitTypeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

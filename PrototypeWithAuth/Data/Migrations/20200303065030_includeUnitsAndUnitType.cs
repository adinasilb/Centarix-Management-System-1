using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class includeUnitsAndUnitType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubSubUnitTypeID",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubSubunit",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubUnit",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubUnitTypeID",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Unit",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitTypeID",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitsInStock",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnitsOrdered",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UnitTypes",
                columns: table => new
                {
                    UnitTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitTypeDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitTypes", x => x.UnitTypeID);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "UnitTypes");

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
                name: "SubSubUnitTypeID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SubSubunit",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SubUnit",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SubUnitTypeID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "UnitTypeID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "UnitsInStock",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "UnitsOrdered",
                table: "Requests");
        }
    }
}

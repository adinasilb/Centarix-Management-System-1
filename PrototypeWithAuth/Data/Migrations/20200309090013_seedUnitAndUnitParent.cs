using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seedUnitAndUnitParent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UnitParentTypes",
                columns: new[] { "UnitParentTypeID", "UnitParentTypeDescription" },
                values: new object[] { 1, "Units" });

            migrationBuilder.InsertData(
                table: "UnitParentTypes",
                columns: new[] { "UnitParentTypeID", "UnitParentTypeDescription" },
                values: new object[] { 2, "Weight/Volume" });

            migrationBuilder.InsertData(
                table: "UnitParentTypes",
                columns: new[] { "UnitParentTypeID", "UnitParentTypeDescription" },
                values: new object[] { 3, "Test" });

            migrationBuilder.InsertData(
                table: "UnitTypes",
                columns: new[] { "UnitTypeID", "UnitParentTypeID", "UnitTypeDescription" },
                values: new object[,]
                {
                    { 1, 1, "Bottle" },
                    { 16, 3, "test" },
                    { 15, 3, "rxhs" },
                    { 14, 2, "gal" },
                    { 13, 2, "�l" },
                    { 12, 2, "ml" },
                    { 11, 2, "Liter" },
                    { 10, 2, "�g" },
                    { 9, 2, "mg" },
                    { 8, 2, "gr" },
                    { 7, 2, "Kg" },
                    { 6, 1, "Vial" },
                    { 5, 1, "Unit" },
                    { 4, 1, "Bag" },
                    { 3, 1, "Pack" },
                    { 2, 1, "Box" },
                    { 17, 3, "preps" },
                    { 18, 3, "assays" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "UnitTypes",
                keyColumn: "UnitTypeID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "UnitParentTypes",
                keyColumn: "UnitParentTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UnitParentTypes",
                keyColumn: "UnitParentTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UnitParentTypes",
                keyColumn: "UnitParentTypeID",
                keyValue: 3);
        }
    }
}

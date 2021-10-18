using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RemoveSubUnitDecimalPlaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SubUnit",
                table: "Products",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SubSubUnit",
                table: "Products",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SubUnit",
                table: "Products",
                type: "decimal(18,10)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SubSubUnit",
                table: "Products",
                type: "decimal(18,10)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}

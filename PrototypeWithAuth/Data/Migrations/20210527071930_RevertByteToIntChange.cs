using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RevertByteToIntChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Warranty",
                table: "Requests",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "ExpectedSupplyDays",
                table: "Requests",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Warranty",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(byte),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExpectedSupplyDays",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(byte),
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ItemSerialNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence<int>(
                name: "SerialNumberHelper",
                schema: "dbo");

            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "Requests",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR dbo.SerialNumberHelper");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_SerialNumber",
                table: "Requests",
                column: "SerialNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Requests_SerialNumber",
                table: "Requests");

            migrationBuilder.DropSequence(
                name: "SerialNumberHelper",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Requests");
        }
    }
}

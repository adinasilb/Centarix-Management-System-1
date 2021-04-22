using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangedProtocolCategoryDesciption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProtocolDescription",
                table: "ProtocolCategories");

            migrationBuilder.AddColumn<string>(
                name: "ProtocolCategoryDescription",
                table: "ProtocolCategories",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProtocolCategories",
                keyColumn: "ProtocolCategoryTypeID",
                keyValue: 1,
                column: "ProtocolCategoryDescription",
                value: "Rejuvenation");

            migrationBuilder.UpdateData(
                table: "ProtocolCategories",
                keyColumn: "ProtocolCategoryTypeID",
                keyValue: 2,
                column: "ProtocolCategoryDescription",
                value: "Biomarkers");

            migrationBuilder.UpdateData(
                table: "ProtocolCategories",
                keyColumn: "ProtocolCategoryTypeID",
                keyValue: 3,
                column: "ProtocolCategoryDescription",
                value: "Delivery Systems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProtocolCategoryDescription",
                table: "ProtocolCategories");

            migrationBuilder.AddColumn<string>(
                name: "ProtocolDescription",
                table: "ProtocolCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ProtocolCategories",
                keyColumn: "ProtocolCategoryTypeID",
                keyValue: 1,
                column: "ProtocolDescription",
                value: "Rejuvenation");

            migrationBuilder.UpdateData(
                table: "ProtocolCategories",
                keyColumn: "ProtocolCategoryTypeID",
                keyValue: 2,
                column: "ProtocolDescription",
                value: "Biomarkers");

            migrationBuilder.UpdateData(
                table: "ProtocolCategories",
                keyColumn: "ProtocolCategoryTypeID",
                keyValue: 3,
                column: "ProtocolDescription",
                value: "Delivery Systems");
        }
    }
}

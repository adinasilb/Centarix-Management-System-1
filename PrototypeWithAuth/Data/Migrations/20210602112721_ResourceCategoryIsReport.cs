using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ResourceCategoryIsReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "IsReportsCategory",
            //    table: "ResourceCategories",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 1,
            //    column: "IsReportsCategory",
            //    value: true);

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 2,
            //    column: "IsReportsCategory",
            //    value: true);

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 3,
            //    column: "IsReportsCategory",
            //    value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "IsReportsCategory",
            //    table: "ResourceCategories");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class UpdateFunctionIcons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "CategoryJson",
            //    table: "ResourceCategories",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "CategoryJsonID",
            //    table: "ResourceCategories",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "AdditionalFieldsJson",
            //    table: "Requests",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "CategoryJson",
            //    table: "ProductSubcategories",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "CategoryJsonID",
            //    table: "ProductSubcategories",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "CategoryJson",
            //    table: "ParentCategories",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "CategoryJsonID",
            //    table: "ParentCategories",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 1,
                column: "Icon",
                value: "icon-account_box-24px-1");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 6,
                column: "Icon",
                value: "icon-table_chart-24px-1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "CategoryJson",
            //    table: "ResourceCategories");

            //migrationBuilder.DropColumn(
            //    name: "CategoryJsonID",
            //    table: "ResourceCategories");

            //migrationBuilder.DropColumn(
            //    name: "AdditionalFieldsJson",
            //    table: "Requests");

            //migrationBuilder.DropColumn(
            //    name: "CategoryJson",
            //    table: "ProductSubcategories");

            //migrationBuilder.DropColumn(
            //    name: "CategoryJsonID",
            //    table: "ProductSubcategories");

            //migrationBuilder.DropColumn(
            //    name: "CategoryJson",
            //    table: "ParentCategories");

            //migrationBuilder.DropColumn(
            //    name: "CategoryJsonID",
            //    table: "ParentCategories");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 1,
                column: "Icon",
                value: "icon-account_box-24px1");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 6,
                column: "Icon",
                value: "icon-table_chart-24px1");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangedResourceCategoryToExtendCategoryBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                             name: "ResourceCategoryID",
                             table: "ResourceCategories",
                             newName: "ID");


            migrationBuilder.RenameColumn(
                            name: "ResourceCategoryDescription",
                            table: "ResourceCategories",
                            newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                             name: "ID",
                             table: "ResourceCategories",
                             newName: "ResourceCategoryID");


            migrationBuilder.RenameColumn(
                            name: "Description",
                            table: "ResourceCategories",
                            newName: "ResourceCategoryDescription");
        }
    }
}

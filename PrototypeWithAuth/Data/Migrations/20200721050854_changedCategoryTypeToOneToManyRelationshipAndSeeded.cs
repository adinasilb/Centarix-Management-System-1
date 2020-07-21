using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class changedCategoryTypeToOneToManyRelationshipAndSeeded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTypeParentCategory");

            migrationBuilder.AddColumn<int>(
                name: "CategoryTypeID",
                table: "ParentCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "CategoryTypes",
                columns: new[] { "CategoryTypeID", "CategoryTypeDescription" },
                values: new object[] { 1, "Lab" });

            migrationBuilder.InsertData(
                table: "CategoryTypes",
                columns: new[] { "CategoryTypeID", "CategoryTypeDescription" },
                values: new object[] { 2, "Operational" });

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 1,
                column: "CategoryTypeID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 2,
                column: "CategoryTypeID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 3,
                column: "CategoryTypeID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 4,
                column: "CategoryTypeID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 5,
                column: "CategoryTypeID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 6,
                column: "CategoryTypeID",
                value: 2);

            migrationBuilder.CreateIndex(
                name: "IX_ParentCategories_CategoryTypeID",
                table: "ParentCategories",
                column: "CategoryTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_ParentCategories_CategoryTypes_CategoryTypeID",
                table: "ParentCategories",
                column: "CategoryTypeID",
                principalTable: "CategoryTypes",
                principalColumn: "CategoryTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParentCategories_CategoryTypes_CategoryTypeID",
                table: "ParentCategories");

            migrationBuilder.DropIndex(
                name: "IX_ParentCategories_CategoryTypeID",
                table: "ParentCategories");

            migrationBuilder.DeleteData(
                table: "CategoryTypes",
                keyColumn: "CategoryTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CategoryTypes",
                keyColumn: "CategoryTypeID",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "CategoryTypeID",
                table: "ParentCategories");

            migrationBuilder.CreateTable(
                name: "CategoryTypeParentCategory",
                columns: table => new
                {
                    CategoryTypeID = table.Column<int>(type: "int", nullable: false),
                    ParentCategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTypeParentCategory", x => new { x.CategoryTypeID, x.ParentCategoryID });
                    table.ForeignKey(
                        name: "FK_CategoryTypeParentCategory_CategoryTypes_CategoryTypeID",
                        column: x => x.CategoryTypeID,
                        principalTable: "CategoryTypes",
                        principalColumn: "CategoryTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CategoryTypeParentCategory_ParentCategories_ParentCategoryID",
                        column: x => x.ParentCategoryID,
                        principalTable: "ParentCategories",
                        principalColumn: "ParentCategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryTypeParentCategory_ParentCategoryID",
                table: "CategoryTypeParentCategory",
                column: "ParentCategoryID");
        }
    }
}

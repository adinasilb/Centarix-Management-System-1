using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MMCategoryTypeVendor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorCategoryType",
                columns: table => new
                {
                    VendorID = table.Column<int>(nullable: false),
                    CategoryTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorCategoryType", x => new { x.VendorID, x.CategoryTypeID });
                    table.ForeignKey(
                        name: "FK_VendorCategoryType_CategoryTypes_CategoryTypeID",
                        column: x => x.CategoryTypeID,
                        principalTable: "CategoryTypes",
                        principalColumn: "CategoryTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VendorCategoryType_Vendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "VendorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorCategoryType_CategoryTypeID",
                table: "VendorCategoryType",
                column: "CategoryTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorCategoryType");
        }
    }
}

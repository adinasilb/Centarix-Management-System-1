using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RenameVendorIDInComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorComments_Vendors_VendorID",
                table: "VendorComments");

            migrationBuilder.DropIndex(
                name: "IX_VendorComments_VendorID",
                table: "VendorComments");

            migrationBuilder.RenameColumn(
                name: "VendorID",
                table: "VendorComments",
                newName: "ObjectID");

            migrationBuilder.CreateIndex(
                name: "IX_VendorComments_ObjectID",
                table: "VendorComments",
                column: "ObjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorComments_Vendors_ObjectID",
                table: "VendorComments",
                column: "ObjectID",
                principalTable: "Vendors",
                principalColumn: "VendorID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorComments_Vendors_ObjectID",
                table: "VendorComments");

            migrationBuilder.DropIndex(
                name: "IX_VendorComments_ObjectID",
                table: "VendorComments");

            migrationBuilder.DropColumn(
                name: "ObjectID",
                table: "VendorComments");

            migrationBuilder.AddColumn<int>(
                name: "VendorID",
                table: "VendorComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VendorComments_VendorID",
                table: "VendorComments",
                column: "VendorID");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorComments_Vendors_VendorID",
                table: "VendorComments",
                column: "VendorID",
                principalTable: "Vendors",
                principalColumn: "VendorID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

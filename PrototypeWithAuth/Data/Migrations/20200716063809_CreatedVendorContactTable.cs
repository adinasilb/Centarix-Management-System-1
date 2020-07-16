using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class CreatedVendorContactTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VendorContacts",
                columns: table => new
                {
                    VendorContactID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorContactName = table.Column<string>(nullable: true),
                    VendorContactEmail = table.Column<string>(nullable: true),
                    VendorContactPhone = table.Column<string>(nullable: true),
                    VendorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorContacts", x => x.VendorContactID);
                    table.ForeignKey(
                        name: "FK_VendorContacts_Vendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "VendorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorContacts_VendorID",
                table: "VendorContacts",
                column: "VendorID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VendorContacts");
        }
    }
}

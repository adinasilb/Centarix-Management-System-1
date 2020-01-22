using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class VendorAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorEnName = table.Column<string>(maxLength: 50, nullable: false),
                    VendorHeName = table.Column<string>(maxLength: 50, nullable: true),
                    VendorBuisnessID = table.Column<string>(maxLength: 9, nullable: false),
                    ContactPerson = table.Column<string>(maxLength: 50, nullable: true),
                    ContactEmail = table.Column<string>(maxLength: 50, nullable: true),
                    OrderEmail = table.Column<string>(maxLength: 50, nullable: true),
                    VendorContactPhone1 = table.Column<string>(nullable: true),
                    VendorContactPhone2 = table.Column<string>(nullable: true),
                    VendorFax = table.Column<string>(nullable: true),
                    VendorCity = table.Column<string>(maxLength: 50, nullable: true),
                    VendorStreet = table.Column<string>(maxLength: 50, nullable: true),
                    VendorZip = table.Column<string>(nullable: true),
                    VendorWebsite = table.Column<string>(nullable: true),
                    VendorBank = table.Column<string>(maxLength: 50, nullable: false),
                    VendorBankBranch = table.Column<string>(maxLength: 4, nullable: false),
                    VendorAccountNum = table.Column<string>(nullable: false),
                    VendorSwift = table.Column<string>(nullable: true),
                    VendorBIC = table.Column<string>(nullable: true),
                    VendorGoldAccount = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vendors");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class migrateOldVendorCountries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO OldVendorCountries SELECT VendorID, VendorCountry FROM Vendors WHERE VendorCountry is not null");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE vendors SET VendorCountry = ovc.oldvendorcountryname FROM vendors v INNER JOIN oldvendorcountries ovc ON v.VendorID = ovc.vendorid");
        }
    }
}

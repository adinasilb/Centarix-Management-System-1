using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class SQLAddVendorCountryAsIsrael : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Vendors SET CountryID = 23 Where CountryID is null");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //be careful before doing this!!
            //migrationBuilder.Sql("UPDATE Vendors SET CountryID = null");
        }
    }
}

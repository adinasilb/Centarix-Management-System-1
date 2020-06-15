using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class updateSeededPlasticsToTripleDigits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE ProductSubcategories SET ")
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

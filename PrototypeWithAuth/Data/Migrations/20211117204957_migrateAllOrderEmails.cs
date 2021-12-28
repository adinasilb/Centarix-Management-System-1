using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class migrateAllOrderEmails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE VENDORS Set QuotesEmail = OrdersEmail Where QuotesEmail IS NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //don't need a down bc the other migration will take it away
        }
    }
}

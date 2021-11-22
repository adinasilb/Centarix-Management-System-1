using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MoveDiscountDetailsOver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("update ParentQuotes from ParentQuotes pq inner join Requests r on pq.ParentQuoteID = r.ParentQuoteID");
            //migrationBuilder.Sql("UPDATE ParentQuotes SET Discount = (SELECT Discount FROM Requests r where ParentQuotes.ParentQuoteID = r.ParentQuoteID)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE ParentQuotes SET Discount = '0.00'");

        }
    }
}

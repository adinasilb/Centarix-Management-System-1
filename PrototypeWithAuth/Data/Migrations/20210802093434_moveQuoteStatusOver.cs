using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class moveQuoteStatusOver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "update requests set quotestatusid = 1 where requestid in " +
                "(select requestid from requests as r  inner join ParentQuotes as pq on r.ParentQuoteID = pq.ParentQuoteID where pq.QuoteStatusID = 1)" +
                "update requests set quotestatusid = 2 where requestid in " +
                "(select requestid from requests as r  inner join ParentQuotes as pq on r.ParentQuoteID = pq.ParentQuoteID where pq.QuoteStatusID = 2)" +
                "update requests set quotestatusid = 3 where requestid in " +
                "(select requestid from requests as r  inner join ParentQuotes as pq on r.ParentQuoteID = pq.ParentQuoteID where pq.QuoteStatusID = 3)" +
                "update requests set quotestatusid = 4 where requestid in " +
                "(select requestid from requests as r  inner join ParentQuotes as pq on r.ParentQuoteID = pq.ParentQuoteID where pq.QuoteStatusID = 4)" +
                "update parentquotes set QuoteStatusID = null"
            );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

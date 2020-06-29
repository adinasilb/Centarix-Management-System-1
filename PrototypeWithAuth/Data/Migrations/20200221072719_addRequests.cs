using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    RequestID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(nullable: false),
                    LocationID = table.Column<int>(nullable: true),
                    AmountWithInLocation = table.Column<int>(nullable: true),
                    AmountWithOutLocation = table.Column<int>(nullable: true),
                    ApplicationUserID = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    WithOrder = table.Column<bool>(nullable: false),
                    InvoiceNumber = table.Column<string>(nullable: true),
                    CatalogNumber = table.Column<string>(nullable: true),
                    SerialNumber = table.Column<string>(nullable: true),
                    URL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.RequestID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}

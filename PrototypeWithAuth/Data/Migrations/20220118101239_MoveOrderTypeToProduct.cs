using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MoveOrderTypeToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<int>(
                name: "OrderTypeID",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            

            migrationBuilder.Sql("update Products set Products.OrderTypeID = r.OrderTypeID from Products p inner join Requests r on p.ProductID = r.ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderTypeID",
                table: "Products",
                column: "OrderTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OrderTypes_OrderTypeID",
                table: "Products",
                column: "OrderTypeID",
                principalTable: "OrderTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_OrderTypes_OrderTypeID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_OrderTypeID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "OrderTypeID",
                table: "Requests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
               name: "OrderTypeID",
               table: "Requests",
               type: "int",
               nullable: false,
               defaultValue: 0);

            

            migrationBuilder.Sql("update Requests set Requests.OrderTypeID = p.OrderTypeID from Requests r inner join Products p on r.ProductID = p.ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_OrderTypeID",
                table: "Requests",
                column: "OrderTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_OrderTypes_OrderTypeID",
                table: "Requests",
                column: "OrderTypeID",
                principalTable: "OrderTypes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropForeignKey(
                    name: "FK_Products_OrderTypes_OrderTypeID",
                    table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderTypeID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderTypeID",
                table: "Products");
        }
           
    }
}

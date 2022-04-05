using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MoveOrderTypeStringToForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<int>(
                name: "OrderMethodID",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "OrderMethods",
                columns: new[] { "ID", "Description", "DescriptionEnum" },
                values: new object[,]
                {
                    { -1, "None", "None" },
                    { 1, "Add To Cart", "AddToCart" },
                    { 2, "Order Now", "OrderNow" },
                    { 3, "Already Purchased", "AlreadyPurchased" },
                    { 4, "Request Price Quote", "RequestPriceQuote" },
                    { 5, "Save", "Save" },
                    { 6, "Excel Upload", "ExcelUpload" }
                });

            migrationBuilder.Sql("UPDATE Requests SET OrderMethodID= 1 WHERE OrderType ='" + AppData.AppUtility.OrderMethod.AddToCart.ToString() + "'");
            migrationBuilder.Sql("UPDATE Requests SET OrderMethodID= 2 WHERE OrderType ='" + AppData.AppUtility.OrderMethod.OrderNow.ToString() + "'");
            migrationBuilder.Sql("UPDATE Requests SET OrderMethodID= 3 WHERE OrderType ='" + AppData.AppUtility.OrderMethod.AlreadyPurchased.ToString() + "'");
            migrationBuilder.Sql("UPDATE Requests SET OrderMethodID= 4 WHERE OrderType ='" + AppData.AppUtility.OrderMethod.RequestPriceQuote.ToString() + "'");
            migrationBuilder.Sql("UPDATE Requests SET OrderMethodID= 5 WHERE OrderType ='" + AppData.AppUtility.OrderMethod.Save.ToString() + "'");
            migrationBuilder.Sql("UPDATE Requests SET OrderMethodID= 6 WHERE OrderType ='" + AppData.AppUtility.OrderMethod.ExcelUpload.ToString() + "'");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_OrderMethodID",
                table: "Requests",
                column: "OrderMethodID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_OrderMethods_OrderMethodID",
                table: "Requests",
                column: "OrderMethodID",
                principalTable: "OrderMethods",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.DropColumn(
                name: "OrderType",
                table: "Requests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "OrderType",
               table: "Requests",
               type: "nvarchar(max)",
               nullable: true);

            migrationBuilder.Sql("UPDATE Requests SET OrderType='" + AppData.AppUtility.OrderMethod.AddToCart.ToString() + "' WHERE OrderMethodID =1");
            migrationBuilder.Sql("UPDATE Requests SET OrderType='" + AppData.AppUtility.OrderMethod.OrderNow.ToString() + "' WHERE OrderMethodID =2");
            migrationBuilder.Sql("UPDATE Requests SET OrderType='" + AppData.AppUtility.OrderMethod.AlreadyPurchased.ToString() + "' WHERE OrderMethodID =3");
            migrationBuilder.Sql("UPDATE Requests SET OrderType='" + AppData.AppUtility.OrderMethod.RequestPriceQuote.ToString() + "' WHERE OrderMethodID =4");
            migrationBuilder.Sql("UPDATE Requests SET OrderType='" + AppData.AppUtility.OrderMethod.Save.ToString() + "' WHERE OrderMethodID =5");
            migrationBuilder.Sql("UPDATE Requests SET OrderType='" + AppData.AppUtility.OrderMethod.ExcelUpload.ToString() + "' WHERE OrderMethodID =6");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_OrderMethods_OrderMethodID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_OrderMethodID",
                table: "Requests");

            migrationBuilder.DeleteData(
                table: "OrderMethods",
                keyColumn: "ID",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "OrderMethods",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderMethods",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderMethods",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderMethods",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderMethods",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OrderMethods",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "OrderMethodID",
                table: "Requests");


        }
    }
}

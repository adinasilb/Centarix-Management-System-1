using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FillOrderTypesAddForeignKeyToRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderTypeID",
                table: "Requests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "OrderTypes",
                columns: new[] { "ID", "Description", "DescriptionEnum" },
                values: new object[] { 1, "Single", "Single" });

            migrationBuilder.InsertData(
                table: "OrderTypes",
                columns: new[] { "ID", "Description", "DescriptionEnum" },
                values: new object[] { 2, "Recurring", "Recurring" });

            migrationBuilder.InsertData(
                table: "OrderTypes",
                columns: new[] { "ID", "Description", "DescriptionEnum" },
                values: new object[] { 3, "Standing", "Standing" });

            migrationBuilder.Sql("UPDATE Requests SET OrderTypeID= 1");

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

            

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_OrderTypes_OrderTypeID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_OrderTypeID",
                table: "Requests");

            migrationBuilder.DeleteData(
                table: "OrderTypes",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderTypes",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderTypes",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "OrderTypeID",
                table: "Requests");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RemoveOrderTypeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_OrderTypes_OrderTypeID",
                table: "Products");

            migrationBuilder.DropTable(
                name: "OrderTypes");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderTypeID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderTypeID",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "OrderTypes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEnum = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTypes", x => x.ID);
                });

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

            migrationBuilder.AddColumn<int>(
                name: "OrderTypeID",
                table: "Products",
                type: "int",
                nullable: true,
                defaultValue: 0);

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
        }
    }
}

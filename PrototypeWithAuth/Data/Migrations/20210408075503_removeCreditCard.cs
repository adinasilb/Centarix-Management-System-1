using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class removeCreditCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CreditCard_CreditCardID",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "CreditCard");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CreditCardID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreditCardID",
                table: "Payments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditCardID",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CreditCard",
                columns: table => new
                {
                    CreditCardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    CompanyAccountID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCard", x => x.CreditCardID);
                    table.ForeignKey(
                        name: "FK_CreditCard_CompanyAccounts_CompanyAccountID",
                        column: x => x.CompanyAccountID,
                        principalTable: "CompanyAccounts",
                        principalColumn: "CompanyAccountID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CreditCard",
                columns: new[] { "CreditCardID", "CardNumber", "CompanyAccountID" },
                values: new object[,]
                {
                    { 1, "2543", 2 },
                    { 2, "4694", 2 },
                    { 3, "3485", 2 },
                    { 4, "0054", 2 },
                    { 5, "4971", 1 },
                    { 6, "4424", 1 },
                    { 7, "4432", 1 },
                    { 8, "7972", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreditCardID",
                table: "Payments",
                column: "CreditCardID");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCard_CompanyAccountID",
                table: "CreditCard",
                column: "CompanyAccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CreditCard_CreditCardID",
                table: "Payments",
                column: "CreditCardID",
                principalTable: "CreditCard",
                principalColumn: "CreditCardID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

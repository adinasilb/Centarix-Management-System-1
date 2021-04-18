using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangeCompanyAccountFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyAccounts_PaymentTypes_PaymentTypeID",
                table: "CompanyAccounts");

            migrationBuilder.DropIndex(
                name: "IX_CompanyAccounts_PaymentTypeID",
                table: "CompanyAccounts");

            migrationBuilder.DropColumn(
                name: "CompanyAccountNum",
                table: "CompanyAccounts");

            migrationBuilder.DropColumn(
                name: "PaymentTypeID",
                table: "CompanyAccounts");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyAccountID",
                table: "Payments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckNumber",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreditCardID",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeID",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CreditCard",
                columns: table => new
                {
                    CreditCardID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyAccountID = table.Column<int>(nullable: false),
                    CardNumber = table.Column<string>(maxLength: 4, nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreditCardID",
                table: "Payments",
                column: "CreditCardID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentTypeID",
                table: "Payments",
                column: "PaymentTypeID");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentTypes_PaymentTypeID",
                table: "Payments",
                column: "PaymentTypeID",
                principalTable: "PaymentTypes",
                principalColumn: "PaymentTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CreditCard_CreditCardID",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentTypes_PaymentTypeID",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "CreditCard");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CreditCardID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentTypeID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CheckNumber",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreditCardID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentTypeID",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyAccountID",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "CompanyAccountNum",
                table: "CompanyAccounts",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeID",
                table: "CompanyAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAccounts_PaymentTypeID",
                table: "CompanyAccounts",
                column: "PaymentTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyAccounts_PaymentTypes_PaymentTypeID",
                table: "CompanyAccounts",
                column: "PaymentTypeID",
                principalTable: "PaymentTypes",
                principalColumn: "PaymentTypeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

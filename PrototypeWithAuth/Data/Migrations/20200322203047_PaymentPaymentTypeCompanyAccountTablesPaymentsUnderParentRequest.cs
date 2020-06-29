using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class PaymentPaymentTypeCompanyAccountTablesPaymentsUnderParentRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Payments",
                table: "Requests");

            migrationBuilder.AddColumn<long>(
                name: "Installments",
                table: "ParentRequests",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    PaymentTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentTypeDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.PaymentTypeID);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAccounts",
                columns: table => new
                {
                    CompanyAccountID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyAccountNum = table.Column<string>(maxLength: 5, nullable: false),
                    CompanyBankNum = table.Column<string>(nullable: true),
                    CompanyBranchNum = table.Column<string>(nullable: true),
                    PaymentTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAccounts", x => x.CompanyAccountID);
                    table.ForeignKey(
                        name: "FK_CompanyAccounts_PaymentTypes_PaymentTypeID",
                        column: x => x.PaymentTypeID,
                        principalTable: "PaymentTypes",
                        principalColumn: "PaymentTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    Reference = table.Column<string>(nullable: true),
                    ParentRequestID = table.Column<int>(nullable: false),
                    CompanyAccountID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_Payments_CompanyAccounts_CompanyAccountID",
                        column: x => x.CompanyAccountID,
                        principalTable: "CompanyAccounts",
                        principalColumn: "CompanyAccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_ParentRequests_ParentRequestID",
                        column: x => x.ParentRequestID,
                        principalTable: "ParentRequests",
                        principalColumn: "ParentRequestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAccounts_PaymentTypeID",
                table: "CompanyAccounts",
                column: "PaymentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CompanyAccountID",
                table: "Payments",
                column: "CompanyAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ParentRequestID",
                table: "Payments",
                column: "ParentRequestID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "CompanyAccounts");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "Installments",
                table: "ParentRequests");

            migrationBuilder.AddColumn<long>(
                name: "Payments",
                table: "Requests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedPaymentStatusWithNullIDOnRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentStatusID",
                table: "Requests",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentStatuses",
                columns: table => new
                {
                    PaymentStatusID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentStatusDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatuses", x => x.PaymentStatusID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_PaymentStatusID",
                table: "Requests",
                column: "PaymentStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_PaymentStatuses_PaymentStatusID",
                table: "Requests",
                column: "PaymentStatusID",
                principalTable: "PaymentStatuses",
                principalColumn: "PaymentStatusID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_PaymentStatuses_PaymentStatusID",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "PaymentStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Requests_PaymentStatusID",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "PaymentStatusID",
                table: "Requests");
        }
    }
}

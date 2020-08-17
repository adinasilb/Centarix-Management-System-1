using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MovedPaymentRelatedInfoToRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_ParentRequests_ParentRequestID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ParentRequestID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ParentRequestID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Installments",
                table: "ParentRequests");

            migrationBuilder.DropColumn(
                name: "Payed",
                table: "ParentRequests");

            migrationBuilder.AddColumn<long>(
                name: "Installments",
                table: "Requests",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "Payed",
                table: "Requests",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RequestID",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RequestID",
                table: "Payments",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Requests_RequestID",
                table: "Payments",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Requests_RequestID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RequestID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Installments",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Payed",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "RequestID",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "ParentRequestID",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "Installments",
                table: "ParentRequests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "Payed",
                table: "ParentRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ParentRequestID",
                table: "Payments",
                column: "ParentRequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_ParentRequests_ParentRequestID",
                table: "Payments",
                column: "ParentRequestID",
                principalTable: "ParentRequests",
                principalColumn: "ParentRequestID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

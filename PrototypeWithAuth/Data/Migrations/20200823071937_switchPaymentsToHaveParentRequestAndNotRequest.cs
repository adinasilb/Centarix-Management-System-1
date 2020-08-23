using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class switchPaymentsToHaveParentRequestAndNotRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Requests_RequestID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RequestID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "RequestID",
                table: "Payments");

            migrationBuilder.AddColumn<int>(
                name: "ParentRequestID",
                table: "Payments",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "RequestID",
                table: "Payments",
                type: "int",
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
    }
}

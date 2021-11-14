using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddRequestListRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestListRequest_RequestLists_ListID",
                table: "RequestListRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestListRequest_Requests_RequestID",
                table: "RequestListRequest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestListRequest",
                table: "RequestListRequest");

            migrationBuilder.RenameTable(
                name: "RequestListRequest",
                newName: "RequestListRequests");

            migrationBuilder.RenameIndex(
                name: "IX_RequestListRequest_RequestID",
                table: "RequestListRequests",
                newName: "IX_RequestListRequests_RequestID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestListRequests",
                table: "RequestListRequests",
                columns: new[] { "ListID", "RequestID" });

            migrationBuilder.AddForeignKey(
                name: "FK_RequestListRequests_RequestLists_ListID",
                table: "RequestListRequests",
                column: "ListID",
                principalTable: "RequestLists",
                principalColumn: "ListID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestListRequests_Requests_RequestID",
                table: "RequestListRequests",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestListRequests_RequestLists_ListID",
                table: "RequestListRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestListRequests_Requests_RequestID",
                table: "RequestListRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestListRequests",
                table: "RequestListRequests");

            migrationBuilder.RenameTable(
                name: "RequestListRequests",
                newName: "RequestListRequest");

            migrationBuilder.RenameIndex(
                name: "IX_RequestListRequests_RequestID",
                table: "RequestListRequest",
                newName: "IX_RequestListRequest_RequestID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestListRequest",
                table: "RequestListRequest",
                columns: new[] { "ListID", "RequestID" });

            migrationBuilder.AddForeignKey(
                name: "FK_RequestListRequest_RequestLists_ListID",
                table: "RequestListRequest",
                column: "ListID",
                principalTable: "RequestLists",
                principalColumn: "ListID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestListRequest_Requests_RequestID",
                table: "RequestListRequest",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddObjectIDToShareBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareProtocols_ProtocolVersions_ProtocolVersionID",
                table: "ShareProtocols");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareRequestLists_RequestLists_RequestListID",
                table: "ShareRequestLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareRequests_Requests_RequestID",
                table: "ShareRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareResources_Resources_ResourceID",
                table: "ShareResources");

            migrationBuilder.DropIndex(
                name: "IX_ShareResources_ResourceID",
                table: "ShareResources");

            migrationBuilder.DropIndex(
                name: "IX_ShareRequests_RequestID",
                table: "ShareRequests");

            migrationBuilder.DropIndex(
                name: "IX_ShareRequestLists_RequestListID",
                table: "ShareRequestLists");

            migrationBuilder.DropIndex(
                name: "IX_ShareProtocols_ProtocolVersionID",
                table: "ShareProtocols");

            migrationBuilder.AddColumn<int>(
                name: "ObjectID",
                table: "ShareResources",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ObjectID",
                table: "ShareRequests",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ObjectID",
                table: "ShareRequestLists",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ObjectID",
                table: "ShareProtocols",
                nullable: true);

            migrationBuilder.Sql("UPDATE ShareRequests SET ObjectID = RequestID ");

            migrationBuilder.Sql("UPDATE ShareResources SET ObjectID = ResourceID ");

            migrationBuilder.Sql("UPDATE ShareRequestLists SET ObjectID = RequestListID ");

            migrationBuilder.Sql("UPDATE ShareProtocols SET ObjectID = ProtocolVersionID ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ObjectID",
                table: "ShareResources");

            migrationBuilder.DropColumn(
                name: "ObjectID",
                table: "ShareRequests");

            migrationBuilder.DropColumn(
                name: "ObjectID",
                table: "ShareRequestLists");

            migrationBuilder.DropColumn(
                name: "ObjectID",
                table: "ShareProtocols");

            migrationBuilder.CreateIndex(
                name: "IX_ShareResources_ResourceID",
                table: "ShareResources",
                column: "ResourceID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequests_RequestID",
                table: "ShareRequests",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequestLists_RequestListID",
                table: "ShareRequestLists",
                column: "RequestListID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareProtocols_ProtocolVersionID",
                table: "ShareProtocols",
                column: "ProtocolVersionID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareProtocols_ProtocolVersions_ProtocolVersionID",
                table: "ShareProtocols",
                column: "ProtocolVersionID",
                principalTable: "ProtocolVersions",
                principalColumn: "ProtocolVersionID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareRequestLists_RequestLists_RequestListID",
                table: "ShareRequestLists",
                column: "RequestListID",
                principalTable: "RequestLists",
                principalColumn: "ListID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareRequests_Requests_RequestID",
                table: "ShareRequests",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareResources_Resources_ResourceID",
                table: "ShareResources",
                column: "ResourceID",
                principalTable: "Resources",
                principalColumn: "ResourceID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

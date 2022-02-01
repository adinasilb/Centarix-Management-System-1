using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddFKToObjectIDShareBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResourceID",
                table: "ShareResources");

            migrationBuilder.DropColumn(
                name: "RequestID",
                table: "ShareRequests");

            migrationBuilder.DropColumn(
                name: "RequestListID",
                table: "ShareRequestLists");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "ShareProtocols");

            migrationBuilder.AlterColumn<int>(
                name: "ObjectID",
                table: "ShareProtocols",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShareResources_ObjectID",
                table: "ShareResources",
                column: "ObjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequests_ObjectID",
                table: "ShareRequests",
                column: "ObjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequestLists_ObjectID",
                table: "ShareRequestLists",
                column: "ObjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareProtocols_ObjectID",
                table: "ShareProtocols",
                column: "ObjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareProtocols_Protocols_ObjectID",
                table: "ShareProtocols",
                column: "ObjectID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareRequestLists_RequestLists_ObjectID",
                table: "ShareRequestLists",
                column: "ObjectID",
                principalTable: "RequestLists",
                principalColumn: "ListID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareRequests_Requests_ObjectID",
                table: "ShareRequests",
                column: "ObjectID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareResources_Resources_ObjectID",
                table: "ShareResources",
                column: "ObjectID",
                principalTable: "Resources",
                principalColumn: "ResourceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareProtocols_Protocols_ObjectID",
                table: "ShareProtocols");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareRequestLists_RequestLists_ObjectID",
                table: "ShareRequestLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareRequests_Requests_ObjectID",
                table: "ShareRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareResources_Resources_ObjectID",
                table: "ShareResources");

            migrationBuilder.DropIndex(
                name: "IX_ShareResources_ObjectID",
                table: "ShareResources");

            migrationBuilder.DropIndex(
                name: "IX_ShareRequests_ObjectID",
                table: "ShareRequests");

            migrationBuilder.DropIndex(
                name: "IX_ShareRequestLists_ObjectID",
                table: "ShareRequestLists");

            migrationBuilder.DropIndex(
                name: "IX_ShareProtocols_ObjectID",
                table: "ShareProtocols");

            migrationBuilder.AddColumn<int>(
                name: "ResourceID",
                table: "ShareResources",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequestID",
                table: "ShareRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequestListID",
                table: "ShareRequestLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ObjectID",
                table: "ShareProtocols",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "ShareProtocols",
                type: "int",
                nullable: true);
        }
    }
}

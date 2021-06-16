using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedFavoriteBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareResources",
                table: "ShareResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareRequests",
                table: "ShareRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareProtocols",
                table: "ShareProtocols");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteResources",
                table: "FavoriteResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteRequests",
                table: "FavoriteRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteProtocols",
                table: "FavoriteProtocols");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ShareResources");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ShareRequests");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ShareProtocols");

            migrationBuilder.DropColumn(
                name: "FavoriteResourceID",
                table: "FavoriteResources");

            migrationBuilder.DropColumn(
                name: "FavoriteRequestID",
                table: "FavoriteRequests");

            migrationBuilder.DropColumn(
                name: "FavoriteProtocolID",
                table: "FavoriteProtocols");

            migrationBuilder.AddColumn<int>(
                name: "ShareID",
                table: "ShareResources",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ShareID",
                table: "ShareRequests",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ShareID",
                table: "ShareProtocols",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteID",
                table: "FavoriteResources",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteID",
                table: "FavoriteRequests",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteID",
                table: "FavoriteProtocols",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareResources",
                table: "ShareResources",
                column: "ShareID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareRequests",
                table: "ShareRequests",
                column: "ShareID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareProtocols",
                table: "ShareProtocols",
                column: "ShareID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteResources",
                table: "FavoriteResources",
                column: "FavoriteID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteRequests",
                table: "FavoriteRequests",
                column: "FavoriteID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteProtocols",
                table: "FavoriteProtocols",
                column: "FavoriteID");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProtocols_ProtocolID",
                table: "FavoriteProtocols",
                column: "ProtocolID");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProtocols_Protocols_ProtocolID",
                table: "FavoriteProtocols",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProtocols_Protocols_ProtocolID",
                table: "FavoriteProtocols");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareResources",
                table: "ShareResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareRequests",
                table: "ShareRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareProtocols",
                table: "ShareProtocols");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteResources",
                table: "FavoriteResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteRequests",
                table: "FavoriteRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteProtocols",
                table: "FavoriteProtocols");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteProtocols_ProtocolID",
                table: "FavoriteProtocols");

            migrationBuilder.DropColumn(
                name: "ShareID",
                table: "ShareResources");

            migrationBuilder.DropColumn(
                name: "ShareID",
                table: "ShareRequests");

            migrationBuilder.DropColumn(
                name: "ShareID",
                table: "ShareProtocols");

            migrationBuilder.DropColumn(
                name: "FavoriteID",
                table: "FavoriteResources");

            migrationBuilder.DropColumn(
                name: "FavoriteID",
                table: "FavoriteRequests");

            migrationBuilder.DropColumn(
                name: "FavoriteID",
                table: "FavoriteProtocols");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ShareResources",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ShareRequests",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ShareProtocols",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteResourceID",
                table: "FavoriteResources",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteRequestID",
                table: "FavoriteRequests",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "FavoriteProtocolID",
                table: "FavoriteProtocols",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareResources",
                table: "ShareResources",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareRequests",
                table: "ShareRequests",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareProtocols",
                table: "ShareProtocols",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteResources",
                table: "FavoriteResources",
                column: "FavoriteResourceID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteRequests",
                table: "FavoriteRequests",
                column: "FavoriteRequestID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteProtocols",
                table: "FavoriteProtocols",
                column: "FavoriteProtocolID");
        }
    }
}

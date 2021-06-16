using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MadeShareBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareResources",
                table: "ShareResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareRequests",
                table: "ShareRequests");

            migrationBuilder.DropColumn(
                name: "ShareResourceID",
                table: "ShareResources");

            migrationBuilder.DropColumn(
                name: "ShareRequestID",
                table: "ShareRequests");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ShareResources",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ShareRequests",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareResources",
                table: "ShareResources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareRequests",
                table: "ShareRequests");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ShareResources");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ShareRequests");

            migrationBuilder.AddColumn<int>(
                name: "ShareResourceID",
                table: "ShareResources",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ShareRequestID",
                table: "ShareRequests",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareResources",
                table: "ShareResources",
                column: "ShareResourceID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareRequests",
                table: "ShareRequests",
                column: "ShareRequestID");
        }
    }
}

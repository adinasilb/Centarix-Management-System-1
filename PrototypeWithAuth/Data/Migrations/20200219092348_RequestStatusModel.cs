using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RequestStatusModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestStatus",
                columns: table => new
                {
                    RequestStatusID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestStatusDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatus", x => x.RequestStatusID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestStatusID",
                table: "Requests",
                column: "RequestStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_RequestStatus_RequestStatusID",
                table: "Requests",
                column: "RequestStatusID",
                principalTable: "RequestStatus",
                principalColumn: "RequestStatusID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_RequestStatus_RequestStatusID",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "RequestStatus");

            migrationBuilder.DropIndex(
                name: "IX_Requests_RequestStatusID",
                table: "Requests");
        }
    }
}

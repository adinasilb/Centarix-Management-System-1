using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addShareRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShareRequests",
                columns: table => new
                {
                    ShareRequestID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestID = table.Column<int>(nullable: false),
                    FromApplicationUserID = table.Column<string>(nullable: true),
                    ToApplicationUserID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareRequests", x => x.ShareRequestID);
                    table.ForeignKey(
                        name: "FK_ShareRequests_AspNetUsers_FromApplicationUserID",
                        column: x => x.FromApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareRequests_Requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Requests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareRequests_AspNetUsers_ToApplicationUserID",
                        column: x => x.ToApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequests_FromApplicationUserID",
                table: "ShareRequests",
                column: "FromApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequests_RequestID",
                table: "ShareRequests",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequests_ToApplicationUserID",
                table: "ShareRequests",
                column: "ToApplicationUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShareRequests");
        }
    }
}

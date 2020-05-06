using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class manyToManyLocationInstancesRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationInstances_Requests_RequestID",
                table: "LocationInstances");

            migrationBuilder.DropIndex(
                name: "IX_LocationInstances_RequestID",
                table: "LocationInstances");

            migrationBuilder.DropColumn(
                name: "RequestID",
                table: "LocationInstances");

            migrationBuilder.CreateTable(
                name: "RequestLocationInstance",
                columns: table => new
                {
                    RequestID = table.Column<int>(nullable: false),
                    LocationInstanceID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestLocationInstance", x => new { x.RequestID, x.LocationInstanceID });
                    table.ForeignKey(
                        name: "FK_RequestLocationInstance_LocationInstances_LocationInstanceID",
                        column: x => x.LocationInstanceID,
                        principalTable: "LocationInstances",
                        principalColumn: "LocationInstanceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestLocationInstance_Requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Requests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestLocationInstance_LocationInstanceID",
                table: "RequestLocationInstance",
                column: "LocationInstanceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestLocationInstance");

            migrationBuilder.AddColumn<int>(
                name: "RequestID",
                table: "LocationInstances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationInstances_RequestID",
                table: "LocationInstances",
                column: "RequestID");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationInstances_Requests_RequestID",
                table: "LocationInstances",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

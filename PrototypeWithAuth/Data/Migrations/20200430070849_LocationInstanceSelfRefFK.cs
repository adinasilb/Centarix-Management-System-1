using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class LocationInstanceSelfRefFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationInstances",
                columns: table => new
                {
                    LocationInstanceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationTypeID = table.Column<int>(nullable: false),
                    LocationInstanceParentID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationInstances", x => x.LocationInstanceID);
                    table.ForeignKey(
                        name: "FK_LocationInstances_LocationInstances_LocationInstanceParentID",
                        column: x => x.LocationInstanceParentID,
                        principalTable: "LocationInstances",
                        principalColumn: "LocationInstanceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationInstances_LocationTypes_LocationTypeID",
                        column: x => x.LocationTypeID,
                        principalTable: "LocationTypes",
                        principalColumn: "LocationTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationInstances_LocationInstanceParentID",
                table: "LocationInstances",
                column: "LocationInstanceParentID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationInstances_LocationTypeID",
                table: "LocationInstances",
                column: "LocationTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationInstances");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class LocationTypeSelfReferenceFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationTypes",
                columns: table => new
                {
                    LocationTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationTypeName = table.Column<string>(nullable: true),
                    LocationTypeParentID = table.Column<int>(nullable: true),
                    LocationTypeChildID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationTypes", x => x.LocationTypeID);
                    table.ForeignKey(
                        name: "FK_LocationTypes_LocationTypes_LocationTypeChildID",
                        column: x => x.LocationTypeChildID,
                        principalTable: "LocationTypes",
                        principalColumn: "LocationTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationTypes_LocationTypes_LocationTypeParentID",
                        column: x => x.LocationTypeParentID,
                        principalTable: "LocationTypes",
                        principalColumn: "LocationTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationTypes_LocationTypeChildID",
                table: "LocationTypes",
                column: "LocationTypeChildID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationTypes_LocationTypeParentID",
                table: "LocationTypes",
                column: "LocationTypeParentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationTypes");
        }
    }
}

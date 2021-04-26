using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddLocationRoomInstanceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationRoomInstances",
                columns: table => new
                {
                    LocationRoomInstanceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationRoomInstancName = table.Column<string>(nullable: true),
                    LocationRoomInstanceAbbrv = table.Column<string>(nullable: true),
                    LocationRoomTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationRoomInstances", x => x.LocationRoomInstanceID);
                    table.ForeignKey(
                        name: "FK_LocationRoomInstances_LocationRoomTypes_LocationRoomTypeID",
                        column: x => x.LocationRoomTypeID,
                        principalTable: "LocationRoomTypes",
                        principalColumn: "LocationRoomTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationRoomInstances_LocationRoomTypeID",
                table: "LocationRoomInstances",
                column: "LocationRoomTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationRoomInstances");
        }
    }
}

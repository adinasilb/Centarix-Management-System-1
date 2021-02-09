using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddLocationInstanceRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationRoomTypes",
                columns: table => new
                {
                    LocationRoomTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationRoomTypeDescription = table.Column<string>(nullable: true),
                    LocationAbbreviation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationRoomTypes", x => x.LocationRoomTypeID);
                });

            migrationBuilder.CreateTable(
                name: "LocationRoomInstances",
                columns: table => new
                {
                    LocationRoomInstanceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationNumber = table.Column<int>(nullable: false),
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

            migrationBuilder.InsertData(
                table: "LocationRoomTypes",
                columns: new[] { "LocationRoomTypeID", "LocationAbbreviation", "LocationRoomTypeDescription" },
                values: new object[,]
                {
                    { 1, "L", "Laboratory" },
                    { 2, "", "Tissue Culture" },
                    { 3, "E", "Equipment Room" },
                    { 4, "R", "Refrigerator Room" },
                    { 5, "W", "Washing Room" },
                    { 6, "S", "Storage Room" }
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

            migrationBuilder.DropTable(
                name: "LocationRoomTypes");
        }
    }
}

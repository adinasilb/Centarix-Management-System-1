using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class TookAwayLocationRoomInstances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationInstances_LocationRoomInstances_LocationRoomInstanceID",
                table: "LocationInstances");

            migrationBuilder.DropTable(
                name: "LocationRoomInstances");

            migrationBuilder.DropIndex(
                name: "IX_LocationInstances_LocationRoomInstanceID",
                table: "LocationInstances");

            migrationBuilder.DropColumn(
                name: "LocationRoomInstanceID",
                table: "LocationInstances");

            migrationBuilder.AddColumn<int>(
                name: "LocationRoomTypeID",
                table: "LocationInstances",
                nullable: true);

            migrationBuilder.InsertData(
                table: "LocationInstances",
                columns: new[] { "LocationInstanceID", "CompanyLocationNo", "ContainsItems", "Height", "IsEmptyShelf", "IsFull", "LabPartID", "LocationInstanceAbbrev", "LocationInstanceName", "LocationInstanceParentID", "LocationNumber", "LocationRoomTypeID", "LocationTypeID", "Place", "Width" },
                values: new object[] { 1, 0, false, 7, false, false, null, null, "25°C", null, 0, 1, 500, null, 1 });

            migrationBuilder.InsertData(
                table: "LocationInstances",
                columns: new[] { "LocationInstanceID", "CompanyLocationNo", "ContainsItems", "Height", "IsEmptyShelf", "IsFull", "LabPartID", "LocationInstanceAbbrev", "LocationInstanceName", "LocationInstanceParentID", "LocationNumber", "LocationRoomTypeID", "LocationTypeID", "Place", "Width" },
                values: new object[,]
                {
                    { 2, 0, false, 0, false, false, null, null, "Laboratory 1", 1, 0, 1, 501, null, 0 },
                    { 3, 0, false, 0, false, false, null, null, "Laboratory 2", 1, 0, 1, 501, null, 0 },
                    { 4, 0, false, 0, false, false, null, null, "Tissue Culture 1", 1, 0, 2, 501, null, 0 },
                    { 5, 0, false, 0, false, false, null, null, "Equipment Room 1", 1, 0, 3, 501, null, 0 },
                    { 6, 0, false, 0, false, false, null, null, "Refrigerator Room 1", 1, 0, 4, 501, null, 0 },
                    { 7, 0, false, 0, false, false, null, null, "Washing Room 1", 1, 0, 5, 501, null, 0 },
                    { 8, 0, false, 0, false, false, null, null, "Storage Room 1", 1, 0, 6, 501, null, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationInstances_LocationRoomTypeID",
                table: "LocationInstances",
                column: "LocationRoomTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationInstances_LocationRoomTypes_LocationRoomTypeID",
                table: "LocationInstances",
                column: "LocationRoomTypeID",
                principalTable: "LocationRoomTypes",
                principalColumn: "LocationRoomTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationInstances_LocationRoomTypes_LocationRoomTypeID",
                table: "LocationInstances");

            migrationBuilder.DropIndex(
                name: "IX_LocationInstances_LocationRoomTypeID",
                table: "LocationInstances");

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "LocationInstances",
                keyColumn: "LocationInstanceID",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "LocationRoomTypeID",
                table: "LocationInstances");

            migrationBuilder.AddColumn<int>(
                name: "LocationRoomInstanceID",
                table: "LocationInstances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LocationRoomInstances",
                columns: table => new
                {
                    LocationRoomInstanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationRoomInstanceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationRoomTypeID = table.Column<int>(type: "int", nullable: false)
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
                table: "LocationRoomInstances",
                columns: new[] { "LocationRoomInstanceID", "LocationRoomInstanceName", "LocationRoomTypeID" },
                values: new object[,]
                {
                    { 1, "Laboratory 1", 1 },
                    { 2, "Laboratory 2", 1 },
                    { 3, "Tissue Culture 1", 2 },
                    { 4, "Equipment Room 1", 3 },
                    { 5, "Refrigerator Room 1", 4 },
                    { 6, "Washing Room 1", 5 },
                    { 7, "Storage Room 1", 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationInstances_LocationRoomInstanceID",
                table: "LocationInstances",
                column: "LocationRoomInstanceID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationRoomInstances_LocationRoomTypeID",
                table: "LocationRoomInstances",
                column: "LocationRoomTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationInstances_LocationRoomInstances_LocationRoomInstanceID",
                table: "LocationInstances",
                column: "LocationRoomInstanceID",
                principalTable: "LocationRoomInstances",
                principalColumn: "LocationRoomInstanceID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

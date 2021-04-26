using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class PutModelExtenstionDataBack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_LocationInstances_LocationRoomTypes_LocationRoomTypeID",
            //    table: "LocationInstances");

            //migrationBuilder.DropIndex(
            //    name: "IX_LocationInstances_LocationRoomTypeID",
            //    table: "LocationInstances");

            //migrationBuilder.DeleteData(
            //    table: "ResourceTypes",
            //    keyColumn: "ResourceTypeId",
            //    keyValue: 1);

            //migrationBuilder.DeleteData(
            //    table: "ResourceTypes",
            //    keyColumn: "ResourceTypeId",
            //    keyValue: 2);

            //migrationBuilder.DropColumn(
            //    name: "CompanyLocationNo",
            //    table: "LocationInstances");

            //migrationBuilder.DropColumn(
            //    name: "LocationRoomTypeID",
            //    table: "LocationInstances");

            //migrationBuilder.DropColumn(
            //    name: "Place",
            //    table: "LocationInstances");

            //migrationBuilder.AddColumn<int>(
            //    name: "LocationRoomInstanceID",
            //    table: "LocationInstances",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "LocationRoomInstances",
            //    columns: table => new
            //    {
            //        LocationRoomInstanceID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        LocationRoomInstanceName = table.Column<string>(nullable: true),
            //        LocationRoomInstanceAbbrev = table.Column<string>(nullable: true),
            //        LocationRoomTypeID = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_LocationRoomInstances", x => x.LocationRoomInstanceID);
            //        table.ForeignKey(
            //            name: "FK_LocationRoomInstances_LocationRoomTypes_LocationRoomTypeID",
            //            column: x => x.LocationRoomTypeID,
            //            principalTable: "LocationRoomTypes",
            //            principalColumn: "LocationRoomTypeID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.InsertData(
            //    table: "MaterialCategories",
            //    columns: new[] { "MaterialCategoryID", "MaterialCategoryDescription" },
            //    values: new object[,]
            //    {
            //        { 1, "Reagents" },
            //        { 2, "Plastics" },
            //        { 3, "Equipment" },
            //        { 4, "Buffers" }
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_LocationInstances_LocationRoomInstanceID",
            //    table: "LocationInstances",
            //    column: "LocationRoomInstanceID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_LocationRoomInstances_LocationRoomTypeID",
            //    table: "LocationRoomInstances",
            //    column: "LocationRoomTypeID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_LocationInstances_LocationRoomInstances_LocationRoomInstanceID",
            //    table: "LocationInstances",
            //    column: "LocationRoomInstanceID",
            //    principalTable: "LocationRoomInstances",
            //    principalColumn: "LocationRoomInstanceID",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_LocationInstances_LocationRoomInstances_LocationRoomInstanceID",
            //    table: "LocationInstances");

            //migrationBuilder.DropTable(
            //    name: "LocationRoomInstances");

            //migrationBuilder.DropIndex(
            //    name: "IX_LocationInstances_LocationRoomInstanceID",
            //    table: "LocationInstances");

            //migrationBuilder.DeleteData(
            //    table: "MaterialCategories",
            //    keyColumn: "MaterialCategoryID",
            //    keyValue: 1);

            //migrationBuilder.DeleteData(
            //    table: "MaterialCategories",
            //    keyColumn: "MaterialCategoryID",
            //    keyValue: 2);

            //migrationBuilder.DeleteData(
            //    table: "MaterialCategories",
            //    keyColumn: "MaterialCategoryID",
            //    keyValue: 3);

            //migrationBuilder.DeleteData(
            //    table: "MaterialCategories",
            //    keyColumn: "MaterialCategoryID",
            //    keyValue: 4);

            //migrationBuilder.DropColumn(
            //    name: "LocationRoomInstanceID",
            //    table: "LocationInstances");

            //migrationBuilder.AddColumn<int>(
            //    name: "CompanyLocationNo",
            //    table: "LocationInstances",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "LocationRoomTypeID",
            //    table: "LocationInstances",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Place",
            //    table: "LocationInstances",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.UpdateData(
            //    table: "LocationInstances",
            //    keyColumn: "LocationInstanceID",
            //    keyValue: 2,
            //    column: "LocationRoomTypeID",
            //    value: 1);

            //migrationBuilder.UpdateData(
            //    table: "LocationInstances",
            //    keyColumn: "LocationInstanceID",
            //    keyValue: 3,
            //    column: "LocationRoomTypeID",
            //    value: 1);

            //migrationBuilder.UpdateData(
            //    table: "LocationInstances",
            //    keyColumn: "LocationInstanceID",
            //    keyValue: 4,
            //    column: "LocationRoomTypeID",
            //    value: 2);

            //migrationBuilder.UpdateData(
            //    table: "LocationInstances",
            //    keyColumn: "LocationInstanceID",
            //    keyValue: 5,
            //    column: "LocationRoomTypeID",
            //    value: 3);

            //migrationBuilder.UpdateData(
            //    table: "LocationInstances",
            //    keyColumn: "LocationInstanceID",
            //    keyValue: 6,
            //    column: "LocationRoomTypeID",
            //    value: 4);

            //migrationBuilder.UpdateData(
            //    table: "LocationInstances",
            //    keyColumn: "LocationInstanceID",
            //    keyValue: 7,
            //    column: "LocationRoomTypeID",
            //    value: 5);

            //migrationBuilder.UpdateData(
            //    table: "LocationInstances",
            //    keyColumn: "LocationInstanceID",
            //    keyValue: 8,
            //    column: "LocationRoomTypeID",
            //    value: 6);

            //migrationBuilder.InsertData(
            //    table: "ResourceTypes",
            //    columns: new[] { "ResourceTypeId", "ResourceTypeDescription" },
            //    values: new object[,]
            //    {
            //        { 1, "Articles and Links" },
            //        { 2, "Resources" }
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_LocationInstances_LocationRoomTypeID",
            //    table: "LocationInstances",
            //    column: "LocationRoomTypeID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_LocationInstances_LocationRoomTypes_LocationRoomTypeID",
            //    table: "LocationInstances",
            //    column: "LocationRoomTypeID",
            //    principalTable: "LocationRoomTypes",
            //    principalColumn: "LocationRoomTypeID",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}

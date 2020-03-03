using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addUnitParentType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnitParentTypeID",
                table: "UnitTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UnitParentTypes",
                columns: table => new
                {
                    UnitParentTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitParentTypeDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitParentTypes", x => x.UnitParentTypeID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitTypes_UnitParentTypeID",
                table: "UnitTypes",
                column: "UnitParentTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_UnitTypes_UnitParentTypes_UnitParentTypeID",
                table: "UnitTypes",
                column: "UnitParentTypeID",
                principalTable: "UnitParentTypes",
                principalColumn: "UnitParentTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnitTypes_UnitParentTypes_UnitParentTypeID",
                table: "UnitTypes");

            migrationBuilder.DropTable(
                name: "UnitParentTypes");

            migrationBuilder.DropIndex(
                name: "IX_UnitTypes_UnitParentTypeID",
                table: "UnitTypes");

            migrationBuilder.DropColumn(
                name: "UnitParentTypeID",
                table: "UnitTypes");
        }
    }
}

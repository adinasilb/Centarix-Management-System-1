using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedCalibrationTypeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalibrationTypeID",
                table: "Calibrations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Calibrations",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CalibrationTypes",
                columns: table => new
                {
                    CalibrationTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationTypes", x => x.CalibrationTypeID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calibrations_CalibrationTypeID",
                table: "Calibrations",
                column: "CalibrationTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Calibrations_CalibrationTypes_CalibrationTypeID",
                table: "Calibrations",
                column: "CalibrationTypeID",
                principalTable: "CalibrationTypes",
                principalColumn: "CalibrationTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calibrations_CalibrationTypes_CalibrationTypeID",
                table: "Calibrations");

            migrationBuilder.DropTable(
                name: "CalibrationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Calibrations_CalibrationTypeID",
                table: "Calibrations");

            migrationBuilder.DropColumn(
                name: "CalibrationTypeID",
                table: "Calibrations");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Calibrations");
        }
    }
}

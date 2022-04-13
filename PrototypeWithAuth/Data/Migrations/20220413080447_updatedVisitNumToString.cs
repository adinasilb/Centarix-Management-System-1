using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class updatedVisitNumToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExperimentEntries_ParticipantID_VisitNumber",
                table: "ExperimentEntries");

            migrationBuilder.AlterColumn<string>(
                name: "VisitNumber",
                table: "ExperimentEntries",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentEntries_ParticipantID_VisitNumber",
                table: "ExperimentEntries",
                columns: new[] { "ParticipantID", "VisitNumber" },
                unique: true,
                filter: "[VisitNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExperimentEntries_ParticipantID_VisitNumber",
                table: "ExperimentEntries");

            migrationBuilder.AlterColumn<int>(
                name: "VisitNumber",
                table: "ExperimentEntries",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentEntries_ParticipantID_VisitNumber",
                table: "ExperimentEntries",
                columns: new[] { "ParticipantID", "VisitNumber" },
                unique: true);
        }
    }
}

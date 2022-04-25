using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class updatedVisitNumberNotUnique : Migration
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
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentEntries_ParticipantID",
                table: "ExperimentEntries",
                column: "ParticipantID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExperimentEntries_ParticipantID",
                table: "ExperimentEntries");

            migrationBuilder.AlterColumn<string>(
                name: "VisitNumber",
                table: "ExperimentEntries",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentEntries_ParticipantID_VisitNumber",
                table: "ExperimentEntries",
                columns: new[] { "ParticipantID", "VisitNumber" },
                unique: true,
                filter: "[VisitNumber] IS NOT NULL");
        }
    }
}

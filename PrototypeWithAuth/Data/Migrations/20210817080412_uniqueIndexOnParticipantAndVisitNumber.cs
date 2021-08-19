using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class uniqueIndexOnParticipantAndVisitNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExperimentEntries_ParticipantID",
                table: "ExperimentEntries");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentEntries_ParticipantID_VisitNumber",
                table: "ExperimentEntries",
                columns: new[] { "ParticipantID", "VisitNumber" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ExperimentEntries_ParticipantID_VisitNumber",
                table: "ExperimentEntries");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentEntries_ParticipantID",
                table: "ExperimentEntries",
                column: "ParticipantID");
        }
    }
}

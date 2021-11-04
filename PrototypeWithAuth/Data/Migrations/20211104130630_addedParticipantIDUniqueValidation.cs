using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedParticipantIDUniqueValidation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Participants_ParticipantID_ParticipantStatusID",
                table: "Participants",
                columns: new[] { "ParticipantID", "ParticipantStatusID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Participants_ParticipantID_ParticipantStatusID",
                table: "Participants");
        }
    }
}

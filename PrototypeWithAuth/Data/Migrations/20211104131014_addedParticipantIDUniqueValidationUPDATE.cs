using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedParticipantIDUniqueValidationUPDATE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Participants_ParticipantID_ParticipantStatusID",
                table: "Participants");

            migrationBuilder.AlterColumn<string>(
                name: "CentarixID",
                table: "Participants",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_ParticipantID_CentarixID",
                table: "Participants",
                columns: new[] { "ParticipantID", "CentarixID" },
                unique: true,
                filter: "[CentarixID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Participants_ParticipantID_CentarixID",
                table: "Participants");

            migrationBuilder.AlterColumn<string>(
                name: "CentarixID",
                table: "Participants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_ParticipantID_ParticipantStatusID",
                table: "Participants",
                columns: new[] { "ParticipantID", "ParticipantStatusID" },
                unique: true);
        }
    }
}

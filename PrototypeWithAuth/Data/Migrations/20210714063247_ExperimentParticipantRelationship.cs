using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ExperimentParticipantRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExperimentID",
                table: "Participants",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Participants_ExperimentID",
                table: "Participants",
                column: "ExperimentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Experiments_ExperimentID",
                table: "Participants",
                column: "ExperimentID",
                principalTable: "Experiments",
                principalColumn: "ExperimentID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Experiments_ExperimentID",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_ExperimentID",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "ExperimentID",
                table: "Participants");
        }
    }
}

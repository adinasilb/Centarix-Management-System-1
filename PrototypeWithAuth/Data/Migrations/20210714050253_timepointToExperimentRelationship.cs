using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class timepointToExperimentRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfTimepoints",
                table: "Experiments");

            migrationBuilder.AddColumn<int>(
                name: "ExperimentID",
                table: "Timepoints",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Timepoints_ExperimentID",
                table: "Timepoints",
                column: "ExperimentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Timepoints_Experiments_ExperimentID",
                table: "Timepoints",
                column: "ExperimentID",
                principalTable: "Experiments",
                principalColumn: "ExperimentID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timepoints_Experiments_ExperimentID",
                table: "Timepoints");

            migrationBuilder.DropIndex(
                name: "IX_Timepoints_ExperimentID",
                table: "Timepoints");

            migrationBuilder.DropColumn(
                name: "ExperimentID",
                table: "Timepoints");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTimepoints",
                table: "Experiments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

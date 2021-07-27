using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seededGenderAndParticipantStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "GenderID", "Description" },
                values: new object[,]
                {
                    { 1, "Male" },
                    { 2, "Female" }
                });

            migrationBuilder.InsertData(
                table: "ParticipantStatuses",
                columns: new[] { "ParticipantStatusID", "Description" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Dropout" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "GenderID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genders",
                keyColumn: "GenderID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ParticipantStatuses",
                keyColumn: "ParticipantStatusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ParticipantStatuses",
                keyColumn: "ParticipantStatusID",
                keyValue: 2);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddDegreeCitizenshipMaritalData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Citizenships",
                columns: new[] { "CitizenshipID", "Description" },
                values: new object[,]
                {
                    { 1, "Israel" },
                    { 2, "USA" }
                });

            migrationBuilder.InsertData(
                table: "Degrees",
                columns: new[] { "DegreeID", "Description" },
                values: new object[,]
                {
                    { 1, "B.Sc" },
                    { 2, "M.Sc" },
                    { 3, "P.hd" },
                    { 4, "Post P.hd" }
                });

            migrationBuilder.InsertData(
                table: "MaritalStatuses",
                columns: new[] { "MaritalStatusID", "Description" },
                values: new object[,]
                {
                    { 1, "Married" },
                    { 2, "Single" },
                    { 3, "Divorced" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Citizenships",
                keyColumn: "CitizenshipID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Citizenships",
                keyColumn: "CitizenshipID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Degrees",
                keyColumn: "DegreeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Degrees",
                keyColumn: "DegreeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Degrees",
                keyColumn: "DegreeID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Degrees",
                keyColumn: "DegreeID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MaritalStatuses",
                keyColumn: "MaritalStatusID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MaritalStatuses",
                keyColumn: "MaritalStatusID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MaritalStatuses",
                keyColumn: "MaritalStatusID",
                keyValue: 3);
        }
    }
}

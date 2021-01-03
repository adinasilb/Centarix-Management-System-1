using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class seededUpdatedJobCategoriesAndJobSubcategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 11);

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 2,
                column: "Description",
                value: "Rejuvenation");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 3,
                column: "Description",
                value: "Biomarker");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 4,
                column: "Description",
                value: "Delivery Systems");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 5,
                column: "Description",
                value: "Clinical Trials");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 6,
                column: "Description",
                value: "Business Development");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 7,
                column: "Description",
                value: "Software Development");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 8,
                column: "Description",
                value: "General");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 9,
                column: "Description",
                value: "Lab");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 10,
                column: "Description",
                value: "Bioinformatics");

            migrationBuilder.InsertData(
                table: "JobSubcategoryTypes",
                columns: new[] { "JobSubcategoryTypeID", "Description", "JobCategoryTypeID" },
                values: new object[,]
                {
                    { 1004, "Bioinformatics Technician", 10 },
                    { 1003, "Bioinformatician Team Manager", 10 },
                    { 801, "Cooking", 8 },
                    { 1001, "Senior Bioinformatician", 10 },
                    { 903, "Automations Implementer", 9 },
                    { 902, "Automations Manager", 9 },
                    { 901, "Lab Manager", 9 },
                    { 806, "Branch Manager", 8 },
                    { 805, "Operations Manager", 8 },
                    { 804, "Administration", 8 },
                    { 803, "IT", 8 },
                    { 802, "Cleaning", 8 },
                    { 1005, "Bioinformatics Researcher", 10 },
                    { 1002, "Bioinformatician Executive", 10 },
                    { 703, "Other", 7 },
                    { 701, "Elixir", 7 },
                    { 202, "Research Associate", 2 },
                    { 203, "Lab Technician", 2 },
                    { 204, "Team Manager", 2 },
                    { 205, "Production Worker", 2 },
                    { 206, "Operation Executive", 2 },
                    { 207, "Business Development", 2 },
                    { 208, "Sales", 2 },
                    { 209, "Lab Manager", 2 },
                    { 210, "Bioinformatician", 2 },
                    { 101, "CEO", 1 },
                    { 102, "CTO", 1 },
                    { 103, "COO", 1 },
                    { 104, "President", 1 },
                    { 105, "Director", 1 },
                    { 106, "CSO", 1 },
                    { 107, "CMO", 1 },
                    { 108, "CFO", 1 },
                    { 109, "CBO", 1 },
                    { 601, "Sales", 6 },
                    { 702, "Automation Developer", 7 },
                    { 201, "Senior Scientist", 2 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 601);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 701);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 702);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 703);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 801);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 802);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 803);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 804);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 805);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 806);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 901);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 902);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 903);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "JobSubcategoryTypes",
                keyColumn: "JobSubcategoryTypeID",
                keyValue: 1005);

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 2,
                column: "Description",
                value: "Senior Manager");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 3,
                column: "Description",
                value: "Manager");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 4,
                column: "Description",
                value: "Senior Bioinformatician");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 5,
                column: "Description",
                value: "Bioinformatician");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 6,
                column: "Description",
                value: "Senior Scientist");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 7,
                column: "Description",
                value: "Lab Technician");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 8,
                column: "Description",
                value: "Research Associate");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 9,
                column: "Description",
                value: "Software Developer");

            migrationBuilder.UpdateData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 10,
                column: "Description",
                value: "Administration");

            migrationBuilder.InsertData(
                table: "JobCategoryTypes",
                columns: new[] { "JobCategoryTypeID", "Description" },
                values: new object[] { 11, "General" });
        }
    }
}

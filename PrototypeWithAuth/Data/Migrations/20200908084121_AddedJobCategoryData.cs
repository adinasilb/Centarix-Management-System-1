using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedJobCategoryData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 1,
                column: "Description",
                value: "Salaried Employee");

            migrationBuilder.InsertData(
                table: "JobCategoryTypes",
                columns: new[] { "JobCategoryTypeID", "Description" },
                values: new object[,]
                {
                    { 1, "Executive" },
                    { 2, "Senior Manager" },
                    { 3, "Manager" },
                    { 4, "Senior Bioinformatician" },
                    { 5, "Bioinformatician" },
                    { 6, "Senior Scientist" },
                    { 7, "Lab Technician" },
                    { 8, "Research Associate" },
                    { 9, "Software Developer" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "JobCategoryTypes",
                keyColumn: "JobCategoryTypeID",
                keyValue: 9);

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 1,
                column: "Description",
                value: "SalariedEmployee");
        }
    }
}

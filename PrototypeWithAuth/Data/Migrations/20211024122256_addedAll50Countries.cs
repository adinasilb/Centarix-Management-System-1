using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedAll50Countries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryID", "CountryName" },
                values: new object[,]
                {
                    { 5, "Bosnia and Herzegovina" },
                    { 30, "Mauritius" },
                    { 31, "Moldova" },
                    { 32, "Netherlands" },
                    { 33, "New Zealand" },
                    { 34, "North Macedonia" },
                    { 35, "Norway" },
                    { 36, "Poland" },
                    { 37, "Portugal" },
                    { 38, "Romania" },
                    { 39, "Russia" },
                    { 40, "Singapore" },
                    { 41, "Slovakia" },
                    { 42, "Slovenia" },
                    { 43, "South Africa" },
                    { 44, "South Korea" },
                    { 45, "Spain" },
                    { 46, "Sweden" },
                    { 47, "Switzerland" },
                    { 48, "United Kingdom" },
                    { 29, "Malaysia" },
                    { 28, "Luxembourg" },
                    { 27, "Lithuania" },
                    { 26, "Latvia" },
                    { 6, "Bulgaria" },
                    { 7, "Canada" },
                    { 8, "Chile" },
                    { 9, "Costa Rica" },
                    { 10, "Cyprus" },
                    { 11, "Czech Republic" },
                    { 12, "Denmark" },
                    { 13, "Estonia" },
                    { 14, "Finland" },
                    { 49, "United States" },
                    { 15, "France" },
                    { 17, "Germany" },
                    { 18, "Greece" },
                    { 19, "Hungary" },
                    { 20, "Iceland" },
                    { 21, "India" },
                    { 22, "Ireland" },
                    { 23, "Israel" },
                    { 24, "Italy" },
                    { 25, "Japan" },
                    { 16, "Georgia" },
                    { 50, "Uruguay" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "CountryID",
                keyValue: 50);
        }
    }
}

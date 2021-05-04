using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class OperationSubcategoryUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 916);

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 801,
                column: "ImageURL",
                value: "/images/css/CategoryImages/it/communications.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 802,
                column: "ImageURL",
                value: "/images/css/CategoryImages/it/cybersecurity.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 803,
                column: "ImageURL",
                value: "/images/css/CategoryImages/it/hardware3.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 804,
                column: "ImageURL",
                value: "/images/css/CategoryImages/it/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 901,
                column: "ImageURL",
                value: "/images/css/CategoryImages/daytoday/taxes2.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 902,
                column: "ImageURL",
                value: "/images/css/CategoryImages/daytoday/books.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 903,
                column: "ImageURL",
                value: "/images/css/CategoryImages/daytoday/branding.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 904,
                column: "ImageURL",
                value: "/images/css/CategoryImages/daytoday/company_events.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 905,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/daytoday/electricity2.png", "Electricity" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 906,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/daytoday/fees.png", "Fees" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 907,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/daytoday/food.png", "Food" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 908,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/daytoday/furniture2.png", "Furniture" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 909,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/daytoday/general.png", "General" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 910,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/daytoday/graphics.png", "Graphic" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 911,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/daytoday/insurance.png", "Insurance" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 912,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/daytoday/parking2.png", "Parking" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 913,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/daytoday/renovation.png", "Renovation" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 914,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/daytoday/rent2.png", "Rent" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 915,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/daytoday/shippment.png", "Shipment" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1001,
                column: "ImageURL",
                value: "/images/css/CategoryImages/travel/conference3.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1002,
                column: "ImageURL",
                value: "/images/css/CategoryImages/travel/flight_tickets.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1003,
                column: "ImageURL",
                value: "/images/css/CategoryImages/travel/food.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1004,
                column: "ImageURL",
                value: "/images/css/CategoryImages/travel/hotels3.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1005,
                column: "ImageURL",
                value: "/images/css/CategoryImages/travel/travel.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1006,
                column: "ImageURL",
                value: "/images/css/CategoryImages/travel/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1101,
                column: "ImageURL",
                value: "/images/css/CategoryImages/advice/business_advice.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1102,
                column: "ImageURL",
                value: "/images/css/CategoryImages/advice/clinical_regulation2.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1103,
                column: "ImageURL",
                value: "/images/css/CategoryImages/advice/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1104,
                column: "ImageURL",
                value: "/images/css/CategoryImages/advice/legal.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1105,
                column: "ImageURL",
                value: "/images/css/CategoryImages/advice/scientific_advice3.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1201,
                column: "ImageURL",
                value: "/images/css/CategoryImages/regulations/regulations.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1202,
                column: "ImageURL",
                value: "/images/css/CategoryImages/regulations/safety.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1203,
                column: "ImageURL",
                value: "/images/css/CategoryImages/regulations/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1301,
                column: "ImageURL",
                value: "/images/css/CategoryImages/government/taxes4.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1302,
                column: "ImageURL",
                value: "/images/css/CategoryImages/government/general.png");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 801,
                column: "ImageURL",
                value: "/images/css/CategoryImages/branding.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 802,
                column: "ImageURL",
                value: "/images/css/CategoryImages/shippment.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 803,
                column: "ImageURL",
                value: "/images/css/CategoryImages/renovation.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 804,
                column: "ImageURL",
                value: "/images/css/CategoryImages/bookeeping.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 901,
                column: "ImageURL",
                value: "/images/css/CategoryImages/conference.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 902,
                column: "ImageURL",
                value: "/images/css/CategoryImages/conference.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 903,
                column: "ImageURL",
                value: "/images/css/CategoryImages/conference.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 904,
                column: "ImageURL",
                value: "/images/css/CategoryImages/conference.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 905,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/conference.png", "Bookeeping" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 906,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/conference.png", "Electricity" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 907,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/conference.png", "Fees" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 908,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/conference.png", "Food" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 909,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/conference.png", "Furniture" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 910,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/conference.png", "General" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 911,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/conference.png", "Graphic" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 912,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/conference.png", "Insurance" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 913,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/conference.png", "Parking" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 914,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/conference.png", "Renovation" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 915,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/conference.png", "Rent" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1001,
                column: "ImageURL",
                value: "/images/css/CategoryImages/legal.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1002,
                column: "ImageURL",
                value: "/images/css/CategoryImages/legal.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1003,
                column: "ImageURL",
                value: "/images/css/CategoryImages/legal.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1004,
                column: "ImageURL",
                value: "/images/css/CategoryImages/legal.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1005,
                column: "ImageURL",
                value: "/images/css/CategoryImages/legal.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1006,
                column: "ImageURL",
                value: "/images/css/CategoryImages/legal.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1101,
                column: "ImageURL",
                value: "/images/css/CategoryImages/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1102,
                column: "ImageURL",
                value: "/images/css/CategoryImages/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1103,
                column: "ImageURL",
                value: "/images/css/CategoryImages/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1104,
                column: "ImageURL",
                value: "/images/css/CategoryImages/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1105,
                column: "ImageURL",
                value: "/images/css/CategoryImages/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1201,
                column: "ImageURL",
                value: "/images/css/CategoryImages/taxes.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1202,
                column: "ImageURL",
                value: "/images/css/CategoryImages/taxes.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1203,
                column: "ImageURL",
                value: "/images/css/CategoryImages/taxes.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1301,
                column: "ImageURL",
                value: "/images/css/CategoryImages/general.png");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1302,
                column: "ImageURL",
                value: "/images/css/CategoryImages/general.png");

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[] { 916, "/images/css/CategoryImages/conference.png", 9, "Shipment" });
        }
    }
}

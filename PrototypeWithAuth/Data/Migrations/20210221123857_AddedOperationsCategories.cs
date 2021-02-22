using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedOperationsCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 605);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 606);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 607);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 608);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 609);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 610);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 611);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 612);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 613);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 614);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 615);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 616);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 617);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 618);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 619);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 620);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 621);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 622);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 623);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 624);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 625);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 626);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 627);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 628);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 629);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 6,
                column: "ParentCategoryDescription",
                value: "IT");

            migrationBuilder.InsertData(
                table: "ParentCategories",
                columns: new[] { "ParentCategoryID", "CategoryTypeID", "ParentCategoryDescription", "isProprietary" },
                values: new object[,]
                {
                    { 8, 2, "Day To Day", false },
                    { 9, 2, "Travel", false },
                    { 10, 2, "Advisment", false },
                    { 11, 2, "Regulations", false },
                    { 12, 2, "Governments", false },
                    { 13, 2, "General", false }
                });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 601,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/software.png", "Hardware" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 602,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/communications.png", "Communication" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 603,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/software.png", "Cybersecurity" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 604,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/general.png", "General" });

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 801, "/images/css/CategoryImages/rent.png", 6, "Rent" },
                    { 808, "/images/css/CategoryImages/branding.png", 6, "Branding" },
                    { 809, "/images/css/CategoryImages/shippment.png", 6, "Shipment" }
                });

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 804, "/images/css/CategoryImages/renovation.png", 8, "Rennovation" },
                    { 1203, "/images/css/CategoryImages/general.png", 12, "General" },
                    { 1201, "/images/css/CategoryImages/taxes.png", 12, "Tax" },
                    { 1101, "/images/css/CategoryImages/safety.png", 11, "Safety" },
                    { 1102, "/images/css/CategoryImages/general.png", 11, "General" },
                    { 1003, "/images/css/CategoryImages/business_advice.png", 10, "Business" },
                    { 1002, "/images/css/CategoryImages/sciemtific_advice.png", 10, "Scientific" },
                    { 1005, "/images/css/CategoryImages/general.png", 10, "General" },
                    { 1004, "/images/css/CategoryImages/clinical_regulation.png", 10, "Clinical Experiments" },
                    { 1001, "/images/css/CategoryImages/legal.png", 10, "Law" },
                    { 904, "/images/css/CategoryImages/food.png", 9, "Food" },
                    { 902, "/images/css/CategoryImages/flight_tickets.png", 9, "Flight Tickets" },
                    { 1202, "/images/css/CategoryImages/furniture.png", 12, "Fees" },
                    { 903, "/images/css/CategoryImages/hotels.png", 9, "Hotels" },
                    { 901, "/images/css/CategoryImages/conference.png", 9, "Conference" },
                    { 813, "/images/css/CategoryImages/furniture.png", 8, "Graphics" },
                    { 803, null, 8, "Parking" },
                    { 802, null, 8, "Electricity" },
                    { 806, "/images/css/CategoryImages/furniture.png", 8, "Furniture" },
                    { 810, "/images/css/CategoryImages/food.png", 8, "Food" },
                    { 814, "/images/css/CategoryImages/general.png", 8, "General" },
                    { 805, "/images/css/CategoryImages/insurance.png", 8, "Insurance" },
                    { 807, "/images/css/CategoryImages/company_events.png", 8, "Company Events" },
                    { 811, "/images/css/CategoryImages/books.png", 8, "Books And Journal" },
                    { 812, "/images/css/CategoryImages/bookeeping.png", 8, "Bookkeeping" },
                    { 905, "/images/css/CategoryImages/general.png", 9, "General" },
                    { 1301, "/images/css/CategoryImages/general.png", 13, "General" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 801);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 802);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 803);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 804);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 805);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 806);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 807);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 808);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 809);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 810);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 811);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 812);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 813);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 814);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 901);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 902);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 903);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 904);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 905);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1001);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1002);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1003);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1004);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1005);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1101);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1102);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1201);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1202);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1203);

            migrationBuilder.DeleteData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 1301);

            migrationBuilder.DeleteData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 13);

            migrationBuilder.UpdateData(
                table: "ParentCategories",
                keyColumn: "ParentCategoryID",
                keyValue: 6,
                column: "ParentCategoryDescription",
                value: "Operation");

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 601,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/computer.png", "Computer" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 602,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/rent.png", "Rent" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 603,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/communications.png", "Communication" });

            migrationBuilder.UpdateData(
                table: "ProductSubcategories",
                keyColumn: "ProductSubcategoryID",
                keyValue: 604,
                columns: new[] { "ImageURL", "ProductSubcategoryDescription" },
                values: new object[] { "/images/css/CategoryImages/branding.png", "Branding" });

            migrationBuilder.InsertData(
                table: "ProductSubcategories",
                columns: new[] { "ProductSubcategoryID", "ImageURL", "ParentCategoryID", "ProductSubcategoryDescription" },
                values: new object[,]
                {
                    { 629, "/images/css/CategoryImages/furniture.png", 6, "Furniture" },
                    { 628, "/images/css/CategoryImages/stationary.png", 6, "Stationary" },
                    { 627, "/images/css/CategoryImages/food.png", 6, "Food" },
                    { 626, "/images/css/CategoryImages/safety.png", 6, "Safety" },
                    { 625, "/images/css/CategoryImages/appliances.png", 6, "Electric Appliances" },
                    { 624, "/images/css/CategoryImages/business_advice.png", 6, "Business Advice" },
                    { 623, null, 6, "Brokerage" },
                    { 622, "/images/css/CategoryImages/sciemtific_advice.png", 6, "Scientific Advice" },
                    { 621, "/images/css/CategoryImages/flight_tickets.png", 6, "Flight Tickets" },
                    { 620, "/images/css/CategoryImages/hotels.png", 6, "Hotels" },
                    { 619, "/images/css/CategoryImages/software.png", 6, "Software" },
                    { 617, "/images/css/CategoryImages/insurance.png", 6, "Insurance" },
                    { 616, "/images/css/CategoryImages/company_events.png", 6, "Company Events" },
                    { 615, "/images/css/CategoryImages/conference.png", 6, "Conference" },
                    { 614, "/images/css/CategoryImages/clinical_regulation.png", 6, "Clinical Regulation" },
                    { 613, "/images/css/CategoryImages/regulations.png", 6, "Regulations" },
                    { 612, "/images/css/CategoryImages/books.png", 6, "Books And Journal" },
                    { 611, "/images/css/CategoryImages/taxes.png", 6, "Tax" },
                    { 610, "/images/css/CategoryImages/legal.png", 6, "Law Advisement" },
                    { 609, "/images/css/CategoryImages/bookeeping.png", 6, "Bookkeeping" },
                    { 608, "/images/css/CategoryImages/renovation.png", 6, "Rennovation" },
                    { 607, "/images/css/CategoryImages/transportation.png", 6, "Transportation" },
                    { 606, "/images/css/CategoryImages/shippment.png", 6, "Shipment" },
                    { 618, "/images/css/CategoryImages/general.png", 6, "General" },
                    { 605, "/images/css/CategoryImages/travel.png", 6, "Travel" }
                });
        }
    }
}

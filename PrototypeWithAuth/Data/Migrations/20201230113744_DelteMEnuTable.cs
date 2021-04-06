using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class DelteMEnuTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    menuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MenuViewName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.menuID);
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "menuID", "ActionName", "ControllerName", "MenuDescription", "MenuImageURL", "MenuViewName" },
                values: new object[,]
                {
                    { 1, "Index", "Requests", "Requests", "/images/css/main_menu_icons/inventory.png", "Orders & Inventory" },
                    { 2, "", "", "Protocols", "/images/css/main_menu_icons/protocols.png", "Protocols" },
                    { 3, "Index", "Operations", "Operations", "/images/css/main_menu_icons/operation.png", "Operation" },
                    { 4, "", "", "Biomarkers", "/images/css/main_menu_icons/biomarkers.png", "Biomarkers" },
                    { 5, "ReportHours", "Timekeeper", "TimeKeeper", "/images/css/main_menu_icons/timekeeper.png", "Timekeeper" },
                    { 6, "IndexForPayment", "Vendors", "LabManagement", "/images/css/main_menu_icons/lab.png", "Lab Management" },
                    { 7, "AccountingPayments", "Requests", "Accounting", "/images/css/main_menu_icons/accounting.png", "Accounting" },
                    { 8, "SummaryPieCharts", "Expenses", "Reports", "/images/css/main_menu_icons/expenses.png", "Reports" },
                    { 9, "", "", "Income", "/images/css/main_menu_icons/income.png", "Income" },
                    { 10, "Index", "Admin", "Users", "/images/css/main_menu_icons/users.png", "Users" }
                });
        }
    }
}

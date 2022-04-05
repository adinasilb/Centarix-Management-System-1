using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddTimePeriodsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimePeriods",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    DescriptionEnum = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimePeriods", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "TimePeriods",
                columns: new[] { "ID", "Description", "DescriptionEnum" },
                values: new object[] { 1, "Days", "Days" });

            migrationBuilder.InsertData(
                table: "TimePeriods",
                columns: new[] { "ID", "Description", "DescriptionEnum" },
                values: new object[] { 2, "Weeks", "Weeks" });

            migrationBuilder.InsertData(
                table: "TimePeriods",
                columns: new[] { "ID", "Description", "DescriptionEnum" },
                values: new object[] { 3, "Months", "Months" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimePeriods");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedCentarixIDsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CentarixIDs",
                columns: table => new
                {
                    CentarixIDID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserID = table.Column<string>(nullable: true),
                    CentarixIDNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CentarixIDs", x => x.CentarixIDID);
                    table.ForeignKey(
                        name: "FK_CentarixIDs_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 1,
                column: "Abbreviation",
                value: "E");

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 2,
                column: "Abbreviation",
                value: "F");

            migrationBuilder.CreateIndex(
                name: "IX_CentarixIDs_ApplicationUserID",
                table: "CentarixIDs",
                column: "ApplicationUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CentarixIDs");

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 1,
                column: "Abbreviation",
                value: null);

            migrationBuilder.UpdateData(
                table: "EmployeeStatuses",
                keyColumn: "EmployeeStatusID",
                keyValue: 2,
                column: "Abbreviation",
                value: null);
        }
    }
}

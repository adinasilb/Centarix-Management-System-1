using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FKPartialOffEHAA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHours_PartialOffDayTypes_PartialOffDayTypeID",
                table: "EmployeeHours");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_PartialOffDayTypes_PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.DropTable(
                name: "PartialOffDayTypes");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHours_OffDayTypes_PartialOffDayTypeID",
                table: "EmployeeHours",
                column: "PartialOffDayTypeID",
                principalTable: "OffDayTypes",
                principalColumn: "OffDayTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_OffDayTypes_PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "PartialOffDayTypeID",
                principalTable: "OffDayTypes",
                principalColumn: "OffDayTypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHours_OffDayTypes_PartialOffDayTypeID",
                table: "EmployeeHours");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_OffDayTypes_PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals");

            migrationBuilder.CreateTable(
                name: "PartialOffDayTypes",
                columns: table => new
                {
                    PartialOffDayTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartialOffDayTypes", x => x.PartialOffDayTypeID);
                });

            migrationBuilder.InsertData(
                table: "PartialOffDayTypes",
                columns: new[] { "PartialOffDayTypeID", "Description" },
                values: new object[] { 1, "Partial Sick Day" });

            migrationBuilder.InsertData(
                table: "PartialOffDayTypes",
                columns: new[] { "PartialOffDayTypeID", "Description" },
                values: new object[] { 2, "Partial Vacation Day" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHours_PartialOffDayTypes_PartialOffDayTypeID",
                table: "EmployeeHours",
                column: "PartialOffDayTypeID",
                principalTable: "PartialOffDayTypes",
                principalColumn: "PartialOffDayTypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeHoursAwaitingApprovals_PartialOffDayTypes_PartialOffDayTypeID",
                table: "EmployeeHoursAwaitingApprovals",
                column: "PartialOffDayTypeID",
                principalTable: "PartialOffDayTypes",
                principalColumn: "PartialOffDayTypeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

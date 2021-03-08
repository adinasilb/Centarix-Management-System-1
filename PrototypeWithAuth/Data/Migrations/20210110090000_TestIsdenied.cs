using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class TestIsdenied : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {     
            migrationBuilder.UpdateData("EmployeeHoursAwaitingApprovals", "IsDenied", null, "IsDenied", false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

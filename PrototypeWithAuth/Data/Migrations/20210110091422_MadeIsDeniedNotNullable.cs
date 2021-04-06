using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MadeIsDeniedNotNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDenied",
                table: "EmployeeHoursAwaitingApprovals",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDenied",
                table: "EmployeeHoursAwaitingApprovals",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldDefaultValue: false);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ChangeCompanyIDtoCompanyNoLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLocationID",
                table: "LocationInstances");

            migrationBuilder.AddColumn<int>(
                name: "CompanyLocationNo",
                table: "LocationInstances",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyLocationNo",
                table: "LocationInstances");

            migrationBuilder.AddColumn<int>(
                name: "CompanyLocationID",
                table: "LocationInstances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddParenLocationInstanceIDToRLI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentLocationInstanceID",
                table: "RequestLocationInstance",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RequestLocationInstance_ParentLocationInstanceID",
                table: "RequestLocationInstance",
                column: "ParentLocationInstanceID");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLocationInstance_LocationInstances_ParentLocationInstanceID",
                table: "RequestLocationInstance",
                column: "ParentLocationInstanceID",
                principalTable: "LocationInstances",
                principalColumn: "LocationInstanceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestLocationInstance_LocationInstances_ParentLocationInstanceID",
                table: "RequestLocationInstance");

            migrationBuilder.DropIndex(
                name: "IX_RequestLocationInstance_ParentLocationInstanceID",
                table: "RequestLocationInstance");

            migrationBuilder.DropColumn(
                name: "ParentLocationInstanceID",
                table: "RequestLocationInstance");
        }
    }
}

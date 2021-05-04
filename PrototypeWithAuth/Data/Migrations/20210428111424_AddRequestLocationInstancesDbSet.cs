using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddRequestLocationInstancesDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestLocationInstance_LocationInstances_LocationInstanceID",
                table: "RequestLocationInstance");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestLocationInstance_LocationInstances_ParentLocationInstanceID",
                table: "RequestLocationInstance");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestLocationInstance_Requests_RequestID",
                table: "RequestLocationInstance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestLocationInstance",
                table: "RequestLocationInstance");

            migrationBuilder.RenameTable(
                name: "RequestLocationInstance",
                newName: "RequestLocationInstances");

            migrationBuilder.RenameIndex(
                name: "IX_RequestLocationInstance_ParentLocationInstanceID",
                table: "RequestLocationInstances",
                newName: "IX_RequestLocationInstances_ParentLocationInstanceID");

            migrationBuilder.RenameIndex(
                name: "IX_RequestLocationInstance_LocationInstanceID",
                table: "RequestLocationInstances",
                newName: "IX_RequestLocationInstances_LocationInstanceID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestLocationInstances",
                table: "RequestLocationInstances",
                columns: new[] { "RequestID", "LocationInstanceID" });

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLocationInstances_LocationInstances_LocationInstanceID",
                table: "RequestLocationInstances",
                column: "LocationInstanceID",
                principalTable: "LocationInstances",
                principalColumn: "LocationInstanceID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLocationInstances_LocationInstances_ParentLocationInstanceID",
                table: "RequestLocationInstances",
                column: "ParentLocationInstanceID",
                principalTable: "LocationInstances",
                principalColumn: "LocationInstanceID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLocationInstances_Requests_RequestID",
                table: "RequestLocationInstances",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestLocationInstances_LocationInstances_LocationInstanceID",
                table: "RequestLocationInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestLocationInstances_LocationInstances_ParentLocationInstanceID",
                table: "RequestLocationInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestLocationInstances_Requests_RequestID",
                table: "RequestLocationInstances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RequestLocationInstances",
                table: "RequestLocationInstances");

            migrationBuilder.RenameTable(
                name: "RequestLocationInstances",
                newName: "RequestLocationInstance");

            migrationBuilder.RenameIndex(
                name: "IX_RequestLocationInstances_ParentLocationInstanceID",
                table: "RequestLocationInstance",
                newName: "IX_RequestLocationInstance_ParentLocationInstanceID");

            migrationBuilder.RenameIndex(
                name: "IX_RequestLocationInstances_LocationInstanceID",
                table: "RequestLocationInstance",
                newName: "IX_RequestLocationInstance_LocationInstanceID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RequestLocationInstance",
                table: "RequestLocationInstance",
                columns: new[] { "RequestID", "LocationInstanceID" });

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLocationInstance_LocationInstances_LocationInstanceID",
                table: "RequestLocationInstance",
                column: "LocationInstanceID",
                principalTable: "LocationInstances",
                principalColumn: "LocationInstanceID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLocationInstance_LocationInstances_ParentLocationInstanceID",
                table: "RequestLocationInstance",
                column: "ParentLocationInstanceID",
                principalTable: "LocationInstances",
                principalColumn: "LocationInstanceID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestLocationInstance_Requests_RequestID",
                table: "RequestLocationInstance",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

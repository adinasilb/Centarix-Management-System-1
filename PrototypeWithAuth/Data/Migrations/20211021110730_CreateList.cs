using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class CreateList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "VendorBuisnessID",
            //    table: "Vendors",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(max)");

            //migrationBuilder.CreateTable(
            //    name: "FavoriteReports",
            //    columns: table => new
            //    {
            //        FavoriteID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ApplicationUserID = table.Column<string>(nullable: true),
            //        ReportID = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FavoriteReports", x => x.FavoriteID);
            //        table.ForeignKey(
            //            name: "FK_FavoriteReports_AspNetUsers_ApplicationUserID",
            //            column: x => x.ApplicationUserID,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_FavoriteReports_Reports_ReportID",
            //            column: x => x.ReportID,
            //            principalTable: "Reports",
            //            principalColumn: "ReportID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateTable(
                name: "Lists",
                columns: table => new
                {
                    ListID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    ApplicationUserOwnerID = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lists", x => x.ListID);
                    table.ForeignKey(
                        name: "FK_Lists_AspNetUsers_ApplicationUserOwnerID",
                        column: x => x.ApplicationUserOwnerID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Vendors_VendorCountry_VendorBuisnessID",
            //    table: "Vendors",
            //    columns: new[] { "VendorCountry", "VendorBuisnessID" },
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_FavoriteReports_ApplicationUserID",
            //    table: "FavoriteReports",
            //    column: "ApplicationUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FavoriteReports_ReportID",
            //    table: "FavoriteReports",
            //    column: "ReportID");

            migrationBuilder.CreateIndex(
                name: "IX_Lists_ApplicationUserOwnerID",
                table: "Lists",
                column: "ApplicationUserOwnerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "FavoriteReports");

            migrationBuilder.DropTable(
                name: "Lists");

            //migrationBuilder.DropIndex(
            //    name: "IX_Vendors_VendorCountry_VendorBuisnessID",
            //    table: "Vendors");

            //migrationBuilder.AlterColumn<string>(
            //    name: "VendorBuisnessID",
            //    table: "Vendors",
            //    type: "nvarchar(max)",
            //    nullable: false,
            //    oldClrType: typeof(string));
        }
    }
}

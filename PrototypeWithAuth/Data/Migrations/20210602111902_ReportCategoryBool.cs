using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class ReportCategoryBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<bool>(
            //    name: "IsReportsCategory",
            //    table: "ResourceCategories",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "DateCreated",
            //    table: "Reports",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.AddColumn<int>(
            //    name: "ReportCategoryID",
            //    table: "Reports",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "ReportNumber",
            //    table: "Reports",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "ReportTitle",
            //    table: "Reports",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "ReportTypeID",
            //    table: "Reports",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "ReportSections",
            //    columns: table => new
            //    {
            //        ReportSectionID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ReportSectionContent = table.Column<string>(nullable: true),
            //        ReportID = table.Column<int>(nullable: false),
            //        SectionNumber = table.Column<int>(nullable: false),
            //        Discriminator = table.Column<string>(nullable: false),
            //        ProtocolID = table.Column<int>(nullable: true),
            //        RequestID = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ReportSections", x => x.ReportSectionID);
            //        table.ForeignKey(
            //            name: "FK_ReportSections_Protocols_ProtocolID",
            //            column: x => x.ProtocolID,
            //            principalTable: "Protocols",
            //            principalColumn: "ProtocolID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ReportSections_Reports_ReportID",
            //            column: x => x.ReportID,
            //            principalTable: "Reports",
            //            principalColumn: "ReportID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ReportSections_Requests_RequestID",
            //            column: x => x.RequestID,
            //            principalTable: "Requests",
            //            principalColumn: "RequestID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ReportTypes",
            //    columns: table => new
            //    {
            //        ReportTypeID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ReportTypeDescription = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ReportTypes", x => x.ReportTypeID);
            //    });

            //migrationBuilder.InsertData(
            //    table: "ReportTypes",
            //    columns: new[] { "ReportTypeID", "ReportTypeDescription" },
            //    values: new object[,]
            //    {
            //        { 1, "Daily" },
            //        { 2, "Weekly" },
            //        { 3, "Monthly" }
            //    });

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 1,
            //    column: "IsReportsCategory",
            //    value: true);

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 2,
            //    column: "IsReportsCategory",
            //    value: true);

            //migrationBuilder.UpdateData(
            //    table: "ResourceCategories",
            //    keyColumn: "ResourceCategoryID",
            //    keyValue: 3,
            //    column: "IsReportsCategory",
            //    value: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Reports_ReportCategoryID",
            //    table: "Reports",
            //    column: "ReportCategoryID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Reports_ReportTypeID",
            //    table: "Reports",
            //    column: "ReportTypeID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ReportSections_ProtocolID",
            //    table: "ReportSections",
            //    column: "ProtocolID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ReportSections_ReportID",
            //    table: "ReportSections",
            //    column: "ReportID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ReportSections_RequestID",
            //    table: "ReportSections",
            //    column: "RequestID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Reports_ResourceCategories_ReportCategoryID",
            //    table: "Reports",
            //    column: "ReportCategoryID",
            //    principalTable: "ResourceCategories",
            //    principalColumn: "ResourceCategoryID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Reports_ReportTypes_ReportTypeID",
            //    table: "Reports",
            //    column: "ReportTypeID",
            //    principalTable: "ReportTypes",
            //    principalColumn: "ReportTypeID",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Reports_ResourceCategories_ReportCategoryID",
            //    table: "Reports");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Reports_ReportTypes_ReportTypeID",
            //    table: "Reports");

            //migrationBuilder.DropTable(
            //    name: "ReportSections");

            //migrationBuilder.DropTable(
            //    name: "ReportTypes");

            //migrationBuilder.DropIndex(
            //    name: "IX_Reports_ReportCategoryID",
            //    table: "Reports");

            //migrationBuilder.DropIndex(
            //    name: "IX_Reports_ReportTypeID",
            //    table: "Reports");

            //migrationBuilder.DropColumn(
            //    name: "IsReportsCategory",
            //    table: "ResourceCategories");

            //migrationBuilder.DropColumn(
            //    name: "DateCreated",
            //    table: "Reports");

            //migrationBuilder.DropColumn(
            //    name: "ReportCategoryID",
            //    table: "Reports");

            //migrationBuilder.DropColumn(
            //    name: "ReportNumber",
            //    table: "Reports");

            //migrationBuilder.DropColumn(
            //    name: "ReportTitle",
            //    table: "Reports");

            //migrationBuilder.DropColumn(
            //    name: "ReportTypeID",
            //    table: "Reports");
        }
    }
}

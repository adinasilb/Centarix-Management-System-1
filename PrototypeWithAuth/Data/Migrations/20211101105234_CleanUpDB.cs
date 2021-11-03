using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class CleanUpDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Tests_Experiments_ExperimentID",
            //    table: "Tests");

            //migrationBuilder.DropTable(
            //    name: "TestFieldHeaders");

            //migrationBuilder.DropIndex(
            //    name: "IX_Vendors_VendorCountry_VendorBuisnessID",
            //    table: "Vendors");

            //migrationBuilder.DropIndex(
            //    name: "IX_Tests_ExperimentID",
            //    table: "Tests");

            //migrationBuilder.DropColumn(
            //    name: "VendorCountry",
            //    table: "Vendors");

            //migrationBuilder.DropColumn(
            //    name: "ExperimentID",
            //    table: "Tests");

            //migrationBuilder.AddColumn<int>(
            //    name: "CountryID",
            //    table: "Vendors",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AlterColumn<int>(
            //    name: "TestCategoryID",
            //    table: "Tests",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AddColumn<int>(
            //    name: "SiteID",
            //    table: "Tests",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "City",
            //    table: "Sites",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Country",
            //    table: "Sites",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "Line1Address",
            //    table: "Sites",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "PhoneNumber",
            //    table: "Sites",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "PrimaryContactID",
            //    table: "Sites",
            //    nullable: true);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "TimeStamp",
            //    table: "ShareResources",
            //    nullable: false,
            //    defaultValueSql: "getdate()",
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime2");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "TimeStamp",
            //    table: "ShareRequests",
            //    nullable: false,
            //    defaultValueSql: "getdate()",
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime2");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "TimeStamp",
            //    table: "ShareProtocols",
            //    nullable: false,
            //    defaultValueSql: "getdate()",
            //    oldClrType: typeof(DateTime),
            //    oldType: "datetime2");

            //migrationBuilder.AddColumn<int>(
            //    name: "AmountOfVisits",
            //    table: "Experiments",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "Countries",
            //    columns: table => new
            //    {
            //        CountryID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CountryName = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Countries", x => x.CountryID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ExperimentEntries",
            //    columns: table => new
            //    {
            //        ExperimentEntryID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        DateTime = table.Column<DateTime>(nullable: false),
            //        ParticipantID = table.Column<int>(nullable: false),
            //        VisitNumber = table.Column<int>(nullable: false),
            //        ApplicationUserID = table.Column<string>(nullable: true),
            //        SiteID = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ExperimentEntries", x => x.ExperimentEntryID);
            //        table.ForeignKey(
            //            name: "FK_ExperimentEntries_AspNetUsers_ApplicationUserID",
            //            column: x => x.ApplicationUserID,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ExperimentEntries_Participants_ParticipantID",
            //            column: x => x.ParticipantID,
            //            principalTable: "Participants",
            //            principalColumn: "ParticipantID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ExperimentEntries_Sites_SiteID",
            //            column: x => x.SiteID,
            //            principalTable: "Sites",
            //            principalColumn: "SiteID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ExperimentTest",
            //    columns: table => new
            //    {
            //        ExperimentID = table.Column<int>(nullable: false),
            //        TestID = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ExperimentTest", x => new { x.ExperimentID, x.TestID });
            //        table.ForeignKey(
            //            name: "FK_ExperimentTest_Experiments_ExperimentID",
            //            column: x => x.ExperimentID,
            //            principalTable: "Experiments",
            //            principalColumn: "ExperimentID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ExperimentTest_Tests_TestID",
            //            column: x => x.TestID,
            //            principalTable: "Tests",
            //            principalColumn: "TestID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "OldVendorCountries",
            //    columns: table => new
            //    {
            //        OldVendorCountryID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        VendorID = table.Column<int>(nullable: false),
            //        OldVendorCountryName = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_OldVendorCountries", x => x.OldVendorCountryID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TestOuterGroups",
            //    columns: table => new
            //    {
            //        TestOuterGroupID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        IsNone = table.Column<bool>(nullable: false),
            //        TestID = table.Column<int>(nullable: false),
            //        SequencePosition = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TestOuterGroups", x => x.TestOuterGroupID);
            //        table.ForeignKey(
            //            name: "FK_TestOuterGroups_Tests_TestID",
            //            column: x => x.TestID,
            //            principalTable: "Tests",
            //            principalColumn: "TestID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TestGroups",
            //    columns: table => new
            //    {
            //        TestGroupID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(nullable: true),
            //        IsNone = table.Column<bool>(nullable: false),
            //        TestOuterGroupID = table.Column<int>(nullable: false),
            //        SequencePosition = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TestGroups", x => x.TestGroupID);
            //        table.ForeignKey(
            //            name: "FK_TestGroups_TestOuterGroups_TestOuterGroupID",
            //            column: x => x.TestOuterGroupID,
            //            principalTable: "TestOuterGroups",
            //            principalColumn: "TestOuterGroupID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TestHeaders",
            //    columns: table => new
            //    {
            //        TestHeaderID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TestGroupID = table.Column<int>(nullable: false),
            //        Name = table.Column<string>(nullable: true),
            //        Type = table.Column<string>(nullable: true),
            //        List = table.Column<string>(nullable: true),
            //        SequencePosition = table.Column<int>(nullable: false),
            //        IsSkip = table.Column<bool>(nullable: false),
            //        Calculation = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TestHeaders", x => x.TestHeaderID);
            //        table.ForeignKey(
            //            name: "FK_TestHeaders_TestGroups_TestGroupID",
            //            column: x => x.TestGroupID,
            //            principalTable: "TestGroups",
            //            principalColumn: "TestGroupID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TestValues",
            //    columns: table => new
            //    {
            //        TestValueID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Value = table.Column<string>(nullable: true),
            //        TestHeaderID = table.Column<int>(nullable: false),
            //        ExperimentEntryID = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TestValues", x => x.TestValueID);
            //        table.ForeignKey(
            //            name: "FK_TestValues_ExperimentEntries_ExperimentEntryID",
            //            column: x => x.ExperimentEntryID,
            //            principalTable: "ExperimentEntries",
            //            principalColumn: "ExperimentEntryID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_TestValues_TestHeaders_TestHeaderID",
            //            column: x => x.TestHeaderID,
            //            principalTable: "TestHeaders",
            //            principalColumn: "TestHeaderID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.InsertData(
            //    table: "Countries",
            //    columns: new[] { "CountryID", "CountryName" },
            //    values: new object[,]
            //    {
            //        { 1, "Armenia" },
            //        { 28, "Luxembourg" },
            //        { 29, "Malaysia" },
            //        { 30, "Mauritius" },
            //        { 31, "Moldova" },
            //        { 32, "Netherlands" },
            //        { 33, "New Zealand" },
            //        { 34, "North Macedonia" },
            //        { 35, "Norway" },
            //        { 36, "Poland" },
            //        { 37, "Portugal" },
            //        { 27, "Lithuania" },
            //        { 38, "Romania" },
            //        { 40, "Singapore" },
            //        { 41, "Slovakia" },
            //        { 42, "Slovenia" },
            //        { 43, "South Africa" },
            //        { 44, "South Korea" },
            //        { 45, "Spain" },
            //        { 46, "Sweden" },
            //        { 47, "Switzerland" },
            //        { 48, "United Kingdom" },
            //        { 49, "United States" },
            //        { 39, "Russia" },
            //        { 50, "Uruguay" },
            //        { 26, "Latvia" },
            //        { 24, "Italy" },
            //        { 2, "Australia" },
            //        { 3, "Austria" },
            //        { 4, "Belgium" },
            //        { 5, "Bosnia and Herzegovina" },
            //        { 6, "Bulgaria" },
            //        { 7, "Canada" },
            //        { 8, "Chile" },
            //        { 9, "Costa Rica" },
            //        { 10, "Cyprus" },
            //        { 11, "Czech Republic" },
            //        { 25, "Japan" },
            //        { 12, "Denmark" },
            //        { 14, "Finland" },
            //        { 15, "France" },
            //        { 16, "Georgia" },
            //        { 17, "Germany" },
            //        { 18, "Greece" },
            //        { 19, "Hungary" },
            //        { 20, "Iceland" },
            //        { 21, "India" },
            //        { 22, "Ireland" },
            //        { 23, "Israel" },
            //        { 13, "Estonia" }
            //    });

            //migrationBuilder.UpdateData(
            //    table: "LabParts",
            //    keyColumn: "LabPartID",
            //    keyValue: 4,
            //    column: "HasShelves",
            //    value: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Vendors_CountryID_VendorBuisnessID",
            //    table: "Vendors",
            //    columns: new[] { "CountryID", "VendorBuisnessID" },
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tests_SiteID",
            //    table: "Tests",
            //    column: "SiteID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Sites_PrimaryContactID",
            //    table: "Sites",
            //    column: "PrimaryContactID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ExperimentEntries_ApplicationUserID",
            //    table: "ExperimentEntries",
            //    column: "ApplicationUserID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ExperimentEntries_SiteID",
            //    table: "ExperimentEntries",
            //    column: "SiteID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ExperimentEntries_ParticipantID_VisitNumber",
            //    table: "ExperimentEntries",
            //    columns: new[] { "ParticipantID", "VisitNumber" },
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_ExperimentTest_TestID",
            //    table: "ExperimentTest",
            //    column: "TestID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TestGroups_TestOuterGroupID",
            //    table: "TestGroups",
            //    column: "TestOuterGroupID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TestHeaders_TestGroupID",
            //    table: "TestHeaders",
            //    column: "TestGroupID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TestHeaders_SequencePosition_TestGroupID",
            //    table: "TestHeaders",
            //    columns: new[] { "SequencePosition", "TestGroupID" },
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_TestOuterGroups_TestID",
            //    table: "TestOuterGroups",
            //    column: "TestID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TestValues_ExperimentEntryID",
            //    table: "TestValues",
            //    column: "ExperimentEntryID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TestValues_TestHeaderID",
            //    table: "TestValues",
            //    column: "TestHeaderID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Sites_AspNetUsers_PrimaryContactID",
            //    table: "Sites",
            //    column: "PrimaryContactID",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Tests_Sites_SiteID",
            //    table: "Tests",
            //    column: "SiteID",
            //    principalTable: "Sites",
            //    principalColumn: "SiteID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Vendors_Countries_CountryID",
            //    table: "Vendors",
            //    column: "CountryID",
            //    principalTable: "Countries",
            //    principalColumn: "CountryID",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Sites_AspNetUsers_PrimaryContactID",
            //    table: "Sites");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Tests_Sites_SiteID",
            //    table: "Tests");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Vendors_Countries_CountryID",
            //    table: "Vendors");

            //migrationBuilder.DropTable(
            //    name: "Countries");

            //migrationBuilder.DropTable(
            //    name: "ExperimentTest");

            //migrationBuilder.DropTable(
            //    name: "OldVendorCountries");

            //migrationBuilder.DropTable(
            //    name: "TestValues");

            //migrationBuilder.DropTable(
            //    name: "ExperimentEntries");

            //migrationBuilder.DropTable(
            //    name: "TestHeaders");

            //migrationBuilder.DropTable(
            //    name: "TestGroups");

            //migrationBuilder.DropTable(
            //    name: "TestOuterGroups");

            //migrationBuilder.DropIndex(
            //    name: "IX_Vendors_CountryID_VendorBuisnessID",
            //    table: "Vendors");

            //migrationBuilder.DropIndex(
            //    name: "IX_Tests_SiteID",
            //    table: "Tests");

            //migrationBuilder.DropIndex(
            //    name: "IX_Sites_PrimaryContactID",
            //    table: "Sites");

            //migrationBuilder.DropColumn(
            //    name: "CountryID",
            //    table: "Vendors");

            //migrationBuilder.DropColumn(
            //    name: "SiteID",
            //    table: "Tests");

            //migrationBuilder.DropColumn(
            //    name: "City",
            //    table: "Sites");

            //migrationBuilder.DropColumn(
            //    name: "Country",
            //    table: "Sites");

            //migrationBuilder.DropColumn(
            //    name: "Line1Address",
            //    table: "Sites");

            //migrationBuilder.DropColumn(
            //    name: "PhoneNumber",
            //    table: "Sites");

            //migrationBuilder.DropColumn(
            //    name: "PrimaryContactID",
            //    table: "Sites");

            //migrationBuilder.DropColumn(
            //    name: "AmountOfVisits",
            //    table: "Experiments");

            //migrationBuilder.AddColumn<string>(
            //    name: "VendorCountry",
            //    table: "Vendors",
            //    type: "nvarchar(50)",
            //    maxLength: 50,
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AlterColumn<int>(
            //    name: "TestCategoryID",
            //    table: "Tests",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldNullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "ExperimentID",
            //    table: "Tests",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "TimeStamp",
            //    table: "ShareResources",
            //    type: "datetime2",
            //    nullable: false,
            //    oldClrType: typeof(DateTime),
            //    oldDefaultValueSql: "getdate()");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "TimeStamp",
            //    table: "ShareRequests",
            //    type: "datetime2",
            //    nullable: false,
            //    oldClrType: typeof(DateTime),
            //    oldDefaultValueSql: "getdate()");

            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "TimeStamp",
            //    table: "ShareProtocols",
            //    type: "datetime2",
            //    nullable: false,
            //    oldClrType: typeof(DateTime),
            //    oldDefaultValueSql: "getdate()");

            //migrationBuilder.CreateTable(
            //    name: "TestFieldHeaders",
            //    columns: table => new
            //    {
            //        TestFieldHeaderID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FieldList = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FieldNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        FieldTypes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TestID = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TestFieldHeaders", x => x.TestFieldHeaderID);
            //        table.ForeignKey(
            //            name: "FK_TestFieldHeaders_Tests_TestID",
            //            column: x => x.TestID,
            //            principalTable: "Tests",
            //            principalColumn: "TestID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.UpdateData(
            //    table: "LabParts",
            //    keyColumn: "LabPartID",
            //    keyValue: 4,
            //    column: "HasShelves",
            //    value: false);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Vendors_VendorCountry_VendorBuisnessID",
            //    table: "Vendors",
            //    columns: new[] { "VendorCountry", "VendorBuisnessID" },
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Tests_ExperimentID",
            //    table: "Tests",
            //    column: "ExperimentID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_TestFieldHeaders_TestID",
            //    table: "TestFieldHeaders",
            //    column: "TestID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Tests_Experiments_ExperimentID",
            //    table: "Tests",
            //    column: "ExperimentID",
            //    principalTable: "Experiments",
            //    principalColumn: "ExperimentID",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}

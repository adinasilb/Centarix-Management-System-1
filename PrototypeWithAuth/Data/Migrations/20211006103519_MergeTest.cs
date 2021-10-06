using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class MergeTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_FavoriteProtocols_Protocols_ProtocolID",
            //    table: "FavoriteProtocols");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_FunctionLines_Protocols_ProtocolID",
            //    table: "FunctionLines");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_FunctionReports_Protocols_ProtocolID",
            //    table: "FunctionReports");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Lines_Protocols_ProtocolID",
            //    table: "Lines");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Links_Protocols_ProtocolID",
            //    table: "Links");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Materials_Protocols_ProtocolID",
            //    table: "Materials");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProtocolInstances_Protocols_ProtocolID",
            //    table: "ProtocolInstances");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Protocols_AspNetUsers_ApplicationUserCreatorID",
            //    table: "Protocols");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ShareProtocols_Protocols_ProtocolID",
            //    table: "ShareProtocols");

            //migrationBuilder.DropIndex(
            //    name: "IX_ShareProtocols_ProtocolID",
            //    table: "ShareProtocols");

            //migrationBuilder.DropIndex(
            //    name: "IX_Protocols_ApplicationUserCreatorID",
            //    table: "Protocols");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProtocolInstances_ProtocolID",
            //    table: "ProtocolInstances");

            //migrationBuilder.DropIndex(
            //    name: "IX_Materials_ProtocolID",
            //    table: "Materials");

            //migrationBuilder.DropIndex(
            //    name: "IX_Links_ProtocolID",
            //    table: "Links");

            //migrationBuilder.DropIndex(
            //    name: "IX_Lines_ProtocolID",
            //    table: "Lines");

            //migrationBuilder.DropIndex(
            //    name: "IX_FunctionReports_ProtocolID",
            //    table: "FunctionReports");

            //migrationBuilder.DropIndex(
            //    name: "IX_FunctionLines_ProtocolID",
            //    table: "FunctionLines");

            //migrationBuilder.DropIndex(
            //    name: "IX_FavoriteProtocols_ProtocolID",
            //    table: "FavoriteProtocols");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolID",
            //    table: "ShareProtocols");

            //migrationBuilder.DropColumn(
            //    name: "ApplicationUserCreatorID",
            //    table: "Protocols");

            //migrationBuilder.DropColumn(
            //    name: "CreationDate",
            //    table: "Protocols");

            //migrationBuilder.DropColumn(
            //    name: "VersionNumber",
            //    table: "Protocols");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolID",
            //    table: "ProtocolInstances");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolID",
            //    table: "Materials");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolID",
            //    table: "Links");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolID",
            //    table: "Lines");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolID",
            //    table: "FunctionReports");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolID",
            //    table: "FunctionLines");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolID",
            //    table: "FavoriteProtocols");

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolVersionID",
            //    table: "ShareProtocols",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolVersionID",
            //    table: "ProtocolInstances",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolVersionID",
            //    table: "Materials",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolVersionID",
            //    table: "Links",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolVersionID",
            //    table: "Lines",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "Description",
            //    table: "FunctionReports",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolVersionID",
            //    table: "FunctionReports",
            //    nullable: true);

            //migrationBuilder.AddColumn<TimeSpan>(
            //    name: "Timer",
            //    table: "FunctionReports",
            //    nullable: false,
            //    defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolVersionID",
            //    table: "FunctionLines",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolVersionID",
            //    table: "FavoriteProtocols",
            //    nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "ProtocolVersions",
            //    columns: table => new
            //    {
            //        ProtocolVersionID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        VersionNumber = table.Column<int>(nullable: false),
            //        ShortDescription = table.Column<string>(nullable: true),
            //        Theory = table.Column<string>(nullable: true),
            //        ApplicationUserCreatorID = table.Column<string>(nullable: true),
            //        CreationDate = table.Column<DateTime>(nullable: false),
            //        ProtocolID = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ProtocolVersions", x => x.ProtocolVersionID);
            //        table.ForeignKey(
            //            name: "FK_ProtocolVersions_AspNetUsers_ApplicationUserCreatorID",
            //            column: x => x.ApplicationUserCreatorID,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_ProtocolVersions_Protocols_ProtocolID",
            //            column: x => x.ProtocolID,
            //            principalTable: "Protocols",
            //            principalColumn: "ProtocolID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TempReportJsons",
            //    columns: table => new
            //    {
            //        TempReportJsonID = table.Column<Guid>(nullable: false),
            //        Json = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TempReportJsons", x => x.TempReportJsonID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "TempResultsJsons",
            //    columns: table => new
            //    {
            //        TempResultsJsonID = table.Column<Guid>(nullable: false),
            //        Json = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TempResultsJsons", x => x.TempResultsJsonID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "FunctionResults",
            //    columns: table => new
            //    {
            //        ID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        FunctionTypeID = table.Column<int>(nullable: false),
            //        ProtocolVersionID = table.Column<int>(nullable: true),
            //        ProductID = table.Column<int>(nullable: true),
            //        Timer = table.Column<TimeSpan>(nullable: false),
            //        Description = table.Column<string>(nullable: true),
            //        IsTemporaryDeleted = table.Column<bool>(nullable: false),
            //        ProtocolInstanceID = table.Column<int>(nullable: false),
            //        IsTemporary = table.Column<bool>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_FunctionResults", x => x.ID);
            //        table.ForeignKey(
            //            name: "FK_FunctionResults_FunctionTypes_FunctionTypeID",
            //            column: x => x.FunctionTypeID,
            //            principalTable: "FunctionTypes",
            //            principalColumn: "FunctionTypeID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_FunctionResults_Products_ProductID",
            //            column: x => x.ProductID,
            //            principalTable: "Products",
            //            principalColumn: "ProductID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_FunctionResults_ProtocolInstances_ProtocolInstanceID",
            //            column: x => x.ProtocolInstanceID,
            //            principalTable: "ProtocolInstances",
            //            principalColumn: "ProtocolInstanceID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_FunctionResults_ProtocolVersions_ProtocolVersionID",
            //            column: x => x.ProtocolVersionID,
            //            principalTable: "ProtocolVersions",
            //            principalColumn: "ProtocolVersionID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_ShareProtocols_ProtocolVersionID",
            //    table: "ShareProtocols",
            //    column: "ProtocolVersionID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProtocolInstances_ProtocolVersionID",
            //    table: "ProtocolInstances",
            //    column: "ProtocolVersionID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Materials_ProtocolVersionID",
            //    table: "Materials",
            //    column: "ProtocolVersionID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Links_ProtocolVersionID",
            //    table: "Links",
            //    column: "ProtocolVersionID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Lines_ProtocolVersionID",
            //    table: "Lines",
            //    column: "ProtocolVersionID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FunctionReports_ProtocolVersionID",
            //    table: "FunctionReports",
            //    column: "ProtocolVersionID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FunctionLines_ProtocolVersionID",
            //    table: "FunctionLines",
            //    column: "ProtocolVersionID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FavoriteProtocols_ProtocolVersionID",
            //    table: "FavoriteProtocols",
            //    column: "ProtocolVersionID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FunctionResults_FunctionTypeID",
            //    table: "FunctionResults",
            //    column: "FunctionTypeID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FunctionResults_ProductID",
            //    table: "FunctionResults",
            //    column: "ProductID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FunctionResults_ProtocolInstanceID",
            //    table: "FunctionResults",
            //    column: "ProtocolInstanceID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FunctionResults_ProtocolVersionID",
            //    table: "FunctionResults",
            //    column: "ProtocolVersionID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProtocolVersions_ApplicationUserCreatorID",
            //    table: "ProtocolVersions",
            //    column: "ApplicationUserCreatorID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProtocolVersions_ProtocolID_VersionNumber",
            //    table: "ProtocolVersions",
            //    columns: new[] { "ProtocolID", "VersionNumber" },
            //    unique: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FavoriteProtocols_ProtocolVersions_ProtocolVersionID",
            //    table: "FavoriteProtocols",
            //    column: "ProtocolVersionID",
            //    principalTable: "ProtocolVersions",
            //    principalColumn: "ProtocolVersionID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FunctionLines_ProtocolVersions_ProtocolVersionID",
            //    table: "FunctionLines",
            //    column: "ProtocolVersionID",
            //    principalTable: "ProtocolVersions",
            //    principalColumn: "ProtocolVersionID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FunctionReports_ProtocolVersions_ProtocolVersionID",
            //    table: "FunctionReports",
            //    column: "ProtocolVersionID",
            //    principalTable: "ProtocolVersions",
            //    principalColumn: "ProtocolVersionID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Lines_ProtocolVersions_ProtocolVersionID",
            //    table: "Lines",
            //    column: "ProtocolVersionID",
            //    principalTable: "ProtocolVersions",
            //    principalColumn: "ProtocolVersionID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Links_ProtocolVersions_ProtocolVersionID",
            //    table: "Links",
            //    column: "ProtocolVersionID",
            //    principalTable: "ProtocolVersions",
            //    principalColumn: "ProtocolVersionID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Materials_ProtocolVersions_ProtocolVersionID",
            //    table: "Materials",
            //    column: "ProtocolVersionID",
            //    principalTable: "ProtocolVersions",
            //    principalColumn: "ProtocolVersionID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProtocolInstances_ProtocolVersions_ProtocolVersionID",
            //    table: "ProtocolInstances",
            //    column: "ProtocolVersionID",
            //    principalTable: "ProtocolVersions",
            //    principalColumn: "ProtocolVersionID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ShareProtocols_ProtocolVersions_ProtocolVersionID",
            //    table: "ShareProtocols",
            //    column: "ProtocolVersionID",
            //    principalTable: "ProtocolVersions",
            //    principalColumn: "ProtocolVersionID",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_FavoriteProtocols_ProtocolVersions_ProtocolVersionID",
            //    table: "FavoriteProtocols");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_FunctionLines_ProtocolVersions_ProtocolVersionID",
            //    table: "FunctionLines");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_FunctionReports_ProtocolVersions_ProtocolVersionID",
            //    table: "FunctionReports");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Lines_ProtocolVersions_ProtocolVersionID",
            //    table: "Lines");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Links_ProtocolVersions_ProtocolVersionID",
            //    table: "Links");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Materials_ProtocolVersions_ProtocolVersionID",
            //    table: "Materials");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ProtocolInstances_ProtocolVersions_ProtocolVersionID",
            //    table: "ProtocolInstances");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_ShareProtocols_ProtocolVersions_ProtocolVersionID",
            //    table: "ShareProtocols");

            //migrationBuilder.DropTable(
            //    name: "FunctionResults");

            //migrationBuilder.DropTable(
            //    name: "TempReportJsons");

            //migrationBuilder.DropTable(
            //    name: "TempResultsJsons");

            //migrationBuilder.DropTable(
            //    name: "ProtocolVersions");

            //migrationBuilder.DropIndex(
            //    name: "IX_ShareProtocols_ProtocolVersionID",
            //    table: "ShareProtocols");

            //migrationBuilder.DropIndex(
            //    name: "IX_ProtocolInstances_ProtocolVersionID",
            //    table: "ProtocolInstances");

            //migrationBuilder.DropIndex(
            //    name: "IX_Materials_ProtocolVersionID",
            //    table: "Materials");

            //migrationBuilder.DropIndex(
            //    name: "IX_Links_ProtocolVersionID",
            //    table: "Links");

            //migrationBuilder.DropIndex(
            //    name: "IX_Lines_ProtocolVersionID",
            //    table: "Lines");

            //migrationBuilder.DropIndex(
            //    name: "IX_FunctionReports_ProtocolVersionID",
            //    table: "FunctionReports");

            //migrationBuilder.DropIndex(
            //    name: "IX_FunctionLines_ProtocolVersionID",
            //    table: "FunctionLines");

            //migrationBuilder.DropIndex(
            //    name: "IX_FavoriteProtocols_ProtocolVersionID",
            //    table: "FavoriteProtocols");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolVersionID",
            //    table: "ShareProtocols");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolVersionID",
            //    table: "ProtocolInstances");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolVersionID",
            //    table: "Materials");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolVersionID",
            //    table: "Links");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolVersionID",
            //    table: "Lines");

            //migrationBuilder.DropColumn(
            //    name: "Description",
            //    table: "FunctionReports");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolVersionID",
            //    table: "FunctionReports");

            //migrationBuilder.DropColumn(
            //    name: "Timer",
            //    table: "FunctionReports");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolVersionID",
            //    table: "FunctionLines");

            //migrationBuilder.DropColumn(
            //    name: "ProtocolVersionID",
            //    table: "FavoriteProtocols");

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolID",
            //    table: "ShareProtocols",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<string>(
            //    name: "ApplicationUserCreatorID",
            //    table: "Protocols",
            //    type: "nvarchar(450)",
            //    nullable: true);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "CreationDate",
            //    table: "Protocols",
            //    type: "datetime2",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.AddColumn<string>(
            //    name: "VersionNumber",
            //    table: "Protocols",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolID",
            //    table: "ProtocolInstances",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolID",
            //    table: "Materials",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolID",
            //    table: "Links",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolID",
            //    table: "Lines",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolID",
            //    table: "FunctionReports",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolID",
            //    table: "FunctionLines",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "ProtocolID",
            //    table: "FavoriteProtocols",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_ShareProtocols_ProtocolID",
            //    table: "ShareProtocols",
            //    column: "ProtocolID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Protocols_ApplicationUserCreatorID",
            //    table: "Protocols",
            //    column: "ApplicationUserCreatorID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ProtocolInstances_ProtocolID",
            //    table: "ProtocolInstances",
            //    column: "ProtocolID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Materials_ProtocolID",
            //    table: "Materials",
            //    column: "ProtocolID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Links_ProtocolID",
            //    table: "Links",
            //    column: "ProtocolID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Lines_ProtocolID",
            //    table: "Lines",
            //    column: "ProtocolID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FunctionReports_ProtocolID",
            //    table: "FunctionReports",
            //    column: "ProtocolID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FunctionLines_ProtocolID",
            //    table: "FunctionLines",
            //    column: "ProtocolID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_FavoriteProtocols_ProtocolID",
            //    table: "FavoriteProtocols",
            //    column: "ProtocolID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FavoriteProtocols_Protocols_ProtocolID",
            //    table: "FavoriteProtocols",
            //    column: "ProtocolID",
            //    principalTable: "Protocols",
            //    principalColumn: "ProtocolID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FunctionLines_Protocols_ProtocolID",
            //    table: "FunctionLines",
            //    column: "ProtocolID",
            //    principalTable: "Protocols",
            //    principalColumn: "ProtocolID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_FunctionReports_Protocols_ProtocolID",
            //    table: "FunctionReports",
            //    column: "ProtocolID",
            //    principalTable: "Protocols",
            //    principalColumn: "ProtocolID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Lines_Protocols_ProtocolID",
            //    table: "Lines",
            //    column: "ProtocolID",
            //    principalTable: "Protocols",
            //    principalColumn: "ProtocolID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Links_Protocols_ProtocolID",
            //    table: "Links",
            //    column: "ProtocolID",
            //    principalTable: "Protocols",
            //    principalColumn: "ProtocolID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Materials_Protocols_ProtocolID",
            //    table: "Materials",
            //    column: "ProtocolID",
            //    principalTable: "Protocols",
            //    principalColumn: "ProtocolID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ProtocolInstances_Protocols_ProtocolID",
            //    table: "ProtocolInstances",
            //    column: "ProtocolID",
            //    principalTable: "Protocols",
            //    principalColumn: "ProtocolID",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Protocols_AspNetUsers_ApplicationUserCreatorID",
            //    table: "Protocols",
            //    column: "ApplicationUserCreatorID",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ShareProtocols_Protocols_ProtocolID",
            //    table: "ShareProtocols",
            //    column: "ProtocolID",
            //    principalTable: "Protocols",
            //    principalColumn: "ProtocolID",
            //    onDelete: ReferentialAction.Restrict);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddShareRequestLists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "NoteToSupplier",
            //    table: "Requests");

            //migrationBuilder.AddColumn<string>(
            //    name: "NoteToSupplier",
            //    table: "ParentRequests",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "CurrencyID",
            //    table: "Countries",
            //    nullable: false,
            //    defaultValue: 0);

            //migrationBuilder.CreateTable(
            //    name: "Currencies",
            //    columns: table => new
            //    {
            //        CurrencyID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        CurrencyName = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Currencies", x => x.CurrencyID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "RequestLists",
            //    columns: table => new
            //    {
            //        ListID = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Title = table.Column<string>(nullable: true),
            //        ApplicationUserOwnerID = table.Column<string>(nullable: true),
            //        DateCreated = table.Column<DateTime>(nullable: false),
            //        IsDefault = table.Column<bool>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_RequestLists", x => x.ListID);
            //        table.ForeignKey(
            //            name: "FK_RequestLists_AspNetUsers_ApplicationUserOwnerID",
            //            column: x => x.ApplicationUserOwnerID,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "RequestListRequests",
            //    columns: table => new
            //    {
            //        ListID = table.Column<int>(nullable: false),
            //        RequestID = table.Column<int>(nullable: false),
            //        TimeStamp = table.Column<DateTime>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_RequestListRequests", x => new { x.ListID, x.RequestID });
            //        table.ForeignKey(
            //            name: "FK_RequestListRequests_RequestLists_ListID",
            //            column: x => x.ListID,
            //            principalTable: "RequestLists",
            //            principalColumn: "ListID",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_RequestListRequests_Requests_RequestID",
            //            column: x => x.RequestID,
            //            principalTable: "Requests",
            //            principalColumn: "RequestID",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateTable(
                name: "ShareRequestLists",
                columns: table => new
                {
                    ShareID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromApplicationUserID = table.Column<string>(nullable: true),
                    ToApplicationUserID = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    RequestListID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareRequestLists", x => x.ShareID);
                    table.ForeignKey(
                        name: "FK_ShareRequestLists_AspNetUsers_FromApplicationUserID",
                        column: x => x.FromApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareRequestLists_RequestLists_RequestListID",
                        column: x => x.RequestListID,
                        principalTable: "RequestLists",
                        principalColumn: "ListID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareRequestLists_AspNetUsers_ToApplicationUserID",
                        column: x => x.ToApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.InsertData(
            //    table: "Currencies",
            //    columns: new[] { "CurrencyID", "CurrencyName" },
            //    values: new object[] { -1, "None" });

            //migrationBuilder.InsertData(
            //    table: "Currencies",
            //    columns: new[] { "CurrencyID", "CurrencyName" },
            //    values: new object[] { 1, "USD" });

            //migrationBuilder.InsertData(
            //    table: "Currencies",
            //    columns: new[] { "CurrencyID", "CurrencyName" },
            //    values: new object[] { 2, "NIS" });

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 1,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 2,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 3,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 4,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 5,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 6,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 7,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 8,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 9,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 10,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 11,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 12,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 13,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 14,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 15,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 16,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 17,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 18,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 19,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 20,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 21,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 22,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 23,
            //    column: "CurrencyID",
            //    value: 2);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 24,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 25,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 26,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 27,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 28,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 29,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 30,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 31,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 32,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 33,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 34,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 35,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 36,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 37,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 38,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 39,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 40,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 41,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 42,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 43,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 44,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 45,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 46,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 47,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 48,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 49,
            //    column: "CurrencyID",
            //    value: 1);

            //migrationBuilder.UpdateData(
            //    table: "Countries",
            //    keyColumn: "CountryID",
            //    keyValue: 50,
            //    column: "CurrencyID",
            //    value: -1);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Countries_CurrencyID",
            //    table: "Countries",
            //    column: "CurrencyID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_RequestListRequests_RequestID",
            //    table: "RequestListRequests",
            //    column: "RequestID");

            //migrationBuilder.CreateIndex(
            //    name: "IX_RequestLists_ApplicationUserOwnerID",
            //    table: "RequestLists",
            //    column: "ApplicationUserOwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequestLists_FromApplicationUserID",
                table: "ShareRequestLists",
                column: "FromApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequestLists_RequestListID",
                table: "ShareRequestLists",
                column: "RequestListID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequestLists_ToApplicationUserID",
                table: "ShareRequestLists",
                column: "ToApplicationUserID");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Countries_Currencies_CurrencyID",
            //    table: "Countries",
            //    column: "CurrencyID",
            //    principalTable: "Currencies",
            //    principalColumn: "CurrencyID",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Countries_Currencies_CurrencyID",
            //    table: "Countries");

            //migrationBuilder.DropTable(
            //    name: "Currencies");

            migrationBuilder.DropTable(
                name: "RequestListRequests");

            migrationBuilder.DropTable(
                name: "ShareRequestLists");

            //migrationBuilder.DropTable(
            //    name: "RequestLists");

            //migrationBuilder.DropIndex(
            //    name: "IX_Countries_CurrencyID",
            //    table: "Countries");

            //migrationBuilder.DropColumn(
            //    name: "NoteToSupplier",
            //    table: "ParentRequests");

            //migrationBuilder.DropColumn(
            //    name: "CurrencyID",
            //    table: "Countries");

            //migrationBuilder.AddColumn<string>(
            //    name: "NoteToSupplier",
            //    table: "Requests",
            //    type: "nvarchar(max)",
            //    nullable: true);
        }
    }
}

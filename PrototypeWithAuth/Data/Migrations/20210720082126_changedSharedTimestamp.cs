using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class changedSharedTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareResources_AspNetUsers_FromApplicationUserID",
                table: "ShareResources");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareResources_Resources_ResourceID",
                table: "ShareResources");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareResources_AspNetUsers_ToApplicationUserID",
                table: "ShareResources");

            migrationBuilder.DropTable(
                name: "ShareProtocols");

            migrationBuilder.DropTable(
                name: "ShareRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareResources",
                table: "ShareResources");

            migrationBuilder.RenameTable(
                name: "ShareResources",
                newName: "ShareBase");

            migrationBuilder.RenameIndex(
                name: "IX_ShareResources_ToApplicationUserID",
                table: "ShareBase",
                newName: "IX_ShareBase_ToApplicationUserID2");

            migrationBuilder.RenameIndex(
                name: "IX_ShareResources_ResourceID",
                table: "ShareBase",
                newName: "IX_ShareBase_ResourceID");

            migrationBuilder.RenameIndex(
                name: "IX_ShareResources_FromApplicationUserID",
                table: "ShareBase",
                newName: "IX_ShareBase_FromApplicationUserID2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeStamp",
                table: "ShareBase",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "ResourceID",
                table: "ShareBase",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "ShareBase",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "ShareBase",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequestID",
                table: "ShareBase",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareBase",
                table: "ShareBase",
                column: "ShareID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareBase_FromApplicationUserID",
                table: "ShareBase",
                column: "FromApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareBase_ProtocolID",
                table: "ShareBase",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareBase_ToApplicationUserID",
                table: "ShareBase",
                column: "ToApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareBase_FromApplicationUserID1",
                table: "ShareBase",
                column: "FromApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareBase_RequestID",
                table: "ShareBase",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareBase_ToApplicationUserID1",
                table: "ShareBase",
                column: "ToApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareBase_AspNetUsers_FromApplicationUserID",
                table: "ShareBase",
                column: "FromApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareBase_Protocols_ProtocolID",
                table: "ShareBase",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareBase_AspNetUsers_ToApplicationUserID",
                table: "ShareBase",
                column: "ToApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareBase_AspNetUsers_FromApplicationUserID1",
                table: "ShareBase",
                column: "FromApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareBase_Requests_RequestID",
                table: "ShareBase",
                column: "RequestID",
                principalTable: "Requests",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareBase_AspNetUsers_ToApplicationUserID1",
                table: "ShareBase",
                column: "ToApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareBase_AspNetUsers_FromApplicationUserID2",
                table: "ShareBase",
                column: "FromApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareBase_Resources_ResourceID",
                table: "ShareBase",
                column: "ResourceID",
                principalTable: "Resources",
                principalColumn: "ResourceID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareBase_AspNetUsers_ToApplicationUserID2",
                table: "ShareBase",
                column: "ToApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareBase_AspNetUsers_FromApplicationUserID",
                table: "ShareBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareBase_Protocols_ProtocolID",
                table: "ShareBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareBase_AspNetUsers_ToApplicationUserID",
                table: "ShareBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareBase_AspNetUsers_FromApplicationUserID1",
                table: "ShareBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareBase_Requests_RequestID",
                table: "ShareBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareBase_AspNetUsers_ToApplicationUserID1",
                table: "ShareBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareBase_AspNetUsers_FromApplicationUserID2",
                table: "ShareBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareBase_Resources_ResourceID",
                table: "ShareBase");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareBase_AspNetUsers_ToApplicationUserID2",
                table: "ShareBase");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareBase",
                table: "ShareBase");

            migrationBuilder.DropIndex(
                name: "IX_ShareBase_FromApplicationUserID",
                table: "ShareBase");

            migrationBuilder.DropIndex(
                name: "IX_ShareBase_ProtocolID",
                table: "ShareBase");

            migrationBuilder.DropIndex(
                name: "IX_ShareBase_ToApplicationUserID",
                table: "ShareBase");

            migrationBuilder.DropIndex(
                name: "IX_ShareBase_FromApplicationUserID1",
                table: "ShareBase");

            migrationBuilder.DropIndex(
                name: "IX_ShareBase_RequestID",
                table: "ShareBase");

            migrationBuilder.DropIndex(
                name: "IX_ShareBase_ToApplicationUserID1",
                table: "ShareBase");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "ShareBase");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "ShareBase");

            migrationBuilder.DropColumn(
                name: "RequestID",
                table: "ShareBase");

            migrationBuilder.RenameTable(
                name: "ShareBase",
                newName: "ShareResources");

            migrationBuilder.RenameIndex(
                name: "IX_ShareBase_ToApplicationUserID2",
                table: "ShareResources",
                newName: "IX_ShareResources_ToApplicationUserID");

            migrationBuilder.RenameIndex(
                name: "IX_ShareBase_ResourceID",
                table: "ShareResources",
                newName: "IX_ShareResources_ResourceID");

            migrationBuilder.RenameIndex(
                name: "IX_ShareBase_FromApplicationUserID2",
                table: "ShareResources",
                newName: "IX_ShareResources_FromApplicationUserID");

            migrationBuilder.AlterColumn<int>(
                name: "ResourceID",
                table: "ShareResources",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeStamp",
                table: "ShareResources",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareResources",
                table: "ShareResources",
                column: "ShareID");

            migrationBuilder.CreateTable(
                name: "ShareProtocols",
                columns: table => new
                {
                    ShareID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromApplicationUserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProtocolID = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToApplicationUserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareProtocols", x => x.ShareID);
                    table.ForeignKey(
                        name: "FK_ShareProtocols_AspNetUsers_FromApplicationUserID",
                        column: x => x.FromApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareProtocols_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareProtocols_AspNetUsers_ToApplicationUserID",
                        column: x => x.ToApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ShareRequests",
                columns: table => new
                {
                    ShareID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromApplicationUserID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestID = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToApplicationUserID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareRequests", x => x.ShareID);
                    table.ForeignKey(
                        name: "FK_ShareRequests_AspNetUsers_FromApplicationUserID",
                        column: x => x.FromApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareRequests_Requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Requests",
                        principalColumn: "RequestID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ShareRequests_AspNetUsers_ToApplicationUserID",
                        column: x => x.ToApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShareProtocols_FromApplicationUserID",
                table: "ShareProtocols",
                column: "FromApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareProtocols_ProtocolID",
                table: "ShareProtocols",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareProtocols_ToApplicationUserID",
                table: "ShareProtocols",
                column: "ToApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequests_FromApplicationUserID",
                table: "ShareRequests",
                column: "FromApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequests_RequestID",
                table: "ShareRequests",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_ShareRequests_ToApplicationUserID",
                table: "ShareRequests",
                column: "ToApplicationUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareResources_AspNetUsers_FromApplicationUserID",
                table: "ShareResources",
                column: "FromApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareResources_Resources_ResourceID",
                table: "ShareResources",
                column: "ResourceID",
                principalTable: "Resources",
                principalColumn: "ResourceID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareResources_AspNetUsers_ToApplicationUserID",
                table: "ShareResources",
                column: "ToApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

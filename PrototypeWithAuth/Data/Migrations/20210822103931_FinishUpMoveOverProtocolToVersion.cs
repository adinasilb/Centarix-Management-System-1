using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FinishUpMoveOverProtocolToVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into ProtocolVersions(ProtocolID, Theory, ShortDescription, ApplicationUserCreatorID, CreationDate ) " +
" select protocolID, Theory, ShortDescription, ApplicationUserCreatorID, CreationDate from protocols " +
" update Links  set Links.ProtocolVersionID = (select ProtocolVersionID from ProtocolVersions where ProtocolID = Links.ProtocolID) " +
" update Materials set Materials.ProtocolVersionID = (select ProtocolVersionID from ProtocolVersions where ProtocolID = Materials.ProtocolID) " +
" update Lines set Lines.ProtocolVersionID = (select ProtocolVersionID from ProtocolVersions where ProtocolID = Lines.ProtocolID) " +
" update ShareProtocols set ShareProtocols.ProtocolVersionID = (select ProtocolVersionID from ProtocolVersions where ProtocolID = ShareProtocols.ProtocolID) " +
" update FavoriteProtocols set FavoriteProtocols.ProtocolVersionID = (select ProtocolVersionID from ProtocolVersions where ProtocolID = FavoriteProtocols.ProtocolID) " +
" update ProtocolInstances set ProtocolInstances.ProtocolVersionID = (select ProtocolVersionID from ProtocolVersions where ProtocolID = ProtocolInstances.ProtocolID) " +
" update FunctionLines set FunctionLines.ProtocolVersionID = (select ProtocolVersionID from ProtocolVersions where ProtocolID = FunctionLines.ProtocolID) ");

            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteProtocols_Protocols_ProtocolID",
                table: "FavoriteProtocols");

            migrationBuilder.DropForeignKey(
                name: "FK_FunctionLines_Protocols_ProtocolID",
                table: "FunctionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_FunctionReports_Protocols_ProtocolID",
                table: "FunctionReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FunctionResults_Protocols_ProtocolID",
                table: "FunctionResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Protocols_ProtocolID",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_Protocols_ProtocolID",
                table: "Links");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Protocols_ProtocolID",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtocolInstances_Protocols_ProtocolID",
                table: "ProtocolInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_Protocols_AspNetUsers_ApplicationUserCreatorID",
                table: "Protocols");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareProtocols_Protocols_ProtocolID",
                table: "ShareProtocols");

            migrationBuilder.DropIndex(
                name: "IX_ShareProtocols_ProtocolID",
                table: "ShareProtocols");

            migrationBuilder.DropIndex(
                name: "IX_Protocols_ApplicationUserCreatorID",
                table: "Protocols");

            migrationBuilder.DropIndex(
                name: "IX_ProtocolInstances_ProtocolID",
                table: "ProtocolInstances");

            migrationBuilder.DropIndex(
                name: "IX_Materials_ProtocolID",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Links_ProtocolID",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Lines_ProtocolID",
                table: "Lines");

            migrationBuilder.DropIndex(
                name: "IX_FunctionResults_ProtocolID",
                table: "FunctionResults");

            migrationBuilder.DropIndex(
                name: "IX_FunctionReports_ProtocolID",
                table: "FunctionReports");

            migrationBuilder.DropIndex(
                name: "IX_FunctionLines_ProtocolID",
                table: "FunctionLines");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteProtocols_ProtocolID",
                table: "FavoriteProtocols");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "ShareProtocols");

            migrationBuilder.DropColumn(
                name: "ApplicationUserCreatorID",
                table: "Protocols");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Protocols");

            migrationBuilder.DropColumn(
                name: "VersionNumber",
                table: "Protocols");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "ProtocolInstances");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "FunctionResults");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "FunctionReports");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "ProtocolID",
                table: "FavoriteProtocols");

            migrationBuilder.AlterColumn<int>(
                name: "ProtocolVersionID",
                table: "ProtocolInstances",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProtocolVersionID",
                table: "Materials",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProtocolVersionID",
                table: "Lines",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "ShareProtocols",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserCreatorID",
                table: "Protocols",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Protocols",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "VersionNumber",
                table: "Protocols",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProtocolVersionID",
                table: "ProtocolInstances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "ProtocolInstances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ProtocolVersionID",
                table: "Materials",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "Links",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ProtocolVersionID",
                table: "Lines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "Lines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "FunctionResults",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "FunctionReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "FunctionLines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProtocolID",
                table: "FavoriteProtocols",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShareProtocols_ProtocolID",
                table: "ShareProtocols",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_Protocols_ApplicationUserCreatorID",
                table: "Protocols",
                column: "ApplicationUserCreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolInstances_ProtocolID",
                table: "ProtocolInstances",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ProtocolID",
                table: "Materials",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_Links_ProtocolID",
                table: "Links",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_ProtocolID",
                table: "Lines",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionResults_ProtocolID",
                table: "FunctionResults",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionReports_ProtocolID",
                table: "FunctionReports",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionLines_ProtocolID",
                table: "FunctionLines",
                column: "ProtocolID");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProtocols_ProtocolID",
                table: "FavoriteProtocols",
                column: "ProtocolID");

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteProtocols_Protocols_ProtocolID",
                table: "FavoriteProtocols",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionLines_Protocols_ProtocolID",
                table: "FunctionLines",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionReports_Protocols_ProtocolID",
                table: "FunctionReports",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionResults_Protocols_ProtocolID",
                table: "FunctionResults",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Protocols_ProtocolID",
                table: "Lines",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Protocols_ProtocolID",
                table: "Links",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Protocols_ProtocolID",
                table: "Materials",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProtocolInstances_Protocols_ProtocolID",
                table: "ProtocolInstances",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Protocols_AspNetUsers_ApplicationUserCreatorID",
                table: "Protocols",
                column: "ApplicationUserCreatorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareProtocols_Protocols_ProtocolID",
                table: "ShareProtocols",
                column: "ProtocolID",
                principalTable: "Protocols",
                principalColumn: "ProtocolID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

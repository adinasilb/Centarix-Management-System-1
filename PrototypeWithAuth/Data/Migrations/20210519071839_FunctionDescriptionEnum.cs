using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class FunctionDescriptionEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.AddColumn<string>(
                name: "DescriptionEnum",
                table: "FunctionTypes",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 1,
                column: "DescriptionEnum",
                value: "AddImage");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 2,
                column: "DescriptionEnum",
                value: "AddTimer");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 3,
                column: "DescriptionEnum",
                value: "AddComment");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 4,
                column: "DescriptionEnum",
                value: "AddWarning");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 5,
                column: "DescriptionEnum",
                value: "AddTip");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 6,
                column: "DescriptionEnum",
                value: "AddTable");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 7,
                column: "DescriptionEnum",
                value: "AddTemplate");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 8,
                column: "DescriptionEnum",
                value: "AddStop");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 9,
                column: "DescriptionEnum",
                value: "AddLinkToProduct");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 10,
                column: "DescriptionEnum",
                value: "AddLinkToProtocol");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 11,
                column: "DescriptionEnum",
                value: "AddFile");
      
    
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.DropColumn(
                name: "DescriptionEnum",
                table: "FunctionTypes");
          
        }
    }
}

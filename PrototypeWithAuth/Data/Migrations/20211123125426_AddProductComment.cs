using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddProductComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProtocolComments_CommentTypes_CommentTypeFKID",
                table: "ProtocolComments");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestComments_CommentTypes_CommentTypeFKID",
                table: "RequestComments");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorComments_CommentTypes_CommentTypeFKID",
                table: "VendorComments");

            migrationBuilder.DropIndex(
                name: "IX_VendorComments_CommentTypeFKID",
                table: "VendorComments");

            migrationBuilder.DropIndex(
                name: "IX_RequestComments_CommentTypeFKID",
                table: "RequestComments");

            migrationBuilder.DropIndex(
                name: "IX_ProtocolComments_CommentTypeFKID",
                table: "ProtocolComments");

            migrationBuilder.RenameColumn(
                name: "CommentTypeFKID",
                table: "VendorComments",
                 newName: "CommentTypeID");

            migrationBuilder.RenameColumn(
                name: "CommentTypeFKID",
                table: "RequestComments",
                 newName: "CommentTypeID");

            migrationBuilder.RenameColumn(
                name: "CommentTypeFKID",
                table: "ProtocolComments",
                newName: "CommentTypeID");


            migrationBuilder.CreateTable(
                name: "ProductComments",
                columns: table => new
                {
                    CommentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserID = table.Column<string>(nullable: true),
                    CommentText = table.Column<string>(nullable: true),
                    CommentTimeStamp = table.Column<DateTime>(nullable: false),
                    CommentTypeID = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ObjectID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductComments", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_ProductComments_AspNetUsers_ApplicationUserID",
                        column: x => x.ApplicationUserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductComments_CommentTypes_CommentTypeID",
                        column: x => x.CommentTypeID,
                        principalTable: "CommentTypes",
                        principalColumn: "TypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductComments_Products_ObjectID",
                        column: x => x.ObjectID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VendorComments_CommentTypeID",
                table: "VendorComments",
                column: "CommentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestComments_CommentTypeID",
                table: "RequestComments",
                column: "CommentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolComments_CommentTypeID",
                table: "ProtocolComments",
                column: "CommentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComments_ApplicationUserID",
                table: "ProductComments",
                column: "ApplicationUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComments_CommentTypeID",
                table: "ProductComments",
                column: "CommentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductComments_ObjectID",
                table: "ProductComments",
                column: "ObjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProtocolComments_CommentTypes_CommentTypeID",
                table: "ProtocolComments",
                column: "CommentTypeID",
                principalTable: "CommentTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComments_CommentTypes_CommentTypeID",
                table: "RequestComments",
                column: "CommentTypeID",
                principalTable: "CommentTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorComments_CommentTypes_CommentTypeID",
                table: "VendorComments",
                column: "CommentTypeID",
                principalTable: "CommentTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProtocolComments_CommentTypes_CommentTypeID",
                table: "ProtocolComments");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestComments_CommentTypes_CommentTypeID",
                table: "RequestComments");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorComments_CommentTypes_CommentTypeID",
                table: "VendorComments");

            migrationBuilder.DropTable(
                name: "ProductComments");

            migrationBuilder.DropIndex(
                name: "IX_VendorComments_CommentTypeID",
                table: "VendorComments");

            migrationBuilder.DropIndex(
                name: "IX_RequestComments_CommentTypeID",
                table: "RequestComments");

            migrationBuilder.DropIndex(
                name: "IX_ProtocolComments_CommentTypeID",
                table: "ProtocolComments");

            migrationBuilder.DropColumn(
                name: "CommentTypeID",
                table: "VendorComments");

            migrationBuilder.DropColumn(
                name: "CommentTypeID",
                table: "RequestComments");

            migrationBuilder.DropColumn(
                name: "CommentTypeID",
                table: "ProtocolComments");

            migrationBuilder.AddColumn<int>(
                name: "CommentTypeFKID",
                table: "VendorComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommentTypeFKID",
                table: "RequestComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommentTypeFKID",
                table: "ProtocolComments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VendorComments_CommentTypeFKID",
                table: "VendorComments",
                column: "CommentTypeFKID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestComments_CommentTypeFKID",
                table: "RequestComments",
                column: "CommentTypeFKID");

            migrationBuilder.CreateIndex(
                name: "IX_ProtocolComments_CommentTypeFKID",
                table: "ProtocolComments",
                column: "CommentTypeFKID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProtocolComments_CommentTypes_CommentTypeFKID",
                table: "ProtocolComments",
                column: "CommentTypeFKID",
                principalTable: "CommentTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestComments_CommentTypes_CommentTypeFKID",
                table: "RequestComments",
                column: "CommentTypeFKID",
                principalTable: "CommentTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorComments_CommentTypes_CommentTypeFKID",
                table: "VendorComments",
                column: "CommentTypeFKID",
                principalTable: "CommentTypes",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

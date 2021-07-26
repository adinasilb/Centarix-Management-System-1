using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddTempIDsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
    
            migrationBuilder.DropTable(
                name: "FunctionLines");


            migrationBuilder.CreateTable(
                name: "FunctionLineIDs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionLineIDs", x => x.ID);
                });

           

            migrationBuilder.CreateTable(
                name: "TempLineIDs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TempLineIDs", x => x.ID);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FunctionLineIDs");

       
            migrationBuilder.DropTable(
                name: "TempLineIDs");

           
            migrationBuilder.CreateTable(
                name: "FunctionLines",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FunctionTypeID = table.Column<int>(type: "int", nullable: false),
                    IsTemporary = table.Column<bool>(type: "bit", nullable: false),
                    IsTemporaryDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LineID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    ProtocolID = table.Column<int>(type: "int", nullable: true),
                    Timer = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionLines", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FunctionLines_FunctionTypes_FunctionTypeID",
                        column: x => x.FunctionTypeID,
                        principalTable: "FunctionTypes",
                        principalColumn: "FunctionTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionLines_Lines_LineID",
                        column: x => x.LineID,
                        principalTable: "Lines",
                        principalColumn: "LineID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionLines_Protocols_ProtocolID",
                        column: x => x.ProtocolID,
                        principalTable: "Protocols",
                        principalColumn: "ProtocolID",
                        onDelete: ReferentialAction.Restrict);
                });
                    

        }
    }
}

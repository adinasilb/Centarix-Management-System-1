using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class LocationTierModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationsTier1Models",
                columns: table => new
                {
                    LocationsTier1ModelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsTier1ModelDescription = table.Column<string>(nullable: false),
                    LocationsTier1ModelAbbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsTier1Models", x => x.LocationsTier1ModelID);
                });

            migrationBuilder.CreateTable(
                name: "LocationsTier2Models",
                columns: table => new
                {
                    LocationsTier2ModelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsTier1ModelID = table.Column<int>(nullable: false),
                    LocationsTier2ModelDescription = table.Column<string>(nullable: false),
                    LocationsTier2ModelAbbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsTier2Models", x => x.LocationsTier2ModelID);
                    table.ForeignKey(
                        name: "FK_LocationsTier2Models_LocationsTier1Models_LocationsTier1ModelID",
                        column: x => x.LocationsTier1ModelID,
                        principalTable: "LocationsTier1Models",
                        principalColumn: "LocationsTier1ModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationsTier3Models",
                columns: table => new
                {
                    LocationsTier3ModelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsTier2ModelID = table.Column<int>(nullable: false),
                    LocationsTier3ModelDescription = table.Column<string>(nullable: false),
                    LocationsTier3ModelAbbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsTier3Models", x => x.LocationsTier3ModelID);
                    table.ForeignKey(
                        name: "FK_LocationsTier3Models_LocationsTier2Models_LocationsTier2ModelID",
                        column: x => x.LocationsTier2ModelID,
                        principalTable: "LocationsTier2Models",
                        principalColumn: "LocationsTier2ModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationsTier4Models",
                columns: table => new
                {
                    LocationsTier4ModelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsTier3ModelID = table.Column<int>(nullable: false),
                    LocationsTier4ModelDescription = table.Column<string>(nullable: false),
                    LocationsTier4ModelAbbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsTier4Models", x => x.LocationsTier4ModelID);
                    table.ForeignKey(
                        name: "FK_LocationsTier4Models_LocationsTier3Models_LocationsTier3ModelID",
                        column: x => x.LocationsTier3ModelID,
                        principalTable: "LocationsTier3Models",
                        principalColumn: "LocationsTier3ModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationsTier5Models",
                columns: table => new
                {
                    LocationsTier5ModelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsTier4ModelID = table.Column<int>(nullable: false),
                    LocationsTier5ModelDescription = table.Column<string>(nullable: false),
                    LocationsTier5ModelAbbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsTier5Models", x => x.LocationsTier5ModelID);
                    table.ForeignKey(
                        name: "FK_LocationsTier5Models_LocationsTier4Models_LocationsTier4ModelID",
                        column: x => x.LocationsTier4ModelID,
                        principalTable: "LocationsTier4Models",
                        principalColumn: "LocationsTier4ModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationsTier6Models",
                columns: table => new
                {
                    LocationsTier6ModelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsTier5ModelID = table.Column<int>(nullable: false),
                    LocationsTier6ModelDescription = table.Column<string>(nullable: false),
                    LocationsTier6ModelAbbreviation = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsTier6Models", x => x.LocationsTier6ModelID);
                    table.ForeignKey(
                        name: "FK_LocationsTier6Models_LocationsTier5Models_LocationsTier5ModelID",
                        column: x => x.LocationsTier5ModelID,
                        principalTable: "LocationsTier5Models",
                        principalColumn: "LocationsTier5ModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier2Models_LocationsTier1ModelID",
                table: "LocationsTier2Models",
                column: "LocationsTier1ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier3Models_LocationsTier2ModelID",
                table: "LocationsTier3Models",
                column: "LocationsTier2ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier4Models_LocationsTier3ModelID",
                table: "LocationsTier4Models",
                column: "LocationsTier3ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier5Models_LocationsTier4ModelID",
                table: "LocationsTier5Models",
                column: "LocationsTier4ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier6Models_LocationsTier5ModelID",
                table: "LocationsTier6Models",
                column: "LocationsTier5ModelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationsTier6Models");

            migrationBuilder.DropTable(
                name: "LocationsTier5Models");

            migrationBuilder.DropTable(
                name: "LocationsTier4Models");

            migrationBuilder.DropTable(
                name: "LocationsTier3Models");

            migrationBuilder.DropTable(
                name: "LocationsTier2Models");

            migrationBuilder.DropTable(
                name: "LocationsTier1Models");
        }
    }
}

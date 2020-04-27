using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class LocationTierInstances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationsTier1Instance",
                columns: table => new
                {
                    LocationsTier1InstanceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsTier1ModelID = table.Column<int>(nullable: false),
                    LocationsTier1InstanceAbrv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsTier1Instance", x => x.LocationsTier1InstanceID);
                    table.ForeignKey(
                        name: "FK_LocationsTier1Instance_LocationsTier1Models_LocationsTier1ModelID",
                        column: x => x.LocationsTier1ModelID,
                        principalTable: "LocationsTier1Models",
                        principalColumn: "LocationsTier1ModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationsTier2Instance",
                columns: table => new
                {
                    LocationsTier2InstanceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsTier2ModelID = table.Column<int>(nullable: false),
                    LocationsTier1InstanceID = table.Column<int>(nullable: false),
                    LocationsTier2InstanceAbrv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsTier2Instance", x => x.LocationsTier2InstanceID);
                    table.ForeignKey(
                        name: "FK_LocationsTier2Instance_LocationsTier1Instance_LocationsTier1InstanceID",
                        column: x => x.LocationsTier1InstanceID,
                        principalTable: "LocationsTier1Instance",
                        principalColumn: "LocationsTier1InstanceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationsTier2Instance_LocationsTier2Models_LocationsTier2ModelID",
                        column: x => x.LocationsTier2ModelID,
                        principalTable: "LocationsTier2Models",
                        principalColumn: "LocationsTier2ModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationsTier3Instance",
                columns: table => new
                {
                    LocationsTier3InstanceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsTier3ModelID = table.Column<int>(nullable: false),
                    LocationsTier2InstanceID = table.Column<int>(nullable: false),
                    LocationsTier3InstanceAbrv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsTier3Instance", x => x.LocationsTier3InstanceID);
                    table.ForeignKey(
                        name: "FK_LocationsTier3Instance_LocationsTier2Instance_LocationsTier2InstanceID",
                        column: x => x.LocationsTier2InstanceID,
                        principalTable: "LocationsTier2Instance",
                        principalColumn: "LocationsTier2InstanceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationsTier3Instance_LocationsTier3Models_LocationsTier3ModelID",
                        column: x => x.LocationsTier3ModelID,
                        principalTable: "LocationsTier3Models",
                        principalColumn: "LocationsTier3ModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationsTier4Instance",
                columns: table => new
                {
                    LocationsTier4InstanceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsTier4ModelID = table.Column<int>(nullable: false),
                    LocationsTier3InstanceID = table.Column<int>(nullable: false),
                    LocationsTier4InstanceAbrv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsTier4Instance", x => x.LocationsTier4InstanceID);
                    table.ForeignKey(
                        name: "FK_LocationsTier4Instance_LocationsTier3Instance_LocationsTier3InstanceID",
                        column: x => x.LocationsTier3InstanceID,
                        principalTable: "LocationsTier3Instance",
                        principalColumn: "LocationsTier3InstanceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationsTier4Instance_LocationsTier4Models_LocationsTier4ModelID",
                        column: x => x.LocationsTier4ModelID,
                        principalTable: "LocationsTier4Models",
                        principalColumn: "LocationsTier4ModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationsTier5Instance",
                columns: table => new
                {
                    LocationsTier5InstanceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsTier5ModelID = table.Column<int>(nullable: false),
                    LocationsTier4InstanceID = table.Column<int>(nullable: false),
                    LocationsTier5InstanceAbrv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsTier5Instance", x => x.LocationsTier5InstanceID);
                    table.ForeignKey(
                        name: "FK_LocationsTier5Instance_LocationsTier4Instance_LocationsTier4InstanceID",
                        column: x => x.LocationsTier4InstanceID,
                        principalTable: "LocationsTier4Instance",
                        principalColumn: "LocationsTier4InstanceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationsTier5Instance_LocationsTier5Models_LocationsTier5ModelID",
                        column: x => x.LocationsTier5ModelID,
                        principalTable: "LocationsTier5Models",
                        principalColumn: "LocationsTier5ModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationsTier6Instance",
                columns: table => new
                {
                    LocationsTier6InstanceID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsTier6ModelID = table.Column<int>(nullable: false),
                    LocationsTier5InstanceID = table.Column<int>(nullable: false),
                    LocationsTier5InstanceAbrv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationsTier6Instance", x => x.LocationsTier6InstanceID);
                    table.ForeignKey(
                        name: "FK_LocationsTier6Instance_LocationsTier5Instance_LocationsTier5InstanceID",
                        column: x => x.LocationsTier5InstanceID,
                        principalTable: "LocationsTier5Instance",
                        principalColumn: "LocationsTier5InstanceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationsTier6Instance_LocationsTier6Models_LocationsTier6ModelID",
                        column: x => x.LocationsTier6ModelID,
                        principalTable: "LocationsTier6Models",
                        principalColumn: "LocationsTier6ModelID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier1Instance_LocationsTier1ModelID",
                table: "LocationsTier1Instance",
                column: "LocationsTier1ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier2Instance_LocationsTier1InstanceID",
                table: "LocationsTier2Instance",
                column: "LocationsTier1InstanceID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier2Instance_LocationsTier2ModelID",
                table: "LocationsTier2Instance",
                column: "LocationsTier2ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier3Instance_LocationsTier2InstanceID",
                table: "LocationsTier3Instance",
                column: "LocationsTier2InstanceID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier3Instance_LocationsTier3ModelID",
                table: "LocationsTier3Instance",
                column: "LocationsTier3ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier4Instance_LocationsTier3InstanceID",
                table: "LocationsTier4Instance",
                column: "LocationsTier3InstanceID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier4Instance_LocationsTier4ModelID",
                table: "LocationsTier4Instance",
                column: "LocationsTier4ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier5Instance_LocationsTier4InstanceID",
                table: "LocationsTier5Instance",
                column: "LocationsTier4InstanceID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier5Instance_LocationsTier5ModelID",
                table: "LocationsTier5Instance",
                column: "LocationsTier5ModelID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier6Instance_LocationsTier5InstanceID",
                table: "LocationsTier6Instance",
                column: "LocationsTier5InstanceID");

            migrationBuilder.CreateIndex(
                name: "IX_LocationsTier6Instance_LocationsTier6ModelID",
                table: "LocationsTier6Instance",
                column: "LocationsTier6ModelID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationsTier6Instance");

            migrationBuilder.DropTable(
                name: "LocationsTier5Instance");

            migrationBuilder.DropTable(
                name: "LocationsTier4Instance");

            migrationBuilder.DropTable(
                name: "LocationsTier3Instance");

            migrationBuilder.DropTable(
                name: "LocationsTier2Instance");

            migrationBuilder.DropTable(
                name: "LocationsTier1Instance");
        }
    }
}

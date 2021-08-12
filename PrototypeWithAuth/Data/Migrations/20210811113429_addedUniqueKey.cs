using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class addedUniqueKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TestHeaders_SequencePosition_TestGroupID",
                table: "TestHeaders",
                columns: new[] { "SequencePosition", "TestGroupID" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TestHeaders_SequencePosition_TestGroupID",
                table: "TestHeaders");
        }
    }
}

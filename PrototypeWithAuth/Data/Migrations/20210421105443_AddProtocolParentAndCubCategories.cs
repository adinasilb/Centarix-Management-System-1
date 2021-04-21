using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddProtocolParentAndCubCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProtocolCategories",
                columns: new[] { "ProtocolCategoryTypeID", "ProtocolDescription" },
                values: new object[] { 1, "Rejuvenation" });

            migrationBuilder.InsertData(
                table: "ProtocolCategories",
                columns: new[] { "ProtocolCategoryTypeID", "ProtocolDescription" },
                values: new object[] { 2, "Biomarkers" });

            migrationBuilder.InsertData(
                table: "ProtocolCategories",
                columns: new[] { "ProtocolCategoryTypeID", "ProtocolDescription" },
                values: new object[] { 3, "Delivery Systems" });

            migrationBuilder.InsertData(
                table: "ProtocolSubCategories",
                columns: new[] { "ProtocolSubCategoryTypeID", "ProtocolCategoryTypeID", "ProtocolSubCategoryTypeDescription" },
                values: new object[,]
                {
                    { 1, 1, "Telomeres " },
                    { 2, 1, "Epigenetics" },
                    { 3, 2, "Telomeres " },
                    { 4, 2, "Transcription" },
                    { 5, 2, "Methylation" },
                    { 6, 3, "AAV" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProtocolSubCategories",
                keyColumn: "ProtocolSubCategoryTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProtocolSubCategories",
                keyColumn: "ProtocolSubCategoryTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProtocolSubCategories",
                keyColumn: "ProtocolSubCategoryTypeID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProtocolSubCategories",
                keyColumn: "ProtocolSubCategoryTypeID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProtocolSubCategories",
                keyColumn: "ProtocolSubCategoryTypeID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProtocolSubCategories",
                keyColumn: "ProtocolSubCategoryTypeID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProtocolCategories",
                keyColumn: "ProtocolCategoryTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProtocolCategories",
                keyColumn: "ProtocolCategoryTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProtocolCategories",
                keyColumn: "ProtocolCategoryTypeID",
                keyValue: 3);
        }
    }
}

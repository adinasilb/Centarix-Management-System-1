using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedFunctionTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FunctionTypes",
                columns: new[] { "FunctionTypeID", "FunctionDescription", "Icon", "IconActionClass" },
                values: new object[,]
                {
                    { 1, "Add Image", "", "add-image-to-line" },
                    { 2, "Add Timer", "", "add-timer-to-line" },
                    { 3, "Add Comment", "", "add-comment-to-line" },
                    { 4, "Add Warning", "", "add-warning-to-line" },
                    { 5, "Add Tip", "", "add-tip-to-line" },
                    { 6, "Add Table", "", "add-table-to-line" },
                    { 7, "Add Template", "", "add-template-to-line" },
                    { 8, "Add Comment", "", "add-comment-to-line" },
                    { 9, "Add Stop", "", "add-stop-to-line" },
                    { 10, "Add Comment", "", "add-comment-to-line" },
                    { 11, "Add Link To Product", "", "add-product-to-line" },
                    { 12, "Add Link To Protocol", "", "add-protocol-to-line" },
                    { 13, "Add File", "", "add-file-to-line" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 13);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddedFunctionTypeIcons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 13);

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 2,
                column: "Icon",
                value: "icon-centarix-icons-19");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 3,
                column: "Icon",
                value: "icon-comment-24px");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 4,
                column: "Icon",
                value: "icon-report_problem-24px");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 5,
                column: "Icon",
                value: "icon-tip-24px");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 6,
                column: "Icon",
                value: "icon-table_chart-24px1");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 8,
                columns: new[] { "FunctionDescription", "Icon", "IconActionClass" },
                values: new object[] { "Add Stop", "icon-stop-24px", "add-stop-to-line" });

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 9,
                columns: new[] { "FunctionDescription", "Icon", "IconActionClass" },
                values: new object[] { "Add Link To Product", "icon-attach-item-24px", "add-product-to-line" });

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 10,
                columns: new[] { "FunctionDescription", "Icon", "IconActionClass" },
                values: new object[] { "Add Link To Protocol", "icon-attach-protocol-24px", "add-protocol-to-line" });

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 11,
                columns: new[] { "FunctionDescription", "Icon", "IconActionClass" },
                values: new object[] { "Add File", "icon-description-24px2", "add-file-to-line" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 2,
                column: "Icon",
                value: "");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 3,
                column: "Icon",
                value: "");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 4,
                column: "Icon",
                value: "");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 5,
                column: "Icon",
                value: "");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 6,
                column: "Icon",
                value: "");

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 8,
                columns: new[] { "FunctionDescription", "Icon", "IconActionClass" },
                values: new object[] { "Add Comment", "", "add-comment-to-line" });

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 9,
                columns: new[] { "FunctionDescription", "Icon", "IconActionClass" },
                values: new object[] { "Add Stop", "", "add-stop-to-line" });

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 10,
                columns: new[] { "FunctionDescription", "Icon", "IconActionClass" },
                values: new object[] { "Add Comment", "", "add-comment-to-line" });

            migrationBuilder.UpdateData(
                table: "FunctionTypes",
                keyColumn: "FunctionTypeID",
                keyValue: 11,
                columns: new[] { "FunctionDescription", "Icon", "IconActionClass" },
                values: new object[] { "Add Link To Product", "", "add-product-to-line" });

            migrationBuilder.InsertData(
                table: "FunctionTypes",
                columns: new[] { "FunctionTypeID", "FunctionDescription", "Icon", "IconActionClass" },
                values: new object[,]
                {
                    { 12, "Add Link To Protocol", "", "add-protocol-to-line" },
                    { 13, "Add File", "", "add-file-to-line" }
                });
        }
    }
}

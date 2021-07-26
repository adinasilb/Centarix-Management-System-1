using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class RemovePrimaryKeyFunctionLine2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FunctionLines",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "FunctonLineID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "FunctioTypeID",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "LieID",
                table: "FunctionLines");

            migrationBuilder.AlterColumn<int>(
                name: "LineID",
                table: "FunctionLines",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FunctionTypeID",
                table: "FunctionLines",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FunctionLineID",
                table: "FunctionLines",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FunctionLines",
                table: "FunctionLines",
                column: "FunctionLineID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FunctionLines",
                table: "FunctionLines");

            migrationBuilder.DropColumn(
                name: "FunctionLineID",
                table: "FunctionLines");

            migrationBuilder.AlterColumn<int>(
                name: "LineID",
                table: "FunctionLines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FunctionTypeID",
                table: "FunctionLines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "FunctonLineID",
                table: "FunctionLines",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "FunctioTypeID",
                table: "FunctionLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LieID",
                table: "FunctionLines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FunctionLines",
                table: "FunctionLines",
                column: "FunctonLineID");
        }
    }
}

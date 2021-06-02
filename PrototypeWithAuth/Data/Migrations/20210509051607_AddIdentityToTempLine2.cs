using Microsoft.EntityFrameworkCore.Migrations;

namespace PrototypeWithAuth.Data.Migrations
{
    public partial class AddIdentityToTempLine2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunctionLines_Lines_LineID",
                table: "FunctionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Lines_ParentLineID",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_Lines_TempLines_TempLineLineFID",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtocolInstances_Lines_CurrentLineID",
                table: "ProtocolInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_Lines_PermanentLineID",
                table: "TempLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TempLines",
                table: "TempLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lines",
                table: "Lines");

            migrationBuilder.DropIndex(
                name: "IX_Lines_TempLineLineFID",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "LineFID",
                table: "TempLines");

            migrationBuilder.DropColumn(
                name: "LineFID",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "TempLineLineFID",
                table: "Lines");

            migrationBuilder.AddColumn<int>(
                name: "LineID",
                table: "TempLines",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "LineID",
                table: "Lines",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TempLineLineID",
                table: "Lines",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempLines",
                table: "TempLines",
                column: "LineID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lines",
                table: "Lines",
                column: "LineID");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_TempLineLineID",
                table: "Lines",
                column: "TempLineLineID");

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionLines_Lines_LineID",
                table: "FunctionLines",
                column: "LineID",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Lines_ParentLineID",
                table: "Lines",
                column: "ParentLineID",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_TempLines_TempLineLineID",
                table: "Lines",
                column: "TempLineLineID",
                principalTable: "TempLines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProtocolInstances_Lines_CurrentLineID",
                table: "ProtocolInstances",
                column: "CurrentLineID",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines",
                column: "ParentLineID",
                principalTable: "TempLines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_Lines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID",
                principalTable: "Lines",
                principalColumn: "LineID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FunctionLines_Lines_LineID",
                table: "FunctionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Lines_ParentLineID",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_Lines_TempLines_TempLineLineID",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_ProtocolInstances_Lines_CurrentLineID",
                table: "ProtocolInstances");

            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TempLines_Lines_PermanentLineID",
                table: "TempLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TempLines",
                table: "TempLines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lines",
                table: "Lines");

            migrationBuilder.DropIndex(
                name: "IX_Lines_TempLineLineID",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "LineID",
                table: "TempLines");

            migrationBuilder.DropColumn(
                name: "LineID",
                table: "Lines");

            migrationBuilder.DropColumn(
                name: "TempLineLineID",
                table: "Lines");

            migrationBuilder.AddColumn<int>(
                name: "LineFID",
                table: "TempLines",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "LineFID",
                table: "Lines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TempLineLineFID",
                table: "Lines",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TempLines",
                table: "TempLines",
                column: "LineFID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lines",
                table: "Lines",
                column: "LineFID");

            migrationBuilder.CreateIndex(
                name: "IX_Lines_TempLineLineFID",
                table: "Lines",
                column: "TempLineLineFID");

            migrationBuilder.AddForeignKey(
                name: "FK_FunctionLines_Lines_LineID",
                table: "FunctionLines",
                column: "LineID",
                principalTable: "Lines",
                principalColumn: "LineFID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Lines_ParentLineID",
                table: "Lines",
                column: "ParentLineID",
                principalTable: "Lines",
                principalColumn: "LineFID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_TempLines_TempLineLineFID",
                table: "Lines",
                column: "TempLineLineFID",
                principalTable: "TempLines",
                principalColumn: "LineFID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProtocolInstances_Lines_CurrentLineID",
                table: "ProtocolInstances",
                column: "CurrentLineID",
                principalTable: "Lines",
                principalColumn: "LineFID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_TempLines_ParentLineID",
                table: "TempLines",
                column: "ParentLineID",
                principalTable: "TempLines",
                principalColumn: "LineFID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TempLines_Lines_PermanentLineID",
                table: "TempLines",
                column: "PermanentLineID",
                principalTable: "Lines",
                principalColumn: "LineFID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

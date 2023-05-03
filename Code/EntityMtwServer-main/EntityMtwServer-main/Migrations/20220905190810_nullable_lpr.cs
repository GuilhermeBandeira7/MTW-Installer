using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityMtwServer.Migrations
{
    public partial class nullable_lpr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Equipment_AcessId",
                table: "Lprs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Equipment_Context1Id",
                table: "Lprs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Equipment_Context2Id",
                table: "Lprs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Equipment_Context3Id",
                table: "Lprs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Equipment_Context4Id",
                table: "Lprs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Equipment_ControllerId",
                table: "Lprs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Origins_OriginId",
                table: "Lprs");

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Lprs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ControllerId",
                table: "Lprs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Context4Id",
                table: "Lprs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Context3Id",
                table: "Lprs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Context2Id",
                table: "Lprs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "Context1Id",
                table: "Lprs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "AcessId",
                table: "Lprs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Equipment_AcessId",
                table: "Lprs",
                column: "AcessId",
                principalTable: "Equipment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Equipment_Context1Id",
                table: "Lprs",
                column: "Context1Id",
                principalTable: "Equipment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Equipment_Context2Id",
                table: "Lprs",
                column: "Context2Id",
                principalTable: "Equipment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Equipment_Context3Id",
                table: "Lprs",
                column: "Context3Id",
                principalTable: "Equipment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Equipment_Context4Id",
                table: "Lprs",
                column: "Context4Id",
                principalTable: "Equipment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Equipment_ControllerId",
                table: "Lprs",
                column: "ControllerId",
                principalTable: "Equipment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Origins_OriginId",
                table: "Lprs",
                column: "OriginId",
                principalTable: "Origins",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Equipment_AcessId",
                table: "Lprs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Equipment_Context1Id",
                table: "Lprs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Equipment_Context2Id",
                table: "Lprs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Equipment_Context3Id",
                table: "Lprs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Equipment_Context4Id",
                table: "Lprs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Equipment_ControllerId",
                table: "Lprs");

            migrationBuilder.DropForeignKey(
                name: "FK_Lprs_Origins_OriginId",
                table: "Lprs");

            migrationBuilder.AlterColumn<long>(
                name: "OriginId",
                table: "Lprs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ControllerId",
                table: "Lprs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Context4Id",
                table: "Lprs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Context3Id",
                table: "Lprs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Context2Id",
                table: "Lprs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Context1Id",
                table: "Lprs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AcessId",
                table: "Lprs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Equipment_AcessId",
                table: "Lprs",
                column: "AcessId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Equipment_Context1Id",
                table: "Lprs",
                column: "Context1Id",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Equipment_Context2Id",
                table: "Lprs",
                column: "Context2Id",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Equipment_Context3Id",
                table: "Lprs",
                column: "Context3Id",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Equipment_Context4Id",
                table: "Lprs",
                column: "Context4Id",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Equipment_ControllerId",
                table: "Lprs",
                column: "ControllerId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lprs_Origins_OriginId",
                table: "Lprs",
                column: "OriginId",
                principalTable: "Origins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

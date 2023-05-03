using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityMtwServer.Migrations
{
    public partial class nullLprRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LprRecords_Lprs_LprId",
                table: "LprRecords");

            migrationBuilder.AlterColumn<long>(
                name: "LprId",
                table: "LprRecords",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_LprRecords_Lprs_LprId",
                table: "LprRecords",
                column: "LprId",
                principalTable: "Lprs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LprRecords_Lprs_LprId",
                table: "LprRecords");

            migrationBuilder.AlterColumn<long>(
                name: "LprId",
                table: "LprRecords",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LprRecords_Lprs_LprId",
                table: "LprRecords",
                column: "LprId",
                principalTable: "Lprs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

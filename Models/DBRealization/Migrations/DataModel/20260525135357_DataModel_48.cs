using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_48 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StatusRaoCode",
                table: "package_passport",
                type: "VARCHAR(128)",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(16)",
                oldMaxLength: 16,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StatusRaoCode",
                table: "package_passport",
                type: "VARCHAR(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(128)",
                oldMaxLength: 128,
                oldNullable: true);
        }
    }
}

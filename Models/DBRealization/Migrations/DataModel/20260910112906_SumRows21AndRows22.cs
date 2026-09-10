using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class SumRows21AndRows22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSumRow",
                table: "form_22",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSumRow",
                table: "form_21",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSumRow",
                table: "form_22");

            migrationBuilder.DropColumn(
                name: "IsSumRow",
                table: "form_21");
        }
    }
}

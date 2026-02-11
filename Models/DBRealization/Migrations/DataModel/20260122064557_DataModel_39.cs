using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_39 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExecutiveAuthority_DB",
                table: "form_50",
                type: "varchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MinObr_DB",
                table: "form_50",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Rosatom_DB",
                table: "form_50",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutiveAuthority_DB",
                table: "form_50");

            migrationBuilder.DropColumn(
                name: "MinObr_DB",
                table: "form_50");

            migrationBuilder.DropColumn(
                name: "Rosatom_DB",
                table: "form_50");
        }
    }
}

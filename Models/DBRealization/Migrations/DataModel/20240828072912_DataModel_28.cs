using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "СontractNumber_DB",
                table: "form_18",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "СontractNumber_DB",
                table: "form_17",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "СontractNumber_DB",
                table: "form_16",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "СontractNumber_DB",
                table: "form_15",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "СontractNumber_DB",
                table: "form_18");

            migrationBuilder.DropColumn(
                name: "СontractNumber_DB",
                table: "form_17");

            migrationBuilder.DropColumn(
                name: "СontractNumber_DB",
                table: "form_16");

            migrationBuilder.DropColumn(
                name: "СontractNumber_DB",
                table: "form_15");
        }
    }
}

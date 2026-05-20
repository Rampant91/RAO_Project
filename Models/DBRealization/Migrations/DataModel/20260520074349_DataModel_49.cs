using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_49 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectVolume",
                table: "license_info");

            migrationBuilder.AddColumn<double>(
                name: "ProjectVolume",
                table: "storage_point",
                type: "DOUBLE PRECISION",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectVolume",
                table: "storage_point");

            migrationBuilder.AddColumn<double>(
                name: "ProjectVolume",
                table: "license_info",
                type: "DOUBLE PRECISION",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

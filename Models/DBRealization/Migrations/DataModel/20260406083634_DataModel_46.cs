using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_46 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlphaActivity",
                table: "characteristic_package");

            migrationBuilder.DropColumn(
                name: "BetaGammaActivity",
                table: "characteristic_package");

            migrationBuilder.DropColumn(
                name: "LongLivingActivity",
                table: "characteristic_package");

            migrationBuilder.DropColumn(
                name: "TotalActivity",
                table: "characteristic_package");

            migrationBuilder.DropColumn(
                name: "TransuraniumActivity",
                table: "characteristic_package");

            migrationBuilder.DropColumn(
                name: "TritiumActivity",
                table: "characteristic_package");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AlphaActivity",
                table: "characteristic_package",
                type: "DOUBLE PRECISION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BetaGammaActivity",
                table: "characteristic_package",
                type: "DOUBLE PRECISION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LongLivingActivity",
                table: "characteristic_package",
                type: "DOUBLE PRECISION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalActivity",
                table: "characteristic_package",
                type: "DOUBLE PRECISION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TransuraniumActivity",
                table: "characteristic_package",
                type: "DOUBLE PRECISION",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TritiumActivity",
                table: "characteristic_package",
                type: "DOUBLE PRECISION",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

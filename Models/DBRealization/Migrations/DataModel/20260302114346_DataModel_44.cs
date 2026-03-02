using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrimaryPackageNum",
                table: "characteristic_package");

            migrationBuilder.DropColumn(
                name: "PrimaryPackageType",
                table: "characteristic_package");

            migrationBuilder.AddColumn<string>(
                name: "PackageIdNum",
                table: "characteristic_package",
                type: "VARCHAR(64)",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageIdNum",
                table: "characteristic_package");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryPackageNum",
                table: "characteristic_package",
                type: "VARCHAR(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryPackageType",
                table: "characteristic_package",
                type: "VARCHAR(32)",
                maxLength: 32,
                nullable: true);
        }
    }
}

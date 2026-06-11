using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_45 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flammability",
                table: "package_passport");

            migrationBuilder.DropColumn(
                name: "MorphologicalComposition",
                table: "package_passport");

            migrationBuilder.DropColumn(
                name: "PhysicochemicalForm",
                table: "package_passport");

            migrationBuilder.DropColumn(
                name: "PackageIdNum",
                table: "characteristic_package");

            migrationBuilder.AlterColumn<long>(
                name: "Width",
                table: "package_passport",
                type: "BIGINT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ServiceLife",
                table: "package_passport",
                type: "BIGINT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<long>(
                name: "Length",
                table: "package_passport",
                type: "BIGINT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Height",
                table: "package_passport",
                type: "BIGINT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "Diameter",
                table: "package_passport",
                type: "BIGINT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PrimaryPackageQuantity",
                table: "characteristic_package",
                type: "BIGINT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Flammability",
                table: "characteristic_package",
                type: "VARCHAR(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MorphologicalComposition",
                table: "characteristic_package",
                type: "VARCHAR(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageNum",
                table: "characteristic_package",
                type: "VARCHAR(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PackageType",
                table: "characteristic_package",
                type: "VARCHAR(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhysicochemicalForm",
                table: "characteristic_package",
                type: "VARCHAR(1024)",
                maxLength: 1024,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Flammability",
                table: "characteristic_package");

            migrationBuilder.DropColumn(
                name: "MorphologicalComposition",
                table: "characteristic_package");

            migrationBuilder.DropColumn(
                name: "PackageNum",
                table: "characteristic_package");

            migrationBuilder.DropColumn(
                name: "PackageType",
                table: "characteristic_package");

            migrationBuilder.DropColumn(
                name: "PhysicochemicalForm",
                table: "characteristic_package");

            migrationBuilder.AlterColumn<int>(
                name: "Width",
                table: "package_passport",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "BIGINT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ServiceLife",
                table: "package_passport",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "BIGINT");

            migrationBuilder.AlterColumn<int>(
                name: "Length",
                table: "package_passport",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "BIGINT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Height",
                table: "package_passport",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "BIGINT",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Diameter",
                table: "package_passport",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "BIGINT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Flammability",
                table: "package_passport",
                type: "VARCHAR(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MorphologicalComposition",
                table: "package_passport",
                type: "VARCHAR(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhysicochemicalForm",
                table: "package_passport",
                type: "VARCHAR(1024)",
                maxLength: 1024,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PrimaryPackageQuantity",
                table: "characteristic_package",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "BIGINT");

            migrationBuilder.AddColumn<string>(
                name: "PackageIdNum",
                table: "characteristic_package",
                type: "VARCHAR(64)",
                maxLength: 64,
                nullable: true);
        }
    }
}

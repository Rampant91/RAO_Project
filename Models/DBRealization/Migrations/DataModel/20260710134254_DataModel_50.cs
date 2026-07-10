using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_50 : Migration
    {
        //ААААААААААААААААААААААААААААААААААААААААААААААААААААА
        //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        //Все неправильно надо переписать с нуля связку в дизайнере
        //И разобраться чтоб нормально мигрировала и сохраняла изменения
        //У тебя 1 день)
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_characteristic_package_pack~",
                table: "characteristic_package");

            migrationBuilder.DropForeignKey(
                name: "FK_radionuclid_characteristic_~",
                table: "radionuclid");

            migrationBuilder.DropIndex(
                name: "IX_radionuclid_CharacteristicId",
                table: "radionuclid");

            migrationBuilder.DropIndex(
                name: "IX_characteristic_package_Pass~",
                table: "characteristic_package");

            migrationBuilder.AddColumn<int>(
                name: "CharacteristicPrimaryPackageId",
                table: "radionuclid",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PackagePassportId",
                table: "characteristic_package",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_radionuclid_CharacteristicP~",
                table: "radionuclid",
                column: "CharacteristicPrimaryPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_characteristic_package_Pack~",
                table: "characteristic_package",
                column: "PackagePassportId");

            migrationBuilder.AddForeignKey(
                name: "FK_characteristic_package_pack~",
                table: "characteristic_package",
                column: "PackagePassportId",
                principalTable: "package_passport",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_radionuclid_characteristic_~",
                table: "radionuclid",
                column: "CharacteristicPrimaryPackageId",
                principalTable: "characteristic_package",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_characteristic_package_pack~",
                table: "characteristic_package");

            migrationBuilder.DropForeignKey(
                name: "FK_radionuclid_characteristic_~",
                table: "radionuclid");

            migrationBuilder.DropIndex(
                name: "IX_radionuclid_CharacteristicP~",
                table: "radionuclid");

            migrationBuilder.DropIndex(
                name: "IX_characteristic_package_Pack~",
                table: "characteristic_package");

            migrationBuilder.DropColumn(
                name: "CharacteristicPrimaryPackageId",
                table: "radionuclid");

            migrationBuilder.DropColumn(
                name: "PackagePassportId",
                table: "characteristic_package");

            migrationBuilder.CreateIndex(
                name: "IX_radionuclid_CharacteristicId",
                table: "radionuclid",
                column: "CharacteristicId");

            migrationBuilder.CreateIndex(
                name: "IX_characteristic_package_Pass~",
                table: "characteristic_package",
                column: "PassportId");

            migrationBuilder.AddForeignKey(
                name: "FK_characteristic_package_pack~",
                table: "characteristic_package",
                column: "PassportId",
                principalTable: "package_passport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_radionuclid_characteristic_~",
                table: "radionuclid",
                column: "CharacteristicId",
                principalTable: "characteristic_package",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

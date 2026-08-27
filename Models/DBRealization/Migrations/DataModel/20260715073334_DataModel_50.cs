using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_50 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_characteristic_package_pack~",
                table: "characteristic_package");

            migrationBuilder.DropForeignKey(
                name: "FK_radionuclid_characteristic_~",
                table: "radionuclid");

            migrationBuilder.AddForeignKey(
                name: "FK_characteristic_package_pack~",
                table: "characteristic_package",
                column: "PassportId",
                principalTable: "package_passport",
                principalColumn: "Id", onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_radionuclid_characteristic_~",
                table: "radionuclid",
                column: "CharacteristicId",
                principalTable: "characteristic_package",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_characteristic_package_pack~",
                table: "characteristic_package");

            migrationBuilder.DropForeignKey(
                name: "FK_radionuclid_characteristic_~",
                table: "radionuclid");

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

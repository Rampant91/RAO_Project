using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataExportModel
{
    public partial class DataExportModel_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PackName_Hidden_Priv",
                table: "form_22",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PackName_Hidden_Priv2",
                table: "form_22",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PackType_Hidden_Priv",
                table: "form_22",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PackType_Hidden_Priv2",
                table: "form_22",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StoragePlaceCode_Hidden_Priv",
                table: "form_22",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StoragePlaceCode_Hidden_Priv2",
                table: "form_22",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StoragePlaceName_Hidden_Priv",
                table: "form_22",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StoragePlaceName_Hidden_Priv2",
                table: "form_22",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackName_Hidden_Priv",
                table: "form_22");

            migrationBuilder.DropColumn(
                name: "PackName_Hidden_Priv2",
                table: "form_22");

            migrationBuilder.DropColumn(
                name: "PackType_Hidden_Priv",
                table: "form_22");

            migrationBuilder.DropColumn(
                name: "PackType_Hidden_Priv2",
                table: "form_22");

            migrationBuilder.DropColumn(
                name: "StoragePlaceCode_Hidden_Priv",
                table: "form_22");

            migrationBuilder.DropColumn(
                name: "StoragePlaceCode_Hidden_Priv2",
                table: "form_22");

            migrationBuilder.DropColumn(
                name: "StoragePlaceName_Hidden_Priv",
                table: "form_22");

            migrationBuilder.DropColumn(
                name: "StoragePlaceName_Hidden_Priv2",
                table: "form_22");
        }
    }
}

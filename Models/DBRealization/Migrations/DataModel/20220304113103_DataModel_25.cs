using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_25 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StoragePlaceName_Hidden_Priv2",
                table: "form_22",
                newName: "_StoragePlaceName_Hidden_Set");

            migrationBuilder.RenameColumn(
                name: "StoragePlaceName_Hidden_Priv",
                table: "form_22",
                newName: "_StoragePlaceName_Hidden_Get");

            migrationBuilder.RenameColumn(
                name: "StoragePlaceCode_Hidden_Priv2",
                table: "form_22",
                newName: "_StoragePlaceCode_Hidden_Set");

            migrationBuilder.RenameColumn(
                name: "StoragePlaceCode_Hidden_Priv",
                table: "form_22",
                newName: "_StoragePlaceCode_Hidden_Get");

            migrationBuilder.RenameColumn(
                name: "PackType_Hidden_Priv2",
                table: "form_22",
                newName: "_PackType_Hidden_Set");

            migrationBuilder.RenameColumn(
                name: "PackType_Hidden_Priv",
                table: "form_22",
                newName: "_PackType_Hidden_Get");

            migrationBuilder.RenameColumn(
                name: "PackName_Hidden_Priv2",
                table: "form_22",
                newName: "_PackName_Hidden_Set");

            migrationBuilder.RenameColumn(
                name: "PackName_Hidden_Priv",
                table: "form_22",
                newName: "_PackName_Hidden_Get");

            migrationBuilder.AddColumn<bool>(
                name: "SumGroup_DB",
                table: "form_22",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "_BaseColor",
                table: "form_22",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "SumGroup_DB",
                table: "form_21",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "_BaseColor",
                table: "form_21",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumGroup_DB",
                table: "form_22");

            migrationBuilder.DropColumn(
                name: "_BaseColor",
                table: "form_22");

            migrationBuilder.DropColumn(
                name: "SumGroup_DB",
                table: "form_21");

            migrationBuilder.DropColumn(
                name: "_BaseColor",
                table: "form_21");

            migrationBuilder.RenameColumn(
                name: "_StoragePlaceName_Hidden_Set",
                table: "form_22",
                newName: "StoragePlaceName_Hidden_Priv2");

            migrationBuilder.RenameColumn(
                name: "_StoragePlaceName_Hidden_Get",
                table: "form_22",
                newName: "StoragePlaceName_Hidden_Priv");

            migrationBuilder.RenameColumn(
                name: "_StoragePlaceCode_Hidden_Set",
                table: "form_22",
                newName: "StoragePlaceCode_Hidden_Priv2");

            migrationBuilder.RenameColumn(
                name: "_StoragePlaceCode_Hidden_Get",
                table: "form_22",
                newName: "StoragePlaceCode_Hidden_Priv");

            migrationBuilder.RenameColumn(
                name: "_PackType_Hidden_Set",
                table: "form_22",
                newName: "PackType_Hidden_Priv2");

            migrationBuilder.RenameColumn(
                name: "_PackType_Hidden_Get",
                table: "form_22",
                newName: "PackType_Hidden_Priv");

            migrationBuilder.RenameColumn(
                name: "_PackName_Hidden_Set",
                table: "form_22",
                newName: "PackName_Hidden_Priv2");

            migrationBuilder.RenameColumn(
                name: "_PackName_Hidden_Get",
                table: "form_22",
                newName: "PackName_Hidden_Priv");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_4 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        #region Add column PackName_Hidden_Priv/2, PackType_Hidden_Priv/2, StoragePlaceCode_Hidden_Priv/2, StoragePlaceName_Hidden_Priv/2 in form_22
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
        #endregion
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
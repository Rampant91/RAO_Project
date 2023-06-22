using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_6 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        #region Add column MassInPack_Hidden_Priv/2, VolumeInPack_Hidden_Priv/2 in form_22
        migrationBuilder.AddColumn<bool>(
            name: "MassInPack_Hidden_Priv",
            table: "form_22",
            type: "BOOLEAN",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "MassInPack_Hidden_Priv2",
            table: "form_22",
            type: "BOOLEAN",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "VolumeInPack_Hidden_Priv",
            table: "form_22",
            type: "BOOLEAN",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "VolumeInPack_Hidden_Priv2",
            table: "form_22",
            type: "BOOLEAN",
            nullable: false,
            defaultValue: false);
        #endregion
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "MassInPack_Hidden_Priv",
            table: "form_22");

        migrationBuilder.DropColumn(
            name: "MassInPack_Hidden_Priv2",
            table: "form_22");

        migrationBuilder.DropColumn(
            name: "VolumeInPack_Hidden_Priv",
            table: "form_22");

        migrationBuilder.DropColumn(
            name: "VolumeInPack_Hidden_Priv2",
            table: "form_22");
    }
}
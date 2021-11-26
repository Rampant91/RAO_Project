using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataExportModel
{
    public partial class DataExportModel_6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
}

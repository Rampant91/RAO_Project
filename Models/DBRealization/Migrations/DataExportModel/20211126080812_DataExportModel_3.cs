using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataExportModel
{
    public partial class DataExportModel_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FcpNumber_Hidden_Priv",
                table: "form_22",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Subsidy_Hidden_Priv",
                table: "form_22",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FcpNumber_Hidden_Priv",
                table: "form_22");

            migrationBuilder.DropColumn(
                name: "Subsidy_Hidden_Priv",
                table: "form_22");
        }
    }
}

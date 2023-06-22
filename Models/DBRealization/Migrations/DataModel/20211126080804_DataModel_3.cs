using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_3 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        #region Add column FcpNumber_Hidden_Priv, Subsidy_Hidden_Priv in form_22
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
        #endregion
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
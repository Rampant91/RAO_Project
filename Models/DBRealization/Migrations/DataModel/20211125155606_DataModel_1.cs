using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_1 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        #region Add column CodeRAOout_Hidden_Priv, StatusRAOIn_Hidden_Priv, StatusRAOout_Hidden_Priv in form_21
        migrationBuilder.AddColumn<bool>(
            name: "CodeRAOout_Hidden_Priv",
            table: "form_21",
            type: "BOOLEAN",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "StatusRAOIn_Hidden_Priv",
            table: "form_21",
            type: "BOOLEAN",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "StatusRAOout_Hidden_Priv",
            table: "form_21",
            type: "BOOLEAN",
            nullable: false,
            defaultValue: false);
        #endregion
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CodeRAOout_Hidden_Priv",
            table: "form_21");

        migrationBuilder.DropColumn(
            name: "StatusRAOIn_Hidden_Priv",
            table: "form_21");

        migrationBuilder.DropColumn(
            name: "StatusRAOout_Hidden_Priv",
            table: "form_21");
    }
}
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_2 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        #region Add column MachineCode_Hidden_Priv2, MachinePower_Hidden_Priv2, NumberOfHoursPerYear_Hidden_P~1, RefineMachineName_Hidden_Priv2 in form_21
        migrationBuilder.AddColumn<bool>(
            name: "MachineCode_Hidden_Priv2",
            table: "form_21",
            type: "BOOLEAN",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "MachinePower_Hidden_Priv2",
            table: "form_21",
            type: "BOOLEAN",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "NumberOfHoursPerYear_Hidden_P~1",
            table: "form_21",
            type: "BOOLEAN",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "RefineMachineName_Hidden_Priv2",
            table: "form_21",
            type: "BOOLEAN",
            nullable: false,
            defaultValue: false);
        #endregion
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "MachineCode_Hidden_Priv2",
            table: "form_21");

        migrationBuilder.DropColumn(
            name: "MachinePower_Hidden_Priv2",
            table: "form_21");

        migrationBuilder.DropColumn(
            name: "NumberOfHoursPerYear_Hidden_P~1",
            table: "form_21");

        migrationBuilder.DropColumn(
            name: "RefineMachineName_Hidden_Priv2",
            table: "form_21");
    }
}
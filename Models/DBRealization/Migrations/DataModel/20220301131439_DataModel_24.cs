using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_24 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "RefineMachineName_Hidden_Priv2",
            table: "form_21",
            newName: "_RefineMachineName_Hidden_Set");

        migrationBuilder.RenameColumn(
            name: "RefineMachineName_Hidden_Priv",
            table: "form_21",
            newName: "_RefineMachineName_Hidden_Get");

        migrationBuilder.RenameColumn(
            name: "NumberOfHoursPerYear_Hidden_P~1",
            table: "form_21",
            newName: "_NumberOfHoursPerYear_Hidden_S~");

        migrationBuilder.RenameColumn(
            name: "NumberOfHoursPerYear_Hidden_Pr~",
            table: "form_21",
            newName: "_NumberOfHoursPerYear_Hidden_G~");

        migrationBuilder.RenameColumn(
            name: "MachineCode_Hidden_Priv2",
            table: "form_21",
            newName: "_MachineCode_Hidden_Set");

        migrationBuilder.RenameColumn(
            name: "MachineCode_Hidden_Priv",
            table: "form_21",
            newName: "_MachineCode_Hidden_Get");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "_RefineMachineName_Hidden_Set",
            table: "form_21",
            newName: "RefineMachineName_Hidden_Priv2");

        migrationBuilder.RenameColumn(
            name: "_RefineMachineName_Hidden_Get",
            table: "form_21",
            newName: "RefineMachineName_Hidden_Priv");

        migrationBuilder.RenameColumn(
            name: "_NumberOfHoursPerYear_Hidden_S~",
            table: "form_21",
            newName: "NumberOfHoursPerYear_Hidden_P~1");

        migrationBuilder.RenameColumn(
            name: "_NumberOfHoursPerYear_Hidden_G~",
            table: "form_21",
            newName: "NumberOfHoursPerYear_Hidden_Pr~");

        migrationBuilder.RenameColumn(
            name: "_MachineCode_Hidden_Set",
            table: "form_21",
            newName: "MachineCode_Hidden_Priv2");

        migrationBuilder.RenameColumn(
            name: "_MachineCode_Hidden_Get",
            table: "form_21",
            newName: "MachineCode_Hidden_Priv");
    }
}
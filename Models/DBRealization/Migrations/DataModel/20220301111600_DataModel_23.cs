using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_23 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Order",
            table: "ReportCollection_DbSet");

        migrationBuilder.RenameColumn(
            name: "MachinePower_Hidden_Priv2",
            table: "form_21",
            newName: "_MachinePower_Hidden_Set");

        migrationBuilder.RenameColumn(
            name: "MachinePower_Hidden_Priv",
            table: "form_21",
            newName: "_MachinePower_Hidden_Get");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "_MachinePower_Hidden_Set",
            table: "form_21",
            newName: "MachinePower_Hidden_Priv2");

        migrationBuilder.RenameColumn(
            name: "_MachinePower_Hidden_Get",
            table: "form_21",
            newName: "MachinePower_Hidden_Priv");

        migrationBuilder.AddColumn<int>(
            name: "Order",
            table: "ReportCollection_DbSet",
            type: "INTEGER",
            nullable: false,
            defaultValue: 0);
    }
}
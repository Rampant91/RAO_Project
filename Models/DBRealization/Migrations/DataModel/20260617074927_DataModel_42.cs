using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_42 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_21");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_22");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_23");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_24");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_25");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_26");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_27");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_28");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_29");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_210");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_211");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_212");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_21",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_22",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_23",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_24",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_25",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_26",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_27",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_28",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_29",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_210",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_211",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_212",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);
    }
}
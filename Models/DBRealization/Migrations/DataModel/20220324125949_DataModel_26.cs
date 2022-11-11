using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_26 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "NumberInOrderSum_DB",
            table: "form_22",
            type: "BLOB SUB_TYPE TEXT",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "NumberInOrderSum_DB",
            table: "form_21",
            type: "BLOB SUB_TYPE TEXT",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "NumberInOrderSum_DB",
            table: "form_22");

        migrationBuilder.DropColumn(
            name: "NumberInOrderSum_DB",
            table: "form_21");
    }
}
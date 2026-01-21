using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_38 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate_DB",
                table: "form_57");

            migrationBuilder.DropColumn(
                name: "StartDate_DB",
                table: "form_57");

            migrationBuilder.AddColumn<string>(
                name: "Note_DB",
                table: "form_57",
                type: "BLOB SUB_TYPE TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderOrRecieverOKPO_DB",
                table: "form_55",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(14)",
                oldMaxLength: 14,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderOrRecieverOKPO_DB",
                table: "form_53",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(14)",
                oldMaxLength: 14,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note_DB",
                table: "form_57");

            migrationBuilder.AddColumn<string>(
                name: "EndDate_DB",
                table: "form_57",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartDate_DB",
                table: "form_57",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderOrRecieverOKPO_DB",
                table: "form_55",
                type: "varchar(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderOrRecieverOKPO_DB",
                table: "form_53",
                type: "varchar(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(64)",
                oldMaxLength: 64,
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "CorrectionNumber",
                table: "package_passport",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectionNumber",
                table: "package_passport");
        }
    }
}

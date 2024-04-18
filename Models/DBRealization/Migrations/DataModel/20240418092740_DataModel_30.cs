using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartPeriod_DB",
                table: "ReportCollection_DbSet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StartPeriod_DB",
                table: "ReportCollection_DbSet",
                type: "BLOB SUB_TYPE TEXT",
                nullable: true);
        }
    }
}

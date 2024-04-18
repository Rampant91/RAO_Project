using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartPeriodNew_DB",
                table: "ReportCollection_DbSet",
                newName: "StartPeriod_DB");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartPeriod_DB",
                table: "ReportCollection_DbSet",
                newName: "StartPeriodNew_DB");
        }
    }
}

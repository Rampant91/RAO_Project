using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE \"ReportCollection_DbSet\" SET \"StartPeriodNew_DB\" = \"StartPeriod_DB\"");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

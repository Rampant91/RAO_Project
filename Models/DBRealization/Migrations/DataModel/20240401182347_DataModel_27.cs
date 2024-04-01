using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet",
                column: "ReportsId",
                principalTable: "ReportsCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet",
                column: "ReportsId",
                principalTable: "ReportsCollection_DbSet",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_27 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportsCollection_DbSet_Rep~",
                table: "ReportsCollection_DbSet");

            migrationBuilder.DropIndex(
                name: "IX_ReportsCollection_DbSet_Mas~",
                table: "ReportsCollection_DbSet");

            migrationBuilder.DropIndex(
                name: "IX_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet");

            migrationBuilder.DropColumn(
                name: "Master_DBId",
                table: "ReportsCollection_DbSet");

            migrationBuilder.AddColumn<int>(
                name: "ReportNew",
                table: "ReportsCollection_DbSet",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet",
                column: "ReportsId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet");

            migrationBuilder.DropColumn(
                name: "ReportNew",
                table: "ReportsCollection_DbSet");

            migrationBuilder.AddColumn<int>(
                name: "Master_DBId",
                table: "ReportsCollection_DbSet",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportsCollection_DbSet_Mas~",
                table: "ReportsCollection_DbSet",
                column: "Master_DBId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet",
                column: "ReportsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportsCollection_DbSet_Rep~",
                table: "ReportsCollection_DbSet",
                column: "Master_DBId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");
        }
    }
}

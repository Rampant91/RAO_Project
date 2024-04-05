using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_28 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_form_10_ReportCollection_Db~",
                table: "form_10");

            migrationBuilder.DropForeignKey(
                name: "FK_form_11_ReportCollection_Db~",
                table: "form_11");

            migrationBuilder.DropForeignKey(
                name: "FK_notes_ReportCollection_DbSe~",
                table: "notes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportsCollection_DbSet_DBO~",
                table: "ReportsCollection_DbSet");

            migrationBuilder.AddForeignKey(
                name: "FK_form_10_ReportCollection_Db~",
                table: "form_10",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_11_ReportCollection_Db~",
                table: "form_11",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notes_ReportCollection_DbSe~",
                table: "notes",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportsCollection_DbSet_DBO~",
                table: "ReportsCollection_DbSet",
                column: "DBObservableId",
                principalTable: "DBObservable_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_form_10_ReportCollection_Db~",
                table: "form_10");

            migrationBuilder.DropForeignKey(
                name: "FK_form_11_ReportCollection_Db~",
                table: "form_11");

            migrationBuilder.DropForeignKey(
                name: "FK_notes_ReportCollection_DbSe~",
                table: "notes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportsCollection_DbSet_DBO~",
                table: "ReportsCollection_DbSet");

            migrationBuilder.AddForeignKey(
                name: "FK_form_10_ReportCollection_Db~",
                table: "form_10",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_11_ReportCollection_Db~",
                table: "form_11",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_notes_ReportCollection_DbSe~",
                table: "notes",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportsCollection_DbSet_DBO~",
                table: "ReportsCollection_DbSet",
                column: "DBObservableId",
                principalTable: "DBObservable_DbSet",
                principalColumn: "Id");
        }
    }
}

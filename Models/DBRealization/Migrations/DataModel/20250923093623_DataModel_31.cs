using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_31 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_form_40_ReportCollection_Db~",
                table: "form_40");

            migrationBuilder.DropForeignKey(
                name: "FK_form_41_ReportCollection_Db~",
                table: "form_41");

            migrationBuilder.AddForeignKey(
                name: "FK_form_40_ReportCollection_Db~",
                table: "form_40",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_41_ReportCollection_Db~",
                table: "form_41",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_form_40_ReportCollection_Db~",
                table: "form_40");

            migrationBuilder.DropForeignKey(
                name: "FK_form_41_ReportCollection_Db~",
                table: "form_41");

            migrationBuilder.AddForeignKey(
                name: "FK_form_40_ReportCollection_Db~",
                table: "form_40",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_41_ReportCollection_Db~",
                table: "form_41",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");
        }
    }
}

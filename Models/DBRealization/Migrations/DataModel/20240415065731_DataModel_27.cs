using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_27 : Migration
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
                name: "FK_form_12_ReportCollection_Db~",
                table: "form_12");

            migrationBuilder.DropForeignKey(
                name: "FK_form_13_ReportCollection_Db~",
                table: "form_13");

            migrationBuilder.DropForeignKey(
                name: "FK_form_14_ReportCollection_Db~",
                table: "form_14");

            migrationBuilder.DropForeignKey(
                name: "FK_form_15_ReportCollection_Db~",
                table: "form_15");

            migrationBuilder.DropForeignKey(
                name: "FK_form_16_ReportCollection_Db~",
                table: "form_16");

            migrationBuilder.DropForeignKey(
                name: "FK_form_17_ReportCollection_Db~",
                table: "form_17");

            migrationBuilder.DropForeignKey(
                name: "FK_form_18_ReportCollection_Db~",
                table: "form_18");

            migrationBuilder.DropForeignKey(
                name: "FK_form_19_ReportCollection_Db~",
                table: "form_19");

            migrationBuilder.DropForeignKey(
                name: "FK_form_20_ReportCollection_Db~",
                table: "form_20");

            migrationBuilder.DropForeignKey(
                name: "FK_form_21_ReportCollection_Db~",
                table: "form_21");

            migrationBuilder.DropForeignKey(
                name: "FK_form_210_ReportCollection_D~",
                table: "form_210");

            migrationBuilder.DropForeignKey(
                name: "FK_form_211_ReportCollection_D~",
                table: "form_211");

            migrationBuilder.DropForeignKey(
                name: "FK_form_212_ReportCollection_D~",
                table: "form_212");

            migrationBuilder.DropForeignKey(
                name: "FK_form_22_ReportCollection_Db~",
                table: "form_22");

            migrationBuilder.DropForeignKey(
                name: "FK_form_23_ReportCollection_Db~",
                table: "form_23");

            migrationBuilder.DropForeignKey(
                name: "FK_form_24_ReportCollection_Db~",
                table: "form_24");

            migrationBuilder.DropForeignKey(
                name: "FK_form_25_ReportCollection_Db~",
                table: "form_25");

            migrationBuilder.DropForeignKey(
                name: "FK_form_26_ReportCollection_Db~",
                table: "form_26");

            migrationBuilder.DropForeignKey(
                name: "FK_form_27_ReportCollection_Db~",
                table: "form_27");

            migrationBuilder.DropForeignKey(
                name: "FK_form_28_ReportCollection_Db~",
                table: "form_28");

            migrationBuilder.DropForeignKey(
                name: "FK_form_29_ReportCollection_Db~",
                table: "form_29");

            migrationBuilder.DropForeignKey(
                name: "FK_notes_ReportCollection_DbSe~",
                table: "notes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet");

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
                name: "FK_form_12_ReportCollection_Db~",
                table: "form_12",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_13_ReportCollection_Db~",
                table: "form_13",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_14_ReportCollection_Db~",
                table: "form_14",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_15_ReportCollection_Db~",
                table: "form_15",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_16_ReportCollection_Db~",
                table: "form_16",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_17_ReportCollection_Db~",
                table: "form_17",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_18_ReportCollection_Db~",
                table: "form_18",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_19_ReportCollection_Db~",
                table: "form_19",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_20_ReportCollection_Db~",
                table: "form_20",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_21_ReportCollection_Db~",
                table: "form_21",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_210_ReportCollection_D~",
                table: "form_210",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_211_ReportCollection_D~",
                table: "form_211",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_212_ReportCollection_D~",
                table: "form_212",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_22_ReportCollection_Db~",
                table: "form_22",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_23_ReportCollection_Db~",
                table: "form_23",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_24_ReportCollection_Db~",
                table: "form_24",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_25_ReportCollection_Db~",
                table: "form_25",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_26_ReportCollection_Db~",
                table: "form_26",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_27_ReportCollection_Db~",
                table: "form_27",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_28_ReportCollection_Db~",
                table: "form_28",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_form_29_ReportCollection_Db~",
                table: "form_29",
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
                name: "FK_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet",
                column: "ReportsId",
                principalTable: "ReportsCollection_DbSet",
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
                name: "FK_form_12_ReportCollection_Db~",
                table: "form_12");

            migrationBuilder.DropForeignKey(
                name: "FK_form_13_ReportCollection_Db~",
                table: "form_13");

            migrationBuilder.DropForeignKey(
                name: "FK_form_14_ReportCollection_Db~",
                table: "form_14");

            migrationBuilder.DropForeignKey(
                name: "FK_form_15_ReportCollection_Db~",
                table: "form_15");

            migrationBuilder.DropForeignKey(
                name: "FK_form_16_ReportCollection_Db~",
                table: "form_16");

            migrationBuilder.DropForeignKey(
                name: "FK_form_17_ReportCollection_Db~",
                table: "form_17");

            migrationBuilder.DropForeignKey(
                name: "FK_form_18_ReportCollection_Db~",
                table: "form_18");

            migrationBuilder.DropForeignKey(
                name: "FK_form_19_ReportCollection_Db~",
                table: "form_19");

            migrationBuilder.DropForeignKey(
                name: "FK_form_20_ReportCollection_Db~",
                table: "form_20");

            migrationBuilder.DropForeignKey(
                name: "FK_form_21_ReportCollection_Db~",
                table: "form_21");

            migrationBuilder.DropForeignKey(
                name: "FK_form_210_ReportCollection_D~",
                table: "form_210");

            migrationBuilder.DropForeignKey(
                name: "FK_form_211_ReportCollection_D~",
                table: "form_211");

            migrationBuilder.DropForeignKey(
                name: "FK_form_212_ReportCollection_D~",
                table: "form_212");

            migrationBuilder.DropForeignKey(
                name: "FK_form_22_ReportCollection_Db~",
                table: "form_22");

            migrationBuilder.DropForeignKey(
                name: "FK_form_23_ReportCollection_Db~",
                table: "form_23");

            migrationBuilder.DropForeignKey(
                name: "FK_form_24_ReportCollection_Db~",
                table: "form_24");

            migrationBuilder.DropForeignKey(
                name: "FK_form_25_ReportCollection_Db~",
                table: "form_25");

            migrationBuilder.DropForeignKey(
                name: "FK_form_26_ReportCollection_Db~",
                table: "form_26");

            migrationBuilder.DropForeignKey(
                name: "FK_form_27_ReportCollection_Db~",
                table: "form_27");

            migrationBuilder.DropForeignKey(
                name: "FK_form_28_ReportCollection_Db~",
                table: "form_28");

            migrationBuilder.DropForeignKey(
                name: "FK_form_29_ReportCollection_Db~",
                table: "form_29");

            migrationBuilder.DropForeignKey(
                name: "FK_notes_ReportCollection_DbSe~",
                table: "notes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet");

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
                name: "FK_form_12_ReportCollection_Db~",
                table: "form_12",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_13_ReportCollection_Db~",
                table: "form_13",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_14_ReportCollection_Db~",
                table: "form_14",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_15_ReportCollection_Db~",
                table: "form_15",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_16_ReportCollection_Db~",
                table: "form_16",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_17_ReportCollection_Db~",
                table: "form_17",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_18_ReportCollection_Db~",
                table: "form_18",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_19_ReportCollection_Db~",
                table: "form_19",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_20_ReportCollection_Db~",
                table: "form_20",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_21_ReportCollection_Db~",
                table: "form_21",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_210_ReportCollection_D~",
                table: "form_210",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_211_ReportCollection_D~",
                table: "form_211",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_212_ReportCollection_D~",
                table: "form_212",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_22_ReportCollection_Db~",
                table: "form_22",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_23_ReportCollection_Db~",
                table: "form_23",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_24_ReportCollection_Db~",
                table: "form_24",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_25_ReportCollection_Db~",
                table: "form_25",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_26_ReportCollection_Db~",
                table: "form_26",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_27_ReportCollection_Db~",
                table: "form_27",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_28_ReportCollection_Db~",
                table: "form_28",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_form_29_ReportCollection_Db~",
                table: "form_29",
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
                name: "FK_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet",
                column: "ReportsId",
                principalTable: "ReportsCollection_DbSet",
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

using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_7 : Migration
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
                name: "FK_ReportsCollection_DbSet_DBO~",
                table: "ReportsCollection_DbSet");

            migrationBuilder.DropIndex(
                name: "IX_ReportsCollection_DbSet_DBO~",
                table: "ReportsCollection_DbSet");

            migrationBuilder.DropIndex(
                name: "IX_notes_ReportId",
                table: "notes");

            migrationBuilder.DropIndex(
                name: "IX_form_29_ReportId",
                table: "form_29");

            migrationBuilder.DropIndex(
                name: "IX_form_28_ReportId",
                table: "form_28");

            migrationBuilder.DropIndex(
                name: "IX_form_27_ReportId",
                table: "form_27");

            migrationBuilder.DropIndex(
                name: "IX_form_26_ReportId",
                table: "form_26");

            migrationBuilder.DropIndex(
                name: "IX_form_25_ReportId",
                table: "form_25");

            migrationBuilder.DropIndex(
                name: "IX_form_24_ReportId",
                table: "form_24");

            migrationBuilder.DropIndex(
                name: "IX_form_23_ReportId",
                table: "form_23");

            migrationBuilder.DropIndex(
                name: "IX_form_22_ReportId",
                table: "form_22");

            migrationBuilder.DropIndex(
                name: "IX_form_212_ReportId",
                table: "form_212");

            migrationBuilder.DropIndex(
                name: "IX_form_211_ReportId",
                table: "form_211");

            migrationBuilder.DropIndex(
                name: "IX_form_210_ReportId",
                table: "form_210");

            migrationBuilder.DropIndex(
                name: "IX_form_21_ReportId",
                table: "form_21");

            migrationBuilder.DropIndex(
                name: "IX_form_20_ReportId",
                table: "form_20");

            migrationBuilder.DropIndex(
                name: "IX_form_19_ReportId",
                table: "form_19");

            migrationBuilder.DropIndex(
                name: "IX_form_18_ReportId",
                table: "form_18");

            migrationBuilder.DropIndex(
                name: "IX_form_17_ReportId",
                table: "form_17");

            migrationBuilder.DropIndex(
                name: "IX_form_16_ReportId",
                table: "form_16");

            migrationBuilder.DropIndex(
                name: "IX_form_15_ReportId",
                table: "form_15");

            migrationBuilder.DropIndex(
                name: "IX_form_14_ReportId",
                table: "form_14");

            migrationBuilder.DropIndex(
                name: "IX_form_13_ReportId",
                table: "form_13");

            migrationBuilder.DropIndex(
                name: "IX_form_12_ReportId",
                table: "form_12");

            migrationBuilder.DropIndex(
                name: "IX_form_11_ReportId",
                table: "form_11");

            migrationBuilder.DropIndex(
                name: "IX_form_10_ReportId",
                table: "form_10");

            migrationBuilder.DropColumn(
                name: "DBObservableId",
                table: "ReportsCollection_DbSet");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "notes");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_29");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_28");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_27");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_26");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_25");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_24");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_23");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_22");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_212");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_211");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_210");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_21");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_20");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_19");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_18");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_17");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_16");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_15");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_14");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_13");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_12");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_11");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "form_10");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DBObservableId",
                table: "ReportsCollection_DbSet",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "notes",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_29",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_28",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_27",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_26",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_25",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_24",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_23",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_22",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_212",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_211",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_210",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_21",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_20",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_19",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_18",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_17",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_16",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_15",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_14",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_13",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_12",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_11",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "form_10",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportsCollection_DbSet_DBO~",
                table: "ReportsCollection_DbSet",
                column: "DBObservableId");

            migrationBuilder.CreateIndex(
                name: "IX_notes_ReportId",
                table: "notes",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_29_ReportId",
                table: "form_29",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_28_ReportId",
                table: "form_28",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_27_ReportId",
                table: "form_27",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_26_ReportId",
                table: "form_26",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_25_ReportId",
                table: "form_25",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_24_ReportId",
                table: "form_24",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_23_ReportId",
                table: "form_23",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_22_ReportId",
                table: "form_22",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_212_ReportId",
                table: "form_212",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_211_ReportId",
                table: "form_211",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_210_ReportId",
                table: "form_210",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_21_ReportId",
                table: "form_21",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_20_ReportId",
                table: "form_20",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_19_ReportId",
                table: "form_19",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_18_ReportId",
                table: "form_18",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_17_ReportId",
                table: "form_17",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_16_ReportId",
                table: "form_16",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_15_ReportId",
                table: "form_15",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_14_ReportId",
                table: "form_14",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_13_ReportId",
                table: "form_13",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_12_ReportId",
                table: "form_12",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_11_ReportId",
                table: "form_11",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_10_ReportId",
                table: "form_10",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_form_10_ReportCollection_Db~",
                table: "form_10",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_11_ReportCollection_Db~",
                table: "form_11",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_12_ReportCollection_Db~",
                table: "form_12",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_13_ReportCollection_Db~",
                table: "form_13",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_14_ReportCollection_Db~",
                table: "form_14",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_15_ReportCollection_Db~",
                table: "form_15",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_16_ReportCollection_Db~",
                table: "form_16",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_17_ReportCollection_Db~",
                table: "form_17",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_18_ReportCollection_Db~",
                table: "form_18",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_19_ReportCollection_Db~",
                table: "form_19",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_20_ReportCollection_Db~",
                table: "form_20",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_21_ReportCollection_Db~",
                table: "form_21",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_210_ReportCollection_D~",
                table: "form_210",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_211_ReportCollection_D~",
                table: "form_211",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_212_ReportCollection_D~",
                table: "form_212",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_22_ReportCollection_Db~",
                table: "form_22",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_23_ReportCollection_Db~",
                table: "form_23",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_24_ReportCollection_Db~",
                table: "form_24",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_25_ReportCollection_Db~",
                table: "form_25",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_26_ReportCollection_Db~",
                table: "form_26",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_27_ReportCollection_Db~",
                table: "form_27",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_28_ReportCollection_Db~",
                table: "form_28",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_form_29_ReportCollection_Db~",
                table: "form_29",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_notes_ReportCollection_DbSe~",
                table: "notes",
                column: "ReportId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportsCollection_DbSet_DBO~",
                table: "ReportsCollection_DbSet",
                column: "DBObservableId",
                principalTable: "DBObservable_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

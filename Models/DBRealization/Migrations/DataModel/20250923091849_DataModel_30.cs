using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "form_40",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    SubjectRF_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Year_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NameOrganUprav_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ShortNameOrganUprav_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    AddressOrganUprav_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    GradeFioDirectorOrganUprav_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    GradeFioExecutorOrganUprav_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    TelephoneOrganUprav_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    FaxOrganUprav_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    EmailOrganUprav_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NameRiac_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ShortNameRiac_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    AddressRiac_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    GradeFioDirectorRiac_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    GradeFioExecutorRiac_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    TelephoneRiac_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    FaxRiac_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    EmailRiac_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_40", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_40_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "form_41",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    RegNo_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okpo_DB = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true),
                    OrganizationName_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    LicenseOrRegistrationInfo_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumOfFormsWithInventarizationI = table.Column<int>(name: "NumOfFormsWithInventarizationI~", type: "INTEGER", nullable: false),
                    NumOfFormsWithoutInventarizati = table.Column<int>(name: "NumOfFormsWithoutInventarizati~", type: "INTEGER", nullable: false),
                    NumOfForms212_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    Note_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_41", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_41_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_form_40_ReportId",
                table: "form_40",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_41_ReportId",
                table: "form_41",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "form_40");

            migrationBuilder.DropTable(
                name: "form_41");
        }
    }
}

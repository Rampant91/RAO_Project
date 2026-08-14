using System;
using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_51 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "form_30",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    RegNo_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OrganUprav_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SubjectRF_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    JurLico_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ShortJurLico_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    JurLicoAddress_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    JurLicoFactAddress_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    GradeFIO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Telephone_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Fax_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Email_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okpo_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okved_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okogu_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Oktmo_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Inn_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Kpp_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okopf_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okfs_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_30", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_30_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_31",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    RecipientName_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    RecipientJurLicoAddress_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    RecipientWorkplaceAddress_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    LicenseNum_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    ValidityPeriod_DB = table.Column<DateOnly>(type: "DATE", nullable: true),
                    ExpectedDecisionTimeframe_DB = table.Column<DateOnly>(type: "DATE", nullable: true),
                    FinalUserName_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    FinalUserJurLicoAddress_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    FinalUserWorkplaceAddress_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    FinalUserTelephone_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    FinalUserEmail_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    ApplicationScope_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    ContractNum_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    ContractDate_DB = table.Column<DateOnly>(type: "DATE", nullable: true),
                    ManufacturerOksm_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_31", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_31_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_32",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    AgreementIdNum_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    DeliveryDay_DB = table.Column<DateOnly>(type: "DATE", nullable: true),
                    RecipientName_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    IsRvProduction = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    IsRvTransportation = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    IsRvExploitation = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    IsRvStoring = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    IsRvRecycling = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    IsRaoTransportation = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    IsRaoStoring = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    IsRaoRecycling = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    LicenseNumRv_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    LicenseNumRao_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    LicenseExpirationDateRv_DB = table.Column<DateOnly>(type: "DATE", nullable: true),
                    LicenseExpirationDateRao_DB = table.Column<DateOnly>(type: "DATE", nullable: true),
                    DeliveryAddress_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    RadionuclidCompositionZri_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    TotalActivity_DB = table.Column<double>(type: "DOUBLE PRECISION", nullable: true),
                    TotalCount_DB = table.Column<int>(type: "INTEGER", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_32", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_32_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_31_table",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Form31Id = table.Column<int>(type: "INTEGER", nullable: false),
                    RadionuclidComposition_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Count_DB = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalActivity_DB = table.Column<double>(type: "DOUBLE PRECISION", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_31_table", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_31_table_form_31_Form3~",
                        column: x => x.Form31Id,
                        principalTable: "form_31",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_form_31_table_ReportCollect~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "form_32_table_1",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Form32Id = table.Column<int>(type: "INTEGER", nullable: false),
                    PassportNum_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Type_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    FactoryNum_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    RadionuclidComposition_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    ReleaseDate_DB = table.Column<DateOnly>(type: "DATE", nullable: true),
                    ActivityOnRealeseDate_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    NuclearMaterials_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Category_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    ManufacturerOksm_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    CertificateNum_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    CertificateExpirationDate_DB = table.Column<DateOnly>(type: "DATE", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_32_table_1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_32_table_1_form_32_For~",
                        column: x => x.Form32Id,
                        principalTable: "form_32",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_form_32_table_1_ReportColle~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "form_32_table_2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Form32Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Type_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    IdNum_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    ReleaseYear_DB = table.Column<int>(type: "INTEGER", nullable: true),
                    DepletedUraniumMass_DB = table.Column<double>(type: "DOUBLE PRECISION", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_32_table_2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_32_table_2_form_32_For~",
                        column: x => x.Form32Id,
                        principalTable: "form_32",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_form_32_table_2_ReportColle~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "form_32_table_3",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Form32Id = table.Column<int>(type: "INTEGER", nullable: false),
                    IdName_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    IdValue_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_32_table_3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_32_table_3_form_32_For~",
                        column: x => x.Form32Id,
                        principalTable: "form_32",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_form_32_table_3_ReportColle~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_form_30_ReportId",
                table: "form_30",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_31_ReportId",
                table: "form_31",
                column: "ReportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_form_31_table_Form31Id",
                table: "form_31_table",
                column: "Form31Id");

            migrationBuilder.CreateIndex(
                name: "IX_form_31_table_ReportId",
                table: "form_31_table",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_32_ReportId",
                table: "form_32",
                column: "ReportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_form_32_table_1_Form32Id",
                table: "form_32_table_1",
                column: "Form32Id");

            migrationBuilder.CreateIndex(
                name: "IX_form_32_table_1_ReportId",
                table: "form_32_table_1",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_32_table_2_Form32Id",
                table: "form_32_table_2",
                column: "Form32Id");

            migrationBuilder.CreateIndex(
                name: "IX_form_32_table_2_ReportId",
                table: "form_32_table_2",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_32_table_3_Form32Id",
                table: "form_32_table_3",
                column: "Form32Id");

            migrationBuilder.CreateIndex(
                name: "IX_form_32_table_3_ReportId",
                table: "form_32_table_3",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "form_30");

            migrationBuilder.DropTable(
                name: "form_31_table");

            migrationBuilder.DropTable(
                name: "form_32_table_1");

            migrationBuilder.DropTable(
                name: "form_32_table_2");

            migrationBuilder.DropTable(
                name: "form_32_table_3");

            migrationBuilder.DropTable(
                name: "form_31");

            migrationBuilder.DropTable(
                name: "form_32");
        }
    }
}

using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_36 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "form_50",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Name_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ShortName_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Address_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    GradeFioDirector_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    GradeFioExecutor_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Telephone_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Fax_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Email_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_50", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_50_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_51",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Category_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Quantity_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    Activity_DB = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_51", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_51_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_52",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Category_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true),
                    Quantity_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    Activity_DB = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_52", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_52_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_53",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TypeORI_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    VarietyORI_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    AggregateState_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Activity_DB = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    Mass_DB = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    Volume_DB = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    Quantity_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_53", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_53_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_54",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    TypeORI_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    VarietyORI_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    AggregateState_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Activity_DB = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    Mass_DB = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    Volume_DB = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    Quantity_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_54", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_54_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_55",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Name_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true),
                    Quantity_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    Mass_DB = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_55", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_55_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_56",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Name_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Quantity_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    Mass_DB = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_56", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_56_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_57",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    OKPO_DB = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true),
                    Name_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Recognizance_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    License_DB = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    StartDate_DB = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    EndDate_DB = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true),
                    Practice_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_57", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_57_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_form_50_ReportId",
                table: "form_50",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_51_ReportId",
                table: "form_51",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_52_ReportId",
                table: "form_52",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_53_ReportId",
                table: "form_53",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_54_ReportId",
                table: "form_54",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_55_ReportId",
                table: "form_55",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_56_ReportId",
                table: "form_56",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_57_ReportId",
                table: "form_57",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "form_50");

            migrationBuilder.DropTable(
                name: "form_51");

            migrationBuilder.DropTable(
                name: "form_52");

            migrationBuilder.DropTable(
                name: "form_53");

            migrationBuilder.DropTable(
                name: "form_54");

            migrationBuilder.DropTable(
                name: "form_55");

            migrationBuilder.DropTable(
                name: "form_56");

            migrationBuilder.DropTable(
                name: "form_57");
        }
    }
}

using System;
using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_48 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "storage_point",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    SgukName = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    SgukNameLastUpdate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Code = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    EgrnName = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    EgrnNameLastUpdate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    CadastreNum = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    CadastreNumLastUpdate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    IsRaoPlaced = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_storage_point", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "license_info",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    StorageId = table.Column<int>(type: "INTEGER", nullable: true),
                    LicenseName = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    StartPeriod = table.Column<DateOnly>(type: "DATE", nullable: false),
                    EndPeriod = table.Column<DateOnly>(type: "DATE", nullable: false),
                    ProjectVolume = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    CodeRAO = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    Volume = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    Mass = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    QuantityOZIII = table.Column<int>(type: "INTEGER", nullable: false),
                    SummaryActivity = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    DocumentNumber = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    DocumentDate = table.Column<DateOnly>(type: "DATE", nullable: false),
                    ExpirationDate = table.Column<DateOnly>(type: "DATE", nullable: false),
                    DocumentName = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_license_info", x => x.Id);
                    table.ForeignKey(
                        name: "FK_license_info_storage_point_~",
                        column: x => x.StorageId,
                        principalTable: "storage_point",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_license_info_StorageId",
                table: "license_info",
                column: "StorageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "license_info");

            migrationBuilder.DropTable(
                name: "storage_point");

           
        }
    }
}

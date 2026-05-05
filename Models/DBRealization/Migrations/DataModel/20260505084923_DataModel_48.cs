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
                    Code = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    LicenseName = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    EgrnName = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    CadastreNum = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    ProjectVolume = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    CodeRAO = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    Mass = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    QuantityOZIII = table.Column<int>(type: "INTEGER", nullable: false),
                    SummaryActivity = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    DocumentNumber = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate = table.Column<DateOnly>(type: "DATE", nullable: false),
                    ExpirationDate = table.Column<DateOnly>(type: "DATE", nullable: false),
                    DocumentName = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    IsRaoPlaced = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_storage_point", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "storage_point");

        }
    }
}

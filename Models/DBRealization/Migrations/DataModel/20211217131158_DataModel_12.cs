using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string b = "\"Id\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"ProjectVolume_DB\",\"CodeRAO_DB\",\"Volume_DB\",\"Mass_DB\",\"QuantityOZIII_DB\",\"SummaryActivity_DB\",\"DocumentNumber_DB\",\"" +
               "DocumentDate_DB\",\"ExpirationDate_DB\",\"DocumentName_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"CorrectionNumber_DB\"";
            migrationBuilder.Sql("INSERT INTO \"form_23\" (" + b + ") SELECT " + b + " FROM FORM_23_TEMP");
            migrationBuilder.DropTable("FORM_23_TEMP");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                 name: "form_23_tmp",
                 columns: table => new
                 {
                     Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                     StoragePlaceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                     StoragePlaceCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                     ProjectVolume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                     CodeRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                     Volume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                     Mass_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                     SummaryActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                     DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                     DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                     ExpirationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                     DocumentName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                     ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                     FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                     NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                     NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                     CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
                 });
            migrationBuilder.CreateTable(
                name: "form_23_new_column",
                columns: table => new
                {
                    IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    QuantityOZIII_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true)
                });
            migrationBuilder.CreateTable(
                name: "FORM_23_TEMP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    StoragePlaceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProjectVolume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CodeRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    QuantityOZIII_DB = table.Column<int>(type: "INTEGER", nullable: true),
                    SummaryActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ExpirationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
                });
        }
    }
}

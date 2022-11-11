using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FORM_23_NEW_COLUMN",
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
                    QuantityOZIII_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
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

        protected override void Down(MigrationBuilder migrationBuilder)//NOT READY YET
        {
            string b = "\"Id\",\"Sum_DB\",\"PackName_DB\",\"PackName_Hidden_Priv\",\"PackType_DB\",\"PackType_Hidden_Priv\",\"PackNumber_DB\",\"PackNumber_Hidden_Priv\",\"PackFactoryNumber_DB\",\"" +
 "PackFactoryNumber_Hidden_Priv\",\"FormingDate_DB\",\"FormingDate_Hidden_Priv\",\"Volume_DB\",\"Volume_Hidden_Priv\",\"Mass_DB\",\"Mass_Hidden_Priv\",\"PassportNumber_DB\",\"PassportNumber_Hidden_Priv\",\"" +
 "Radionuclids_DB\",\"SpecificActivity_DB\",\"ProviderOrRecieverOKPO_DB\",\"ProviderOrRecieverOKPO_Hidden_~\",\"TransporterOKPO_DB\",\"TransporterOKPO_Hidden_Priv\",\"StoragePlaceName_DB\",\"" +
 "StoragePlaceName_Hidden_Priv\",\"StoragePlaceCode_DB\",\"StoragePlaceCode_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"CodeRAO_DB\",\"StatusRAO_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"Quantity_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"RefineOrSortRAOCode_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"" +
 "NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"" +
 "DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql($"INSERT INTO \"form_23\" ({b}) SELECT {b} FROM FORM_23_TEMP");
            migrationBuilder.DropTable("FORM_23_TEMP");
        }
    }
}

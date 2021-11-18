using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations
{
    public partial class DataContext_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("form_210");

            migrationBuilder.CreateTable(
                name: "form_210",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    IndicatorName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PlotName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PlotKadastrNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PlotCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    InfectedArea_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AvgGammaRaysDosePower_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MaxGammaRaysDosePower_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    WasteDensityAlpha_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    WasteDensityBeta_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false)
                });

            migrationBuilder.DropTable("form_18");
            migrationBuilder.CreateTable(
                name: "form_18",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    IndividualNumberZHRO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    IndividualNumberZHRO_Hidden_Pr = table.Column<bool>(name: "IndividualNumberZHRO_Hidden_Pr~", type: "BOOLEAN", nullable: false),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PassportNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Volume6_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume6_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Mass7_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass7_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    SaltConcentration_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SaltConcentration_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SpecificActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_Hidden_ = table.Column<bool>(name: "ProviderOrRecieverOKPO_Hidden_~", type: "BOOLEAN", nullable: false),
                    TransporterOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransporterOKPO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    StoragePlaceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    StoragePlaceCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    CodeRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StatusRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume20_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass21_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TritiumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    BetaGammaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AlphaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransuraniumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RefineOrSortRAOCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Subsidy_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                });

            migrationBuilder.DropTable("form_14");
            migrationBuilder.CreateTable(
                name: "form_14",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Name_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Sort_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Activity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ActivityMeasurementDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AggregateState_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    Owner_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransporterOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackType_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("form_210");

            migrationBuilder.CreateTable(
                name: "form_210",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    IndicatorName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PlotName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PlotKadastrNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PlotCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    InfectedArea_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AvgGammaRaysDosePower_DB = table.Column<double>(type: "DOUBLE PRECISION", nullable: true),
                    MaxGammaRaysDosePower_DB = table.Column<double>(type: "DOUBLE PRECISION", nullable: true),
                    WasteDensityAlpha_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    WasteDensityBeta_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false)
                });

            migrationBuilder.DropTable("form_18");
            migrationBuilder.CreateTable(
                name: "form_18",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    IndividualNumberZHRO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    IndividualNumberZHRO_Hidden_Pr = table.Column<bool>(name: "IndividualNumberZHRO_Hidden_Pr~", type: "BOOLEAN", nullable: false),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PassportNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Volume6_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume6_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Mass7_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass7_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    SaltConcentration_DB = table.Column<double>(type: "DOUBLE PRECISION", nullable: true),
                    SaltConcentration_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SpecificActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_Hidden_ = table.Column<bool>(name: "ProviderOrRecieverOKPO_Hidden_~", type: "BOOLEAN", nullable: false),
                    TransporterOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransporterOKPO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    StoragePlaceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    StoragePlaceCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    CodeRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StatusRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume20_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass21_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TritiumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    BetaGammaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AlphaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransuraniumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RefineOrSortRAOCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Subsidy_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                });

            migrationBuilder.DropTable("form_14");
            migrationBuilder.CreateTable(
                name: "form_14",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Name_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Sort_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Activity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ActivityMeasurementDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass_DB = table.Column<double>(type: "DOUBLE PRECISION", nullable: true),
                    AggregateState_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    Owner_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransporterOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackType_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                });
        }
    }
}

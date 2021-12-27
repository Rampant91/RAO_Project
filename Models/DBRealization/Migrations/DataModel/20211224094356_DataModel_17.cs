using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region form19
            string a = "\"Id\",\"CodeTypeAccObject_DB\",\"Radionuclids_DB\",\"Activity_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_Hidden_Priv\",\"" +
           "OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO FORM_19_NEW_COLUMN (\"IdNew\",\"OperationCode_DB\") SELECT \"Id\",CAST(\"OperationCode_DB\" AS BLOB SUB_TYPE TEXT) FROM \"form_19\";");
            migrationBuilder.Sql("INSERT INTO \"form_19_tmp\" (" + a + ") SELECT " + a + " FROM \"form_19\"");
            migrationBuilder.DropTable(name: "form_19");
            migrationBuilder.CreateTable(
                name: "form_19",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    CodeTypeAccObject_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Activity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_19", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_19_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            string b = "\"Id\",\"CodeTypeAccObject_DB\",\"Radionuclids_DB\",\"Activity_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"" +
           "OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"FORM_19_TEMP\" (" + b + ") SELECT " + b + " FROM FORM_19_NEW_COLUMN INNER JOIN \"form_19_tmp\" ON \"Id\"=\"IdNew\"");
            migrationBuilder.DropTable("FORM_19_NEW_COLUMN");
            migrationBuilder.DropTable("form_19_tmp");
            #endregion
            #region form18
            a = "\"Id\",\"Sum_DB\",\"IndividualNumberZHRO_DB\",\"IndividualNumberZHRO_Hidden_Pr~\",\"PassportNumber_DB\",\"PassportNumber_Hidden_Priv\",\"Volume6_DB\",\"Volume6_Hidden_Priv\",\"Mass7_DB\",\"" +
            "Mass7_Hidden_Priv\",\"SaltConcentration_DB\",\"SaltConcentration_Hidden_Priv\",\"Radionuclids_DB\",\"SpecificActivity_DB\",\"ProviderOrRecieverOKPO_DB\",\"ProviderOrRecieverOKPO_Hidden_~\",\"TransporterOKPO_DB\",\"TransporterOKPO_Hidden_Priv\",\"" +
            "StoragePlaceName_DB\",\"StoragePlaceName_Hidden_Priv\",\"StoragePlaceCode_DB\",\"StoragePlaceCode_Hidden_Priv\",\"CodeRAO_DB\",\"StatusRAO_DB\"," +
            "\"Volume20_DB\",\"Mass21_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"RefineOrSortRAOCode_DB\",\"Subsidy_DB\",\"FcpNumber_DB\",\"ReportId\",\"FormNum_DB\",\"" +
            "NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"" +
            "DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO FORM_18_NEW_COLUMN (\"IdNew\",\"OperationCode_DB\") SELECT \"Id\",CAST(\"OperationCode_DB\" AS BLOB SUB_TYPE TEXT) FROM \"form_18\";");
            migrationBuilder.Sql("INSERT INTO \"form_18_tmp\" (" + a + ") SELECT " + a + " FROM \"form_18\"");
            migrationBuilder.DropTable(name: "form_18");
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
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_18", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_18_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            b = "\"Id\",\"Sum_DB\",\"IndividualNumberZHRO_DB\",\"IndividualNumberZHRO_Hidden_Pr~\",\"PassportNumber_DB\",\"PassportNumber_Hidden_Priv\",\"Volume6_DB\",\"Volume6_Hidden_Priv\",\"Mass7_DB\",\"" +
            "Mass7_Hidden_Priv\",\"SaltConcentration_DB\",\"SaltConcentration_Hidden_Priv\",\"Radionuclids_DB\",\"SpecificActivity_DB\",\"ProviderOrRecieverOKPO_DB\",\"ProviderOrRecieverOKPO_Hidden_~\",\"TransporterOKPO_DB\",\"TransporterOKPO_Hidden_Priv\",\"" +
            "StoragePlaceName_DB\",\"StoragePlaceName_Hidden_Priv\",\"StoragePlaceCode_DB\",\"StoragePlaceCode_Hidden_Priv\",\"CodeRAO_DB\",\"StatusRAO_DB\"," +
            "\"Volume20_DB\",\"Mass21_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"RefineOrSortRAOCode_DB\",\"Subsidy_DB\",\"FcpNumber_DB\",\"ReportId\",\"FormNum_DB\",\"" +
            "NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"" +
            "DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"FORM_18_TEMP\" (" + b + ") SELECT " + b + " FROM FORM_18_NEW_COLUMN INNER JOIN \"form_18_tmp\" ON \"Id\"=\"IdNew\"");
            migrationBuilder.DropTable("FORM_18_NEW_COLUMN");
            migrationBuilder.DropTable("form_18_tmp");
            #endregion
            #region form17
            a = "\"Id\",\"Sum_DB\",\"PackName_DB\",\"PackName_Hidden_Priv\",\"PackType_DB\",\"PackType_Hidden_Priv\",\"PackNumber_DB\",\"PackNumber_Hidden_Priv\",\"PackFactoryNumber_DB\",\"" +
            "PackFactoryNumber_Hidden_Priv\",\"FormingDate_DB\",\"FormingDate_Hidden_Priv\",\"Volume_DB\",\"Volume_Hidden_Priv\",\"Mass_DB\",\"Mass_Hidden_Priv\",\"PassportNumber_DB\",\"PassportNumber_Hidden_Priv\",\"" +
            "Radionuclids_DB\",\"SpecificActivity_DB\",\"ProviderOrRecieverOKPO_DB\",\"ProviderOrRecieverOKPO_Hidden_~\",\"TransporterOKPO_DB\",\"TransporterOKPO_Hidden_Priv\",\"StoragePlaceName_DB\",\"" +
            "StoragePlaceName_Hidden_Priv\",\"StoragePlaceCode_DB\",\"StoragePlaceCode_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"CodeRAO_DB\",\"StatusRAO_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"Quantity_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"RefineOrSortRAOCode_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"" +
            "NumberOfFields_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"" +
            "DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO FORM_17_NEW_COLUMN (\"IdNew\",\"OperationCode_DB\") SELECT \"Id\",CAST(\"OperationCode_DB\" AS BLOB SUB_TYPE TEXT) FROM \"form_17\";");
            migrationBuilder.Sql("INSERT INTO \"form_17_tmp\" (" + a + ") SELECT " + a + " FROM \"form_17\"");
            migrationBuilder.DropTable(name: "form_17");
            migrationBuilder.CreateTable(
                name: "form_17",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    PackName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    PackType_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackType_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    PackNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    PackFactoryNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackFactoryNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    FormingDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FormingDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Volume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Mass_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PassportNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
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
                    Subsidy_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CodeRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StatusRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    VolumeOutOfPack_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MassOutOfPack_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Quantity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TritiumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    BetaGammaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AlphaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransuraniumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RefineOrSortRAOCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_17", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_17_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            b = "\"Id\",\"Sum_DB\",\"PackName_DB\",\"PackName_Hidden_Priv\",\"PackType_DB\",\"PackType_Hidden_Priv\",\"PackNumber_DB\",\"PackNumber_Hidden_Priv\",\"PackFactoryNumber_DB\",\"" +
            "PackFactoryNumber_Hidden_Priv\",\"FormingDate_DB\",\"FormingDate_Hidden_Priv\",\"Volume_DB\",\"Volume_Hidden_Priv\",\"Mass_DB\",\"Mass_Hidden_Priv\",\"PassportNumber_DB\",\"PassportNumber_Hidden_Priv\",\"" +
            "Radionuclids_DB\",\"SpecificActivity_DB\",\"ProviderOrRecieverOKPO_DB\",\"ProviderOrRecieverOKPO_Hidden_~\",\"TransporterOKPO_DB\",\"TransporterOKPO_Hidden_Priv\",\"StoragePlaceName_DB\",\"" +
            "StoragePlaceName_Hidden_Priv\",\"StoragePlaceCode_DB\",\"StoragePlaceCode_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"CodeRAO_DB\",\"StatusRAO_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"Quantity_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"RefineOrSortRAOCode_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"" +
            "NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"" +
            "DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"FORM_17_TEMP\" (" + b + ") SELECT " + b + " FROM FORM_17_NEW_COLUMN INNER JOIN \"form_17_tmp\" ON \"Id\"=\"IdNew\"");
            migrationBuilder.DropTable("FORM_17_NEW_COLUMN");
            migrationBuilder.DropTable("form_17_tmp");
            #endregion
            #region form16
            a = "\"Id\",\"CodeRAO_DB\",\"StatusRAO_DB\",\"Volume_DB\",\"Mass_DB\",\"MainRadionuclids_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"ActivityMeasurementDate_DB\",\"QuantityOZIII_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"Subsidy_DB\",\"FcpNumber_DB\",\"RefineOrSortRAOCode_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"" +
            "OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"" +
            "DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO FORM_16_NEW_COLUMN (\"IdNew\",\"OperationCode_DB\") SELECT \"Id\",CAST(\"OperationCode_DB\" AS BLOB SUB_TYPE TEXT) FROM \"form_16\";");
            migrationBuilder.Sql("INSERT INTO \"form_16_tmp\" (" + a + ") SELECT " + a + " FROM \"form_16\"");
            migrationBuilder.DropTable(name: "form_16");
            migrationBuilder.CreateTable(
                name: "form_16",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    CodeRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StatusRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MainRadionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TritiumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    BetaGammaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AlphaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransuraniumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ActivityMeasurementDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    QuantityOZIII_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransporterOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackType_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Subsidy_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RefineOrSortRAOCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_16", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_16_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            b = "\"Id\",\"CodeRAO_DB\",\"StatusRAO_DB\",\"Volume_DB\",\"Mass_DB\",\"MainRadionuclids_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"ActivityMeasurementDate_DB\",\"QuantityOZIII_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"Subsidy_DB\",\"FcpNumber_DB\",\"RefineOrSortRAOCode_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"" +
            "OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"" +
            "DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"FORM_16_TEMP\" (" + b + ") SELECT " + b + " FROM FORM_16_NEW_COLUMN INNER JOIN \"form_16_tmp\" ON \"Id\"=\"IdNew\"");
            migrationBuilder.DropTable("FORM_16_NEW_COLUMN");
            migrationBuilder.DropTable("form_16_tmp");
            #endregion
            #region form15
            a = "\"Id\",\"PassportNumber_DB\",\"Type_DB\",\"Radionuclids_DB\",\"FactoryNumber_DB\",\"Quantity_DB\",\"Activity_DB\",\"CreationDate_DB\",\"StatusRAO_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"RefineOrSortRAOCode_DB\",\"" +
            "Subsidy_DB\",\"FcpNumber_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO FORM_15_NEW_COLUMN (\"IdNew\",\"OperationCode_DB\") SELECT \"Id\",CAST(\"OperationCode_DB\" AS BLOB SUB_TYPE TEXT) FROM \"form_15\";");
            migrationBuilder.Sql("INSERT INTO \"form_15_tmp\" (" + a + ") SELECT " + a + " FROM \"form_15\"");
            migrationBuilder.DropTable(name: "form_15");
            migrationBuilder.CreateTable(
                name: "form_15",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Type_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FactoryNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Quantity_DB = table.Column<int>(type: "INTEGER", nullable: true),
                    Activity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StatusRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransporterOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackType_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RefineOrSortRAOCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Subsidy_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_15", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_15_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            b = "\"Id\",\"PassportNumber_DB\",\"Type_DB\",\"Radionuclids_DB\",\"FactoryNumber_DB\",\"Quantity_DB\",\"Activity_DB\",\"CreationDate_DB\",\"StatusRAO_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"RefineOrSortRAOCode_DB\",\"" +
            "Subsidy_DB\",\"FcpNumber_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"FORM_15_TEMP\" (" + b + ") SELECT " + b + " FROM FORM_15_NEW_COLUMN INNER JOIN \"form_15_tmp\" ON \"Id\"=\"IdNew\"");
            migrationBuilder.DropTable("FORM_15_NEW_COLUMN");
            migrationBuilder.DropTable("form_15_tmp");
            #endregion
            #region form14
            a = "\"Id\",\"PassportNumber_DB\",\"Name_DB\",\"Sort_DB\",\"Radionuclids_DB\",\"Activity_DB\",\"ActivityMeasurementDate_DB\",\"Volume_DB\",\"Mass_DB\",\"AggregateState_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO FORM_14_NEW_COLUMN (\"IdNew\",\"OperationCode_DB\") SELECT \"Id\",CAST(\"OperationCode_DB\" AS BLOB SUB_TYPE TEXT) FROM \"form_14\";");
            migrationBuilder.Sql("INSERT INTO \"form_14_tmp\" (" + a + ") SELECT " + a + " FROM \"form_14\"");
            migrationBuilder.DropTable(name: "form_14");
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
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_14", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_14_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            b = "\"Id\",\"PassportNumber_DB\",\"Name_DB\",\"Sort_DB\",\"Radionuclids_DB\",\"Activity_DB\",\"ActivityMeasurementDate_DB\",\"Volume_DB\",\"Mass_DB\",\"AggregateState_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"FORM_14_TEMP\" (" + b + ") SELECT " + b + " FROM FORM_14_NEW_COLUMN INNER JOIN \"form_14_tmp\" ON \"Id\"=\"IdNew\"");
            migrationBuilder.DropTable("FORM_14_NEW_COLUMN");
            migrationBuilder.DropTable("form_14_tmp");
            #endregion
            #region form13
            a = "\"Id\",\"PassportNumber_DB\",\"Type_DB\",\"Radionuclids_DB\",\"FactoryNumber_DB\",\"Activity_DB\",\"CreationDate_DB\",\"CreatorOKPO_DB\",\"AggregateState_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO FORM_13_NEW_COLUMN (\"IdNew\",\"OperationCode_DB\") SELECT \"Id\",CAST(\"OperationCode_DB\" AS BLOB SUB_TYPE TEXT) FROM \"form_13\";");
            migrationBuilder.Sql("INSERT INTO \"form_13_tmp\" (" + a + ") SELECT " + a + " FROM \"form_13\"");
            migrationBuilder.DropTable(name: "form_13");
            migrationBuilder.CreateTable(
                name: "form_13",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Type_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FactoryNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Activity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreatorOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
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
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_13", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_13_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            b = "\"Id\",\"PassportNumber_DB\",\"Type_DB\",\"Radionuclids_DB\",\"FactoryNumber_DB\",\"Activity_DB\",\"CreationDate_DB\",\"CreatorOKPO_DB\",\"AggregateState_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"FORM_13_TEMP\" (" + b + ") SELECT " + b + " FROM FORM_13_NEW_COLUMN INNER JOIN \"form_13_tmp\" ON \"Id\"=\"IdNew\"");
            migrationBuilder.DropTable("FORM_13_NEW_COLUMN");
            migrationBuilder.DropTable("form_13_tmp");
            #endregion
            #region form12
            a = "\"Id\",\"PassportNumber_DB\",\"NameIOU_DB\",\"FactoryNumber_DB\",\"Mass_DB\",\"CreatorOKPO_DB\",\"CreationDate_DB\",\"SignedServicePeriod_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO FORM_12_NEW_COLUMN (\"IdNew\",\"OperationCode_DB\") SELECT \"Id\",CAST(\"OperationCode_DB\" AS BLOB SUB_TYPE TEXT) FROM \"form_12\";");
            migrationBuilder.Sql("INSERT INTO \"form_12_tmp\" (" + a + ") SELECT " + a + " FROM \"form_12\"");
            migrationBuilder.DropTable(name: "form_12");
            migrationBuilder.CreateTable(
                name: "form_12",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NameIOU_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FactoryNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreatorOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SignedServicePeriod_DB = table.Column<float>(type: "FLOAT", nullable: true),
                    PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    Owner_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransporterOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackType_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_12", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_12_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            b = "\"Id\",\"PassportNumber_DB\",\"NameIOU_DB\",\"FactoryNumber_DB\",\"Mass_DB\",\"CreatorOKPO_DB\",\"CreationDate_DB\",\"SignedServicePeriod_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"FORM_12_TEMP\" (" + b + ") SELECT " + b + " FROM FORM_12_NEW_COLUMN INNER JOIN \"form_12_tmp\" ON \"Id\"=\"IdNew\"");
            migrationBuilder.DropTable("FORM_12_NEW_COLUMN");
            migrationBuilder.DropTable("form_12_tmp");
            #endregion
            #region form11
            a = "\"Id\",\"PassportNumber_DB\",\"Type_DB\",\"Radionuclids_DB\",\"FactoryNumber_DB\",\"Quantity_DB\",\"Activity_DB\",\"CreationDate_DB\",\"CreatorOKPO_DB\",\"Category_DB\",\"SignedServicePeriod_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO FORM_11_NEW_COLUMN (\"IdNew\",\"OperationCode_DB\") SELECT \"Id\",CAST(\"OperationCode_DB\" AS BLOB SUB_TYPE TEXT) FROM \"form_11\";");
            migrationBuilder.Sql("INSERT INTO \"form_11_tmp\" (" + a + ") SELECT " + a + " FROM \"form_11\"");
            migrationBuilder.DropTable(name: "form_11");
            migrationBuilder.CreateTable(
                name: "form_11",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Type_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FactoryNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Quantity_DB = table.Column<int>(type: "INTEGER", nullable: true),
                    Activity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreatorOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Category_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    SignedServicePeriod_DB = table.Column<float>(type: "FLOAT", nullable: true),
                    PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    Owner_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransporterOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackType_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    OperationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_11", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_11_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            b = "\"Id\",\"PassportNumber_DB\",\"Type_DB\",\"Radionuclids_DB\",\"FactoryNumber_DB\",\"Quantity_DB\",\"Activity_DB\",\"CreationDate_DB\",\"CreatorOKPO_DB\",\"Category_DB\",\"SignedServicePeriod_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"FORM_11_TEMP\" (" + b + ") SELECT " + b + " FROM FORM_11_NEW_COLUMN INNER JOIN \"form_11_tmp\" ON \"Id\"=\"IdNew\"");
            migrationBuilder.DropTable("FORM_11_NEW_COLUMN");
            migrationBuilder.DropTable("form_11_tmp");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)//NOT READY
        {
            #region form19
            #endregion
            #region form18
            #endregion
            #region form17
            #endregion
            #region form16
            #endregion
            #region form15
            #endregion
            #region form14
            #endregion
            #region form13
            #endregion
            #region form12
            #endregion
            #region form11
            #endregion
        }
    }
}

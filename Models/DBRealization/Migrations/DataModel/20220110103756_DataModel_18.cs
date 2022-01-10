using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            #region form19
            string b = "\"Id\",\"CodeTypeAccObject_DB\",\"Radionuclids_DB\",\"Activity_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"" +
           "OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"form_19\" (" + b + ") SELECT " + b + " FROM FORM_19_TEMP");
            migrationBuilder.DropTable("FORM_19_TEMP");
            #endregion
            #region form18
            b = "\"Id\",\"Sum_DB\",\"IndividualNumberZHRO_DB\",\"IndividualNumberZHRO_Hidden_Pr~\",\"PassportNumber_DB\",\"PassportNumber_Hidden_Priv\",\"Volume6_DB\",\"Volume6_Hidden_Priv\",\"Mass7_DB\",\"" +
            "Mass7_Hidden_Priv\",\"SaltConcentration_DB\",\"SaltConcentration_Hidden_Priv\",\"Radionuclids_DB\",\"SpecificActivity_DB\",\"ProviderOrRecieverOKPO_DB\",\"ProviderOrRecieverOKPO_Hidden_~\",\"TransporterOKPO_DB\",\"TransporterOKPO_Hidden_Priv\",\"" +
            "StoragePlaceName_DB\",\"StoragePlaceName_Hidden_Priv\",\"StoragePlaceCode_DB\",\"StoragePlaceCode_Hidden_Priv\",\"CodeRAO_DB\",\"StatusRAO_DB\"," +
            "\"Volume20_DB\",\"Mass21_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"RefineOrSortRAOCode_DB\",\"Subsidy_DB\",\"FcpNumber_DB\",\"ReportId\",\"FormNum_DB\",\"" +
            "NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"" +
            "DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"form_18\" (" + b + ") SELECT " + b + " FROM FORM_18_TEMP");
            migrationBuilder.DropTable("FORM_18_TEMP");
            #endregion
            #region form17
            b = "\"Id\",\"Sum_DB\",\"PackName_DB\",\"PackName_Hidden_Priv\",\"PackType_DB\",\"PackType_Hidden_Priv\",\"PackNumber_DB\",\"PackNumber_Hidden_Priv\",\"PackFactoryNumber_DB\",\"" +
            "PackFactoryNumber_Hidden_Priv\",\"FormingDate_DB\",\"FormingDate_Hidden_Priv\",\"Volume_DB\",\"Volume_Hidden_Priv\",\"Mass_DB\",\"Mass_Hidden_Priv\",\"PassportNumber_DB\",\"PassportNumber_Hidden_Priv\",\"" +
            "Radionuclids_DB\",\"SpecificActivity_DB\",\"ProviderOrRecieverOKPO_DB\",\"ProviderOrRecieverOKPO_Hidden_~\",\"TransporterOKPO_DB\",\"TransporterOKPO_Hidden_Priv\",\"StoragePlaceName_DB\",\"" +
            "StoragePlaceName_Hidden_Priv\",\"StoragePlaceCode_DB\",\"StoragePlaceCode_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"CodeRAO_DB\",\"StatusRAO_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"Quantity_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"RefineOrSortRAOCode_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"" +
            "NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"" +
            "DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"form_17\" (" + b + ") SELECT " + b + " FROM FORM_17_TEMP");
            migrationBuilder.DropTable("FORM_17_TEMP");
            #endregion
            #region form16
            b = "\"Id\",\"CodeRAO_DB\",\"StatusRAO_DB\",\"Volume_DB\",\"Mass_DB\",\"MainRadionuclids_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"ActivityMeasurementDate_DB\",\"QuantityOZIII_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"Subsidy_DB\",\"FcpNumber_DB\",\"RefineOrSortRAOCode_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"" +
            "OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"" +
            "DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"form_16\" (" + b + ") SELECT " + b + " FROM FORM_16_TEMP");
            migrationBuilder.DropTable("FORM_16_TEMP");
            #endregion
            #region form15
            b = "\"Id\",\"PassportNumber_DB\",\"Type_DB\",\"Radionuclids_DB\",\"FactoryNumber_DB\",\"Quantity_DB\",\"Activity_DB\",\"CreationDate_DB\",\"StatusRAO_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"RefineOrSortRAOCode_DB\",\"" +
            "Subsidy_DB\",\"FcpNumber_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"form_15\" (" + b + ") SELECT " + b + " FROM FORM_15_TEMP");
            migrationBuilder.DropTable("FORM_15_TEMP");
            #endregion
            #region form14
            b = "\"Id\",\"PassportNumber_DB\",\"Name_DB\",\"Sort_DB\",\"Radionuclids_DB\",\"Activity_DB\",\"ActivityMeasurementDate_DB\",\"Volume_DB\",\"Mass_DB\",\"AggregateState_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"form_14\" (" + b + ") SELECT " + b + " FROM FORM_14_TEMP");
            migrationBuilder.DropTable("FORM_14_TEMP");
            #endregion
            #region form13
            b = "\"Id\",\"PassportNumber_DB\",\"Type_DB\",\"Radionuclids_DB\",\"FactoryNumber_DB\",\"Activity_DB\",\"CreationDate_DB\",\"CreatorOKPO_DB\",\"AggregateState_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"form_13\" (" + b + ") SELECT " + b + " FROM FORM_13_TEMP");
            migrationBuilder.DropTable("FORM_13_TEMP");
            #endregion
            #region form12
            b = "\"Id\",\"PassportNumber_DB\",\"NameIOU_DB\",\"FactoryNumber_DB\",\"Mass_DB\",\"CreatorOKPO_DB\",\"CreationDate_DB\",\"SignedServicePeriod_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"form_12\" (" + b + ") SELECT " + b + " FROM FORM_12_TEMP");
            migrationBuilder.DropTable("FORM_12_TEMP");
            #endregion
            #region form11
            b = "\"Id\",\"PassportNumber_DB\",\"Type_DB\",\"Radionuclids_DB\",\"FactoryNumber_DB\",\"Quantity_DB\",\"Activity_DB\",\"CreationDate_DB\",\"CreatorOKPO_DB\",\"Category_DB\",\"SignedServicePeriod_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"form_11\" (" + b + ") SELECT " + b + " FROM FORM_11_TEMP");
            migrationBuilder.DropTable("FORM_11_TEMP");
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

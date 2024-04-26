using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel;

// Заполняем вновь созданные таблицы форм данными из временных таблиц, которые потом удаляем
public partial class DataModel_30 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        #region form10
        
        const string allColumns10 =
            "\"Id\", \"RegNo_DB\", \"OrganUprav_DB\", \"SubjectRF_DB\", \"JurLico_DB\", \"ShortJurLico_DB\", " +
            "\"JurLicoAddress_DB\", \"JurLicoFactAddress_DB\", \"GradeFIO_DB\", \"Telephone_DB\", \"Fax_DB\", " +
            "\"Email_DB\", \"Okpo_DB\", \"Okved_DB\", \"Okogu_DB\", \"Oktmo_DB\", \"Inn_DB\", \"Kpp_DB\", " +
            "\"Okopf_DB\", \"Okfs_DB\", \"ReportId\", \"FormNum_DB\", \"NumberInOrder_DB\", \"NumberOfFields_DB\"";

        migrationBuilder.Sql($"INSERT INTO \"form_10\" ({allColumns10}) " +
                             $"SELECT {allColumns10} " +
                             $"FROM \"form_10_editableColumns\" " +
                             $"INNER JOIN \"form_10_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_10_editableColumns");
        migrationBuilder.DropTable(name: "form_10_withoutEditableColumns"); 
        
        #endregion

        #region form11
        
        const string allColumns11 =
            "\"Id\", \"PassportNumber_DB\", \"Type_DB\", \"Radionuclids_DB\", \"FactoryNumber_DB\", \"Quantity_DB\", " +
            "\"Activity_DB\", \"CreationDate_DB\", \"CreatorOKPO_DB\", \"Category_DB\", \"SignedServicePeriod_DB\", " +
            "\"PropertyCode_DB\", \"Owner_DB\", \"ProviderOrRecieverOKPO_DB\", \"TransporterOKPO_DB\", \"PackName_DB\", " +
            "\"PackType_DB\", \"PackNumber_DB\", \"ReportId\", \"FormNum_DB\", \"NumberInOrder_DB\", " +
            "\"NumberOfFields_DB\", \"OperationCode_DB\", \"OperationCode_Hidden_Priv\", \"OperationDate_DB\", " +
            "\"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", \"DocumentNumber_DB\", " +
            "\"DocumentNumber_Hidden_Priv\", \"DocumentDate_DB\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_11\" ({allColumns11}) " +
                             $"SELECT {allColumns11} " +
                             $"FROM \"form_11_editableColumns\" " +
                             $"INNER JOIN \"form_11_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_11_editableColumns");
        migrationBuilder.DropTable(name: "form_11_withoutEditableColumns"); 
        
        #endregion

        #region form12

        const string allColumns12 =
            "\"Id\", \"PassportNumber_DB\", \"NameIOU_DB\", \"FactoryNumber_DB\", \"Mass_DB\", \"CreatorOKPO_DB\", " +
            "\"CreationDate_DB\", \"SignedServicePeriod_DB\", \"PropertyCode_DB\", \"Owner_DB\", \"ProviderOrRecieverOKPO_DB\", " +
            "\"TransporterOKPO_DB\", \"PackName_DB\", \"PackType_DB\", \"PackNumber_DB\", \"ReportId\", \"FormNum_DB\", " + 
            "\"NumberInOrder_DB\", \"NumberOfFields_DB\", \"OperationCode_DB\", \"OperationCode_Hidden_Priv\", " + 
            "\"OperationDate_DB\", \"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", " + 
            "\"DocumentNumber_DB\", \"DocumentNumber_Hidden_Priv\", \"DocumentDate_DB\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_12\" ({allColumns12}) " +
                             $"SELECT {allColumns12} " +
                             $"FROM \"form_12_editableColumns\" " +
                             $"INNER JOIN \"form_12_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_12_editableColumns");
        migrationBuilder.DropTable(name: "form_12_withoutEditableColumns");

        #endregion

        #region form13

        const string allColumns13 =
            "\"Id\", \"PassportNumber_DB\", \"Type_DB\", \"Radionuclids_DB\", \"FactoryNumber_DB\", \"Activity_DB\", " +
            "\"CreationDate_DB\", \"CreatorOKPO_DB\",\"AggregateState_DB\", \"PropertyCode_DB\", \"Owner_DB\", " +
            "\"ProviderOrRecieverOKPO_DB\", \"TransporterOKPO_DB\", \"PackName_DB\", \"PackType_DB\", " +
            "\"PackNumber_DB\", \"ReportId\", \"FormNum_DB\", \"NumberInOrder_DB\", \"NumberOfFields_DB\", " +
            "\"OperationCode_DB\", \"OperationCode_Hidden_Priv\", \"OperationDate_DB\", \"OperationDate_Hidden_Priv\", " +
            "\"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", \"DocumentNumber_DB\", \"DocumentNumber_Hidden_Priv\", " +
            "\"DocumentDate_DB\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_13\" ({allColumns13}) " +
                             $"SELECT {allColumns13} " +
                             $"FROM \"form_13_editableColumns\" " +
                             $"INNER JOIN \"form_13_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_13_editableColumns");
        migrationBuilder.DropTable(name: "form_13_withoutEditableColumns");

        #endregion

        #region form14

        const string allColumns14 =
            "\"Id\", \"PassportNumber_DB\", \"Name_DB\", \"Sort_DB\", \"Radionuclids_DB\", \"Activity_DB\", " +
            "\"ActivityMeasurementDate_DB\", \"Volume_DB\",\"Mass_DB\", \"AggregateState_DB\", \"PropertyCode_DB\", " +
            "\"Owner_DB\", \"ProviderOrRecieverOKPO_DB\", \"TransporterOKPO_DB\", \"PackName_DB\", " +
            "\"PackType_DB\", \"PackNumber_DB\", \"ReportId\", \"FormNum_DB\", \"NumberInOrder_DB\", " +
            "\"NumberOfFields_DB\", \"OperationCode_DB\", \"OperationCode_Hidden_Priv\", \"OperationDate_DB\", " +
            "\"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", \"DocumentNumber_DB\", " +
            "\"DocumentNumber_Hidden_Priv\", \"DocumentDate_DB\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_14\" ({allColumns14}) " +
                             $"SELECT {allColumns14} " +
                             $"FROM \"form_14_editableColumns\" " +
                             $"INNER JOIN \"form_14_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_14_editableColumns");
        migrationBuilder.DropTable(name: "form_14_withoutEditableColumns");

        #endregion

        #region form15

        const string allColumns15 =
            "\"Id\", \"PassportNumber_DB\", \"Type_DB\", \"Radionuclids_DB\", \"FactoryNumber_DB\", \"Quantity_DB\", " +
            "\"Activity_DB\",\"CreationDate_DB\", \"StatusRAO_DB\", \"ProviderOrRecieverOKPO_DB\", " + 
            "\"TransporterOKPO_DB\", \"PackName_DB\", \"PackType_DB\", \"PackNumber_DB\", \"StoragePlaceName_DB\", " + 
            "\"StoragePlaceCode_DB\", \"RefineOrSortRAOCode_DB\", \"Subsidy_DB\", \"FcpNumber_DB\", \"ReportId\", " + 
            "\"FormNum_DB\", \"NumberInOrder_DB\", \"NumberOfFields_DB\", \"OperationCode_DB\", " + 
            "\"OperationCode_Hidden_Priv\", \"OperationDate_DB\", \"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", " + 
            "\"DocumentVid_Hidden_Priv\", \"DocumentNumber_DB\", \"DocumentNumber_Hidden_Priv\", \"DocumentDate_DB\", " + 
            "\"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_15\" ({allColumns15}) " +
                             $"SELECT {allColumns15} " +
                             $"FROM \"form_15_editableColumns\" " +
                             $"INNER JOIN \"form_15_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_15_editableColumns");
        migrationBuilder.DropTable(name: "form_15_withoutEditableColumns");

        #endregion

        #region form16

        const string allColumns16 =
            "\"Id\", \"CodeRAO_DB\", \"StatusRAO_DB\", \"Volume_DB\", \"Mass_DB\", \"MainRadionuclids_DB\", " + 
            "\"TritiumActivity_DB\", \"BetaGammaActivity_DB\", \"AlphaActivity_DB\", \"TransuraniumActivity_DB\", " + 
            "\"ActivityMeasurementDate_DB\", \"QuantityOZIII_DB\", \"ProviderOrRecieverOKPO_DB\", \"TransporterOKPO_DB\", " + 
            "\"PackName_DB\", \"PackType_DB\", \"PackNumber_DB\", \"StoragePlaceName_DB\", \"StoragePlaceCode_DB\", " + 
            "\"Subsidy_DB\", \"FcpNumber_DB\", \"RefineOrSortRAOCode_DB\", \"ReportId\", \"FormNum_DB\", " + 
            "\"NumberInOrder_DB\", \"NumberOfFields_DB\", \"OperationCode_DB\", \"OperationCode_Hidden_Priv\", " + 
            "\"OperationDate_DB\", \"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", " + 
            "\"DocumentNumber_DB\", \"DocumentNumber_Hidden_Priv\", \"DocumentDate_DB\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_16\" ({allColumns16}) " +
                             $"SELECT {allColumns16} " +
                             $"FROM \"form_16_editableColumns\" " +
                             $"INNER JOIN \"form_16_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_16_editableColumns");
        migrationBuilder.DropTable(name: "form_16_withoutEditableColumns");

        #endregion

        #region form17

        const string allColumns17 =
            "\"Id\", \"Sum_DB\", \"PackName_DB\", \"PackName_Hidden_Priv\", \"PackType_DB\", \"PackType_Hidden_Priv\", " + 
            "\"PackNumber_DB\", \"PackNumber_Hidden_Priv\", \"PackFactoryNumber_DB\", \"PackFactoryNumber_Hidden_Priv\", " + 
            "\"FormingDate_DB\", \"FormingDate_Hidden_Priv\", \"Volume_DB\", \"Volume_Hidden_Priv\", \"Mass_DB\", " + 
            "\"Mass_Hidden_Priv\", \"PassportNumber_DB\", \"PassportNumber_Hidden_Priv\", \"Radionuclids_DB\", " + 
            "\"SpecificActivity_DB\", \"ProviderOrRecieverOKPO_DB\", \"ProviderOrRecieverOKPO_Hidden_~\", " + 
            "\"TransporterOKPO_DB\", \"TransporterOKPO_Hidden_Priv\", \"StoragePlaceName_DB\", " + 
            "\"StoragePlaceName_Hidden_Priv\", \"StoragePlaceCode_DB\", \"StoragePlaceCode_Hidden_Priv\", \"Subsidy_DB\", " + 
            "\"FcpNumber_DB\", \"CodeRAO_DB\", \"StatusRAO_DB\", \"VolumeOutOfPack_DB\", \"MassOutOfPack_DB\", " + 
            "\"Quantity_DB\", \"TritiumActivity_DB\", \"BetaGammaActivity_DB\", \"AlphaActivity_DB\", " + 
            "\"TransuraniumActivity_DB\", \"RefineOrSortRAOCode_DB\", \"ReportId\", \"FormNum_DB\", " + 
            "\"NumberInOrder_DB\", \"NumberOfFields_DB\", \"OperationCode_DB\", \"OperationCode_Hidden_Priv\", " +
            "\"OperationDate_DB\", \"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", " +
            "\"DocumentNumber_DB\", \"DocumentNumber_Hidden_Priv\", \"DocumentDate_DB\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_17\" ({allColumns17}) " +
                             $"SELECT {allColumns17} " +
                             $"FROM \"form_17_editableColumns\" " +
                             $"INNER JOIN \"form_17_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_17_editableColumns");
        migrationBuilder.DropTable(name: "form_17_withoutEditableColumns");

        #endregion

        #region form18

        const string allColumns18 =
            "\"Id\", \"Sum_DB\", \"IndividualNumberZHRO_DB\", \"IndividualNumberZHRO_Hidden_Pr~\", \"PassportNumber_DB\", " +
            "\"PassportNumber_Hidden_Priv\", \"Volume6_DB\", \"Volume6_Hidden_Priv\", \"Mass7_DB\", " +
            "\"Mass7_Hidden_Priv\", \"SaltConcentration_DB\", \"SaltConcentration_Hidden_Priv\", \"Radionuclids_DB\", " +
            "\"SpecificActivity_DB\", \"ProviderOrRecieverOKPO_DB\", \"ProviderOrRecieverOKPO_Hidden_~\", " +
            "\"TransporterOKPO_DB\", \"TransporterOKPO_Hidden_Priv\", \"StoragePlaceName_DB\", " +
            "\"StoragePlaceName_Hidden_Priv\", \"StoragePlaceCode_DB\", \"StoragePlaceCode_Hidden_Priv\", \"CodeRAO_DB\", " +
            "\"StatusRAO_DB\", \"Volume20_DB\", \"Mass21_DB\", \"TritiumActivity_DB\", \"BetaGammaActivity_DB\", " +
            "\"AlphaActivity_DB\", \"TransuraniumActivity_DB\", \"RefineOrSortRAOCode_DB\", \"Subsidy_DB\", " +
            "\"FcpNumber_DB\", \"ReportId\", \"FormNum_DB\", \"NumberInOrder_DB\", \"NumberOfFields_DB\", " +
            "\"OperationCode_Hidden_Priv\", \"OperationDate_DB\", \"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", " +
            "\"DocumentVid_Hidden_Priv\", \"DocumentNumber_DB\", \"DocumentNumber_Hidden_Priv\", \"DocumentDate_DB\", " +
            "\"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_18\" ({allColumns18}) " +
                             $"SELECT {allColumns18} " +
                             $"FROM \"form_18_editableColumns\" " +
                             $"INNER JOIN \"form_18_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_18_editableColumns");
        migrationBuilder.DropTable(name: "form_18_withoutEditableColumns");

        #endregion

        #region form19

        const string allColumns19 =
            "\"Id\", \"CodeTypeAccObject_DB\", \"Radionuclids_DB\", \"Activity_DB\", \"ReportId\", \"FormNum_DB\", " +
            "\"NumberInOrder_DB\", \"NumberOfFields_DB\", \"OperationCode_DB\", \"OperationCode_Hidden_Priv\", " +
            "\"OperationDate_DB\", \"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", " +
            "\"DocumentNumber_DB\", \"DocumentNumber_Hidden_Priv\", \"DocumentDate_DB\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_19\" ({allColumns19}) " +
                             $"SELECT {allColumns19} " +
                             $"FROM \"form_19_editableColumns\" " +
                             $"INNER JOIN \"form_19_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_19_editableColumns");
        migrationBuilder.DropTable(name: "form_19_withoutEditableColumns");

        #endregion

        #region form20
        
        const string allColumns20 =
            "\"Id\", \"RegNo_DB\", \"OrganUprav_DB\", \"SubjectRF_DB\", \"JurLico_DB\", \"ShortJurLico_DB\", " +
            "\"JurLicoAddress_DB\", \"JurLicoFactAddress_DB\", \"GradeFIO_DB\", \"Telephone_DB\", \"Fax_DB\", " +
            "\"Email_DB\", \"Okpo_DB\", \"Okved_DB\", \"Okogu_DB\", \"Oktmo_DB\", \"Inn_DB\", \"Kpp_DB\", " +
            "\"Okopf_DB\", \"Okfs_DB\", \"ReportId\", \"FormNum_DB\", \"NumberInOrder_DB\", \"NumberOfFields_DB\"";

        migrationBuilder.Sql($"INSERT INTO \"form_20\" ({allColumns20}) " +
                             $"SELECT {allColumns20} " +
                             $"FROM \"form_20_editableColumns\" " +
                             $"INNER JOIN \"form_20_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_20_editableColumns");
        migrationBuilder.DropTable(name: "form_20_withoutEditableColumns"); 
        
        #endregion

        #region form21
        
        const string allColumns21 =
            "\"Id\", \"Sum_DB\", \"RefineMachineName_DB\", \"_RefineMachineName_Hidden_Get\", \"MachineCode_DB\", " +
            "\"_MachineCode_Hidden_Get\", \"MachinePower_DB\", \"_MachinePower_Hidden_Get\", " +
            "\"NumberOfHoursPerYear_DB\", \"_NumberOfHoursPerYear_Hidden_G~\", \"CodeRAOIn_DB\", " +
            "\"CodeRAOIn_Hidden_Priv\", \"StatusRAOIn_DB\", \"VolumeIn_DB\", \"MassIn_DB\", \"QuantityIn_DB\", " +
            "\"TritiumActivityIn_DB\", \"BetaGammaActivityIn_DB\", \"AlphaActivityIn_DB\", " +
            "\"TransuraniumActivityIn_DB\", \"CodeRAOout_DB\", \"StatusRAOout_DB\", \"VolumeOut_DB\", " +
            "\"MassOut_DB\", \"QuantityOZIIIout_DB\", \"TritiumActivityOut_DB\", \"BetaGammaActivityOut_DB\", " +
            "\"AlphaActivityOut_DB\", \"TransuraniumActivityOut_DB\", \"ReportId\", \"FormNum_DB\", " +
            "\"NumberInOrder_DB\", \"NumberOfFields_DB\", \"CorrectionNumber_DB\", \"CodeRAOout_Hidden_Priv\", " +
            "\"StatusRAOIn_Hidden_Priv\", \"StatusRAOout_Hidden_Priv\", \"_MachineCode_Hidden_Set\", " +
            "\"_MachinePower_Hidden_Set\", \"_NumberOfHoursPerYear_Hidden_S~\", \"_RefineMachineName_Hidden_Set\", " +
            "\"SumGroup_DB\", \"_BaseColor\", \"NumberInOrderSum_DB\"";

        migrationBuilder.Sql($"INSERT INTO \"form_21\" ({allColumns21}) " +
                             $"SELECT {allColumns21} " +
                             $"FROM \"form_21_editableColumns\" " +
                             $"INNER JOIN \"form_21_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_21_editableColumns");
        migrationBuilder.DropTable(name: "form_21_withoutEditableColumns"); 
        
        #endregion

        #region form22
        
        const string allColumns22 =
            "\"Id\", \"_PackName_Hidden_Get\", \"_PackName_Hidden_Set\", \"_PackType_Hidden_Get\", " +
            "\"_PackType_Hidden_Set\", \"_StoragePlaceCode_Hidden_Get\", \"_StoragePlaceCode_Hidden_Set\", " +
            "\"_StoragePlaceName_Hidden_Get\", \"_StoragePlaceName_Hidden_Set\", \"MassInPack_Hidden_Priv\", " +
            "\"MassInPack_Hidden_Priv2\", \"VolumeInPack_Hidden_Priv\", \"VolumeInPack_Hidden_Priv2\", " +
            "\"FcpNumber_Hidden_Priv\", \"Subsidy_Hidden_Priv\", \"Sum_DB\", \"StoragePlaceName_DB\", " +
            "\"StoragePlaceCode_DB\", \"PackName_DB\", \"PackType_DB\", \"PackQuantity_DB\", \"CodeRAO_DB\", " +
            "\"CodeRAO_Hidden_Priv\", \"StatusRAO_DB\", \"StatusRAO_Hidden_Priv\", \"VolumeInPack_DB\", " +
            "\"MassInPack_DB\", \"VolumeOutOfPack_DB\", \"MassOutOfPack_DB\", \"QuantityOZIII_DB\", " +
            "\"TritiumActivity_DB\", \"BetaGammaActivity_DB\", \"AlphaActivity_DB\", \"TransuraniumActivity_DB\", " +
            "\"MainRadionuclids_DB\", \"MainRadionuclids_Hidden_Priv\", \"Subsidy_DB\", \"FcpNumber_DB\", " +
            "\"ReportId\", \"FormNum_DB\", \"NumberInOrder_DB\", \"NumberOfFields_DB\", \"CorrectionNumber_DB\", " +
            "\"SumGroup_DB\", \"_BaseColor\", \"NumberInOrderSum_DB\"";

        migrationBuilder.Sql($"INSERT INTO \"form_22\" ({allColumns22}) " +
                             $"SELECT {allColumns22} " +
                             $"FROM \"form_22_editableColumns\" " +
                             $"INNER JOIN \"form_22_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_22_editableColumns");
        migrationBuilder.DropTable(name: "form_22_withoutEditableColumns"); 
        
        #endregion
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}
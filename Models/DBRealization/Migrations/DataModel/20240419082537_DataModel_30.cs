using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel;

// Заполняем вновь созданные таблицы форм данными из временных таблиц, которые потом удаляем
public partial class DataModel_30 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
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
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}
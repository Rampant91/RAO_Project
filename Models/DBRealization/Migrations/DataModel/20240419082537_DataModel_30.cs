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
            "\"Id\", \"PassportNumber_DB\", \"Type_DB\", \"Radionuclids_DB\", \"FactoryNumber_DB\", \"Activity_DB\", " + 
            "\"CreationDate_DB\", \"CreatorOKPO_DB\",\"AggregateState_DB\", \"PropertyCode_DB\", \"Owner_DB\", " + 
            "\"ProviderOrRecieverOKPO_DB\", \"TransporterOKPO_DB\", \"PackName_DB\", \"PackType_DB\", " + 
            "\"PackNumber_DB\", \"ReportId\", \"FormNum_DB\", \"NumberInOrder_DB\", \"NumberOfFields_DB\", " + 
            "\"OperationCode_DB\", \"OperationCode_Hidden_Priv\", \"OperationDate_DB\", \"OperationDate_Hidden_Priv\", " + 
            "\"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", \"DocumentNumber_DB\", \"DocumentNumber_Hidden_Priv\", " + 
            "\"DocumentDate_DB\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_14\" ({allColumns14}) " +
                             $"SELECT {allColumns14} " +
                             $"FROM \"form_14_editableColumns\" " +
                             $"INNER JOIN \"form_14_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

        migrationBuilder.DropTable(name: "form_14_editableColumns");
        migrationBuilder.DropTable(name: "form_14_withoutEditableColumns"); 
        
        #endregion
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}
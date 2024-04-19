using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_30 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string allColumns = 
                "\"Id\", \"PassportNumber_DB\", \"Type_DB\", \"Radionuclids_DB\", \"FactoryNumber_DB\", \"Quantity_DB\", " + 
                "\"Activity_DB\", \"CreationDate_DB\", \"CreatorOKPO_DB\", \"Category_DB\", \"SignedServicePeriod_DB\", " + 
                "\"PropertyCode_DB\", \"Owner_DB\", \"ProviderOrRecieverOKPO_DB\", \"TransporterOKPO_DB\", \"PackName_DB\", " + 
                "\"PackType_DB\", \"PackNumber_DB\", \"ReportId\", \"FormNum_DB\", \"NumberInOrder_DB\", " + 
                "\"NumberOfFields_DB\", \"OperationCode_DB\", \"OperationCode_Hidden_Priv\", \"OperationDate_DB\", " + 
                "\"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", \"DocumentNumber_DB\", " + 
                "\"DocumentNumber_Hidden_Priv\", \"DocumentDate_DB\", \"DocumentDate_Hidden_Priv\"";

            migrationBuilder.Sql($"INSERT INTO \"form_11\" ({allColumns}) " +
                                 $"SELECT {allColumns} " +
                                 $"FROM \"form_11_editableColumns\" " +
                                 $"INNER JOIN \"form_11_withoutEditableColumns\" ON \"Id\"=\"IdNew\"");

            migrationBuilder.DropTable(name: "form_11_editableColumns");
            migrationBuilder.DropTable(name: "form_11_withoutEditableColumns");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

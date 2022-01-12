using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string attrWCol = "\"PassportNumber_DB\",\"NameIOU_DB\",\"FactoryNumber_DB\",\"Mass_DB\",\"CreatorOKPO_DB\",\"CreationDate_DB\",\"SignedServicePeriod_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"form_12\" (" + attrWCol + ") SELECT " + attrWCol + " FROM FORM_12_TEMP");
            migrationBuilder.DropTable("FORM_12_TEMP");
        }

        protected override void Down(MigrationBuilder migrationBuilder)//NOT READY YET
        {

        }
    }
}

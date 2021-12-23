using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string b = "\"Id\",\"Sum_DB\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"PackName_DB\",\"PackType_DB\",\"CodeRAO_DB\",\"CodeRAO_Hidden_Priv\",\"StatusRAO_DB\",\"" +
               "StatusRAO_Hidden_Priv\",\"VolumeInPack_DB\",\"MassInPack_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"QuantityOZIII_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\"," +
               "\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"MainRadionuclids_DB\",\"MainRadionuclids_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"ReportId\",\"FormNum_DB\"," +
               "\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"CorrectionNumber_DB\"";
            migrationBuilder.Sql("INSERT INTO \"form_22\" (" + b + ") SELECT " + b + " FROM FORM_22_TEMP");
            migrationBuilder.DropTable("FORM_22_TEMP");
        }

        protected override void Down(MigrationBuilder migrationBuilder)//NOT READY YET
        {

        }
    }
}

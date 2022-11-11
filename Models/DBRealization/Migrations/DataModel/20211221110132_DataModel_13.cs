using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_13 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "form_22_tmp",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PackName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackName_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackType_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackType_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceCode_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceName_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                MassInPack_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                MassInPack_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                VolumeInPack_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                VolumeInPack_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                FcpNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Subsidy_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                StoragePlaceCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                PackName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                PackType_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                CodeRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                CodeRAO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StatusRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                StatusRAO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                VolumeInPack_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                MassInPack_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                VolumeOutOfPack_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                MassOutOfPack_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                QuantityOZIII_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                TritiumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                BetaGammaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                AlphaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                TransuraniumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                MainRadionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                MainRadionuclids_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Subsidy_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
            });
        migrationBuilder.CreateTable(
            name: "FORM_22_NEW_COLUMN",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PackQuantity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true)
            });
        migrationBuilder.CreateTable(
            name: "FORM_22_TEMP",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PackName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackName_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackType_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackType_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceCode_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceName_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                MassInPack_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                MassInPack_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                VolumeInPack_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                VolumeInPack_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                FcpNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Subsidy_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                StoragePlaceCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                PackName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                PackType_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                PackQuantity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                CodeRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                CodeRAO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StatusRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                StatusRAO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                VolumeInPack_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                MassInPack_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                VolumeOutOfPack_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                MassOutOfPack_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                QuantityOZIII_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                TritiumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                BetaGammaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                AlphaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                TransuraniumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                MainRadionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                MainRadionuclids_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Subsidy_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        string b = "\"Id\",\"PackName_Hidden_Priv\",\"PackName_Hidden_Priv2\",\"PackType_Hidden_Priv\",\"PackType_Hidden_Priv2\",\"StoragePlaceCode_Hidden_Priv\",\"StoragePlaceCode_Hidden_Priv2\",\"StoragePlaceName_Hidden_Priv\",\"StoragePlaceName_Hidden_Priv2\",\"MassInPack_Hidden_Priv\",\"MassInPack_Hidden_Priv2\",\"VolumeInPack_Hidden_Priv\",\"VolumeInPack_Hidden_Priv2\",\"FcpNumber_Hidden_Priv\",\"Subsidy_Hidden_Priv\",\"Sum_DB\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"PackName_DB\",\"PackType_DB\",\"CodeRAO_DB\",\"CodeRAO_Hidden_Priv\",\"StatusRAO_DB\",\"" +
                   "StatusRAO_Hidden_Priv\",\"VolumeInPack_DB\",\"MassInPack_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"QuantityOZIII_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\"," +
                   "\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"MainRadionuclids_DB\",\"MainRadionuclids_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"ReportId\",\"FormNum_DB\"," +
                   "\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"CorrectionNumber_DB\"";
        migrationBuilder.Sql($"INSERT INTO \"form_22\" ({b}) SELECT {b} FROM FORM_22_TEMP");
        migrationBuilder.DropTable("FORM_22_TEMP");
    }
}
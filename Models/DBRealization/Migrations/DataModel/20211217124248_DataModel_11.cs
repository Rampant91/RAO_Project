using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_11 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        string a = "\"Id\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"ProjectVolume_DB\",\"CodeRAO_DB\",\"Volume_DB\",\"Mass_DB\",\"SummaryActivity_DB\",\"DocumentNumber_DB\",\"" +
                   "DocumentDate_DB\",\"ExpirationDate_DB\",\"DocumentName_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"CorrectionNumber_DB\"";
        migrationBuilder.Sql("INSERT INTO FORM_23_NEW_COLUMN (\"IdNew\",\"QuantityOZIII_DB\") SELECT \"Id\",CAST(\"QuantityOZIII_DB\" AS BLOB SUB_TYPE TEXT) FROM \"form_23\";");
        migrationBuilder.Sql($"INSERT INTO \"form_23_tmp\" ({a}) SELECT {a} FROM \"form_23\"");
        migrationBuilder.DropTable(name: "form_23");
        migrationBuilder.CreateTable(
            name: "form_23",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                StoragePlaceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                StoragePlaceCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                ProjectVolume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                CodeRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                Volume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                Mass_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                QuantityOZIII_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                SummaryActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                DocumentNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                DocumentDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                ExpirationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                DocumentName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_form_23", x => x.Id);
                table.ForeignKey(
                    name: "FK_form_23_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });
        string b = "\"Id\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"ProjectVolume_DB\",\"CodeRAO_DB\",\"Volume_DB\",\"Mass_DB\",\"QuantityOZIII_DB\",\"SummaryActivity_DB\",\"DocumentNumber_DB\",\"" +
                   "DocumentDate_DB\",\"ExpirationDate_DB\",\"DocumentName_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"CorrectionNumber_DB\"";
        migrationBuilder.Sql(
            $"INSERT INTO \"FORM_23_TEMP\" ({b}) SELECT {b} FROM FORM_23_NEW_COLUMN INNER JOIN \"form_23_tmp\" ON \"Id\"=\"IdNew\"");
        migrationBuilder.DropTable("FORM_23_NEW_COLUMN");
        migrationBuilder.DropTable("form_23_tmp");
    }

    protected override void Down(MigrationBuilder migrationBuilder)//NOT READY YET
    {
        string a = "\"Id\",\"Sum_DB\",\"PackName_DB\",\"PackName_Hidden_Priv\",\"PackType_DB\",\"PackType_Hidden_Priv\",\"PackNumber_DB\",\"PackNumber_Hidden_Priv\",\"PackFactoryNumber_DB\",\"" +
                   "PackFactoryNumber_Hidden_Priv\",\"FormingDate_DB\",\"FormingDate_Hidden_Priv\",\"Volume_DB\",\"Volume_Hidden_Priv\",\"Mass_DB\",\"Mass_Hidden_Priv\",\"PassportNumber_DB\",\"PassportNumber_Hidden_Priv\",\"" +
                   "Radionuclids_DB\",\"SpecificActivity_DB\",\"ProviderOrRecieverOKPO_DB\",\"ProviderOrRecieverOKPO_Hidden_~\",\"TransporterOKPO_DB\",\"TransporterOKPO_Hidden_Priv\",\"StoragePlaceName_DB\",\"" +
                   "StoragePlaceName_Hidden_Priv\",\"StoragePlaceCode_DB\",\"StoragePlaceCode_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"CodeRAO_DB\",\"StatusRAO_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"RefineOrSortRAOCode_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"" +
                   "NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"" +
                   "DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
        migrationBuilder.Sql("INSERT INTO FORM_17_NEW_COLUMN (\"IdNew\",\"Quantity_DB\") SELECT \"Id\" FROM \"form_17\"");
        migrationBuilder.Sql(
            $"INSERT INTO \"form_17_tmp\" ({a}) SELECT \"Id\",\"Sum_DB\",\"PackName_DB\",\"PackName_Hidden_Priv\",\"PackType_DB\",\"PackType_Hidden_Priv\",\"PackNumber_DB\",\"PackNumber_Hidden_Priv\",\"PackFactoryNumber_DB\",\"PackFactoryNumber_Hidden_Priv\",\"FormingDate_DB\",\"FormingDate_Hidden_Priv\",\"Volume_DB\",\"Volume_Hidden_Priv\",\"Mass_DB\",\"Mass_Hidden_Priv\",\"PassportNumber_DB\",\"PassportNumber_Hidden_Priv\",\"Radionuclids_DB\",\"SpecificActivity_DB\",\"ProviderOrRecieverOKPO_DB\",\"ProviderOrRecieverOKPO_Hidden_~\",\"TransporterOKPO_DB\",\"TransporterOKPO_Hidden_Priv\",\"StoragePlaceName_DB\",\"StoragePlaceName_Hidden_Priv\",\"StoragePlaceCode_DB\",\"StoragePlaceCode_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"CodeRAO_DB\",\"StatusRAO_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"RefineOrSortRAOCode_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\" FROM \"form_17\"");
        migrationBuilder.Sql("SELECT \"Id\",CAST(\"Quantity_DB\" AS INTEGER) FROM \"form_17\"");
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
                Quantity_DB = table.Column<int>(type: "INTEGER", nullable: true),
                TritiumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                BetaGammaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                AlphaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                TransuraniumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                RefineOrSortRAOCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
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
        string b = "\"Id\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"ProjectVolume_DB\",\"CodeRAO_DB\",\"Volume_DB\",\"Mass_DB\",\"QuantityOZIII_DB\",\"SummaryActivity_DB\",\"DocumentNumber_DB\",\"" +
                   "DocumentDate_DB\",\"ExpirationDate_DB\",\"DocumentName_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"CorrectionNumber_DB\"";
        migrationBuilder.Sql(
            $"INSERT INTO \"FORM_17_TEMP\" ({b}) SELECT {b} FROM FORM_17_NEW_COLUMN INNER JOIN \"form_17_tmp\" ON \"Id\"=\"IdNew\"");
        migrationBuilder.DropTable("FORM_17_NEW_COLUMN");
        migrationBuilder.DropTable("form_17_tmp");
    }
}
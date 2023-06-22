using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_9 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "NotesId",
            table: "ReportCollection_DbSet");
        var b = "\"Sum_DB\",\"PackName_DB\",\"PackName_Hidden_Priv\",\"PackType_DB\",\"PackType_Hidden_Priv\",\"PackNumber_DB\",\"PackNumber_Hidden_Priv\",\"PackFactoryNumber_DB\",\"" +
                "PackFactoryNumber_Hidden_Priv\",\"FormingDate_DB\",\"FormingDate_Hidden_Priv\",\"Volume_DB\",\"Volume_Hidden_Priv\",\"Mass_DB\",\"Mass_Hidden_Priv\",\"PassportNumber_DB\",\"PassportNumber_Hidden_Priv\",\"" +
                "Radionuclids_DB\",\"SpecificActivity_DB\",\"ProviderOrRecieverOKPO_DB\",\"ProviderOrRecieverOKPO_Hidden_~\",\"TransporterOKPO_DB\",\"TransporterOKPO_Hidden_Priv\",\"StoragePlaceName_DB\",\"" +
                "StoragePlaceName_Hidden_Priv\",\"StoragePlaceCode_DB\",\"StoragePlaceCode_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"CodeRAO_DB\",\"StatusRAO_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"Quantity_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\",\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"RefineOrSortRAOCode_DB\",\"ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"" +
                "NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"" +
                "DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"DocumentDate_Hidden_Priv\"";
        migrationBuilder.Sql($"INSERT INTO \"form_17\" ({b}) SELECT {b} FROM FORM_17_TEMP");
        migrationBuilder.DropTable("FORM_17_TEMP");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "NotesId",
            table: "ReportCollection_DbSet",
            type: "INTEGER",
            nullable: true);
        migrationBuilder.CreateTable(
            name: "form_17_tmp",
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
            });
        migrationBuilder.CreateTable(
            name: "form_17_new_column",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                Quantity_DB = table.Column<int>(type: "INTEGER", nullable: true)
            });
        migrationBuilder.CreateTable(
            name: "FORM_17_TEMP",
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
            });
    }
}
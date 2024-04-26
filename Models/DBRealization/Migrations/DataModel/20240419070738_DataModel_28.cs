using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel;

// Создаём временные таблицы для переноса в них данных из таблиц форм
public partial class DataModel_28 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        #region form10
        
        migrationBuilder.CreateTable(
            name: "form_10_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                RegNo_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OrganUprav_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                SubjectRF_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                JurLico_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ShortJurLico_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                JurLicoAddress_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                JurLicoFactAddress_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                GradeFIO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Telephone_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Fax_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Email_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Okpo_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Okved_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Okogu_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Oktmo_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Inn_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Kpp_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Okopf_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Okfs_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_10_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
            });

        #endregion

        #region form11
        
        migrationBuilder.CreateTable(
            name: "form_11_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Type_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FactoryNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Activity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreatorOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Owner_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_11_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                Quantity_DB = table.Column<int>(type: "INTEGER", nullable: true),
                Category_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                SignedServicePeriod_DB = table.Column<float>(type: "FLOAT", nullable: true),
                PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            });

        #endregion

        #region form12
        
        migrationBuilder.CreateTable(
            name: "form_12_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NameIOU_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FactoryNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Mass_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreatorOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                SignedServicePeriod_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Owner_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_12_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            });

        #endregion

        #region form13
        
        migrationBuilder.CreateTable(
            name: "form_13_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Type_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FactoryNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Activity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreatorOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Owner_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_13_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                AggregateState_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            });

        #endregion

        #region form14

        migrationBuilder.CreateTable(
            name: "form_14_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Name_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Activity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ActivityMeasurementDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Volume_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Mass_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Owner_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_14_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                Sort_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                AggregateState_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            });

        #endregion

        #region form15

        migrationBuilder.CreateTable(
            name: "form_15_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Type_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FactoryNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Activity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StatusRAO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                RefineOrSortRAOCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Subsidy_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FcpNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_15_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                Quantity_DB = table.Column<int>(type: "INTEGER", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            });

        #endregion

        #region form16

        migrationBuilder.CreateTable(
            name: "form_16_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                CodeRAO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StatusRAO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Volume_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Mass_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                MainRadionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TritiumActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                BetaGammaActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                AlphaActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransuraniumActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ActivityMeasurementDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                QuantityOZIII_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Subsidy_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FcpNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                RefineOrSortRAOCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_16_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            });

        #endregion

        #region form17

        migrationBuilder.CreateTable(
            name: "form_17_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackFactoryNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormingDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Volume_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Mass_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                SpecificActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Subsidy_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FcpNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CodeRAO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StatusRAO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                VolumeOutOfPack_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                MassOutOfPack_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Quantity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TritiumActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                BetaGammaActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                AlphaActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransuraniumActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                RefineOrSortRAOCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_17_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackType_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackFactoryNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                FormingDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Volume_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Mass_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PassportNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                ProviderOrRecieverOKPO_Hidden_ = table.Column<bool>(name: "ProviderOrRecieverOKPO_Hidden_~", type: "BOOLEAN", nullable: false),
                TransporterOKPO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            });

        #endregion

        #region form18

        migrationBuilder.CreateTable(
            name: "form_18_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                IndividualNumberZHRO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Volume6_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Mass7_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                SaltConcentration_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                SpecificActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CodeRAO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StatusRAO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Volume20_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Mass21_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TritiumActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                BetaGammaActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                AlphaActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransuraniumActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                RefineOrSortRAOCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Subsidy_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FcpNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_18_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                IndividualNumberZHRO_Hidden_Pr = table.Column<bool>(name: "IndividualNumberZHRO_Hidden_Pr~", type: "BOOLEAN", nullable: false),
                PassportNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Volume6_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Mass7_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                SaltConcentration_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                ProviderOrRecieverOKPO_Hidden_ = table.Column<bool>(name: "ProviderOrRecieverOKPO_Hidden_~", type: "BOOLEAN", nullable: false),
                TransporterOKPO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            });

        #endregion

        #region form19

        migrationBuilder.CreateTable(
            name: "form_19_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Activity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_19_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                CodeTypeAccObject_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            });

        #endregion

        #region form20
        
        migrationBuilder.CreateTable(
            name: "form_20_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                RegNo_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OrganUprav_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                SubjectRF_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                JurLico_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ShortJurLico_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                JurLicoAddress_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                JurLicoFactAddress_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                GradeFIO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Telephone_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Fax_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Email_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Okpo_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Okved_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Okogu_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Oktmo_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Inn_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Kpp_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Okopf_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Okfs_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_20_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
            });

        #endregion

        #region form21
        
        migrationBuilder.CreateTable(
            name: "form_21_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                RefineMachineName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                MachinePower_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberOfHoursPerYear_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CodeRAOIn_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StatusRAOIn_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                VolumeIn_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                MassIn_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                QuantityIn_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TritiumActivityIn_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                BetaGammaActivityIn_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                AlphaActivityIn_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransuraniumActivityIn_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CodeRAOout_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StatusRAOout_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                VolumeOut_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                MassOut_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                QuantityOZIIIout_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TritiumActivityOut_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                BetaGammaActivityOut_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                AlphaActivityOut_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransuraniumActivityOut_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrderSum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_21_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                _RefineMachineName_Hidden_Get = table.Column<bool>(type: "BOOLEAN", nullable: false),
                MachineCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                _MachineCode_Hidden_Get = table.Column<bool>(type: "BOOLEAN", nullable: false),
                _MachinePower_Hidden_Get = table.Column<bool>(type: "BOOLEAN", nullable: false),
                _NumberOfHoursPerYear_Hidden_Get = table.Column<bool>(name: "_NumberOfHoursPerYear_Hidden_G~", type: "BOOLEAN", nullable: false),
                CodeRAOIn_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false),
                CodeRAOout_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false),
                StatusRAOIn_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false),
                StatusRAOout_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false),
                _MachineCode_Hidden_Set = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false),
                _MachinePower_Hidden_Set = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false),
                _NumberOfHoursPerYear_Hidden_Set = table.Column<bool>(name: "_NumberOfHoursPerYear_Hidden_S~", type: "BOOLEAN", nullable: false, defaultValue: false),
                _RefineMachineName_Hidden_Set = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false),
                SumGroup_DB = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false),
                _BaseColor = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
            });

        #endregion

        #region form22
        
        migrationBuilder.CreateTable(
            name: "form_22_editableColumns",
            columns: table => new
            {
                IdNew = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                StoragePlaceName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackQuantity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CodeRAO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StatusRAO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                VolumeInPack_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                MassInPack_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                VolumeOutOfPack_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                MassOutOfPack_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                QuantityOZIII_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TritiumActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                BetaGammaActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                AlphaActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransuraniumActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                MainRadionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Subsidy_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FcpNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrderSum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            });

        migrationBuilder.CreateTable(
            name: "form_22_withoutEditableColumns",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                _PackName_Hidden_Get = table.Column<bool>(type: "BOOLEAN", nullable: false),
                _PackName_Hidden_Set = table.Column<bool>(type: "BOOLEAN", nullable: false),
                _PackType_Hidden_Get = table.Column<bool>(type: "BOOLEAN", nullable: false),
                _PackType_Hidden_Set = table.Column<bool>(type: "BOOLEAN", nullable: false),
                _StoragePlaceCode_Hidden_Get = table.Column<bool>(type: "BOOLEAN", nullable: false),
                _StoragePlaceCode_Hidden_Set = table.Column<bool>(type: "BOOLEAN", nullable: false),
                _StoragePlaceName_Hidden_Get = table.Column<bool>(type: "BOOLEAN", nullable: false),
                _StoragePlaceName_Hidden_Set = table.Column<bool>(type: "BOOLEAN", nullable: false),
                MassInPack_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                MassInPack_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                VolumeInPack_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                VolumeInPack_Hidden_Priv2 = table.Column<bool>(type: "BOOLEAN", nullable: false),
                FcpNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Subsidy_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                CodeRAO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StatusRAO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                MainRadionuclids_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false),
                SumGroup_DB = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false),
                _BaseColor = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
            });

        #endregion
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}
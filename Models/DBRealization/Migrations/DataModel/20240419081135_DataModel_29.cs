using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel;

// Заполняем временные таблицы данными из таблиц форм, удаляем таблицы форм и создаём их заново с нужными типами данных
public partial class DataModel_29 : Migration
{
    private static string Trim(string column, ushort length) => $"LEFT(TRIM(\"{column}\"), {length})";

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        #region form10
        
        const string columnsWithEditableTypes10 =
            "\"RegNo_DB\", \"OrganUprav_DB\", \"SubjectRF_DB\", \"JurLico_DB\", \"ShortJurLico_DB\", " +
            "\"JurLicoAddress_DB\", \"JurLicoFactAddress_DB\", \"GradeFIO_DB\", \"Telephone_DB\", \"Fax_DB\", " +
            "\"Email_DB\", \"Okpo_DB\", \"Okved_DB\", \"Okogu_DB\", \"Oktmo_DB\", \"Inn_DB\", \"Kpp_DB\", " +
            "\"Okopf_DB\", \"Okfs_DB\", \"FormNum_DB\"";

        const string columnsWithoutEditableTypes10 = "\"ReportId\", \"NumberInOrder_DB\", \"NumberOfFields_DB\"";

        migrationBuilder.Sql($"INSERT INTO \"form_10_editableColumns\" (\"IdNew\", {columnsWithEditableTypes10}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes10} " +
                             "FROM \"form_10\"");

        migrationBuilder.Sql($"INSERT INTO \"form_10_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes10}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes10}" +
                             "FROM \"form_10\"");

        migrationBuilder.DropTable(name: "form_10");

        migrationBuilder.CreateTable(
            name: "form_10",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
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
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_10", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_10_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            }); 
        
        #endregion

        #region form11
        
        const string columnsWithEditableTypes11 =
            "\"PassportNumber_DB\", \"Type_DB\", \"Radionuclids_DB\", \"FactoryNumber_DB\", \"Activity_DB\", " +
            "\"CreationDate_DB\", \"CreatorOKPO_DB\", \"Owner_DB\", \"ProviderOrRecieverOKPO_DB\", " +
            "\"TransporterOKPO_DB\", \"PackName_DB\", \"PackType_DB\", \"PackNumber_DB\", \"FormNum_DB\", " +
            "\"OperationCode_DB\", \"OperationDate_DB\", \"DocumentNumber_DB\", \"DocumentDate_DB\"";

        const string columnsWithoutEditableTypes11 =
            "\"Quantity_DB\", \"Category_DB\", \"SignedServicePeriod_DB\", \"PropertyCode_DB\", \"ReportId\", " +
            "\"NumberInOrder_DB\", \"NumberOfFields_DB\", \"OperationCode_Hidden_Priv\", \"OperationDate_Hidden_Priv\", " +
            "\"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", \"DocumentNumber_Hidden_Priv\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_11_editableColumns\" (\"IdNew\", {columnsWithEditableTypes11}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes11} " +
                             "FROM \"form_11\"");

        migrationBuilder.Sql($"INSERT INTO \"form_11_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes11}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes11}" +
                             "FROM \"form_11\"");

        migrationBuilder.DropTable(name: "form_11");

        migrationBuilder.CreateTable(
            name: "form_11",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Type_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FactoryNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Quantity_DB = table.Column<int>(type: "INTEGER", nullable: true),
                Activity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreatorOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Category_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                SignedServicePeriod_DB = table.Column<float>(type: "FLOAT", nullable: true),
                PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                Owner_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_11", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_11_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            }); 
        
        #endregion

        #region form12
        
        const string columnsWithEditableTypes12 =
            "\"PassportNumber_DB\", \"NameIOU_DB\", \"FactoryNumber_DB\", \"Mass_DB\", \"CreatorOKPO_DB\", " + 
            "\"CreationDate_DB\", \"SignedServicePeriod_DB\", \"Owner_DB\", \"ProviderOrRecieverOKPO_DB\", " + 
            "\"TransporterOKPO_DB\", \"PackName_DB\", \"PackType_DB\", \"PackNumber_DB\", \"FormNum_DB\", " + 
            "\"OperationCode_DB\", \"OperationDate_DB\", \"DocumentNumber_DB\", \"DocumentDate_DB\"";

        const string columnsWithoutEditableTypes12 =
            "\"PropertyCode_DB\", \"ReportId\", \"NumberInOrder_DB\", \"NumberOfFields_DB\", " +
            "\"OperationCode_Hidden_Priv\", \"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", " +
            "\"DocumentNumber_Hidden_Priv\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_12_editableColumns\" (\"IdNew\", {columnsWithEditableTypes12}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes12} " +
                             "FROM \"form_12\"");

        migrationBuilder.Sql($"INSERT INTO \"form_12_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes12}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes12}" +
                             "FROM \"form_12\"");

        migrationBuilder.DropTable(name: "form_12");

        migrationBuilder.CreateTable(
            name: "form_12",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NameIOU_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FactoryNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Mass_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreatorOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                SignedServicePeriod_DB = table.Column<float>(type: "VARCHAR(255)", nullable: true),
                PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                Owner_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_12", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_12_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        #endregion

        #region form13

        const string columnsWithEditableTypes13 =
            "\"PassportNumber_DB\", \"Type_DB\", \"Radionuclids_DB\", \"FactoryNumber_DB\", \"Activity_DB\", " +
            "\"CreationDate_DB\", \"CreatorOKPO_DB\", \"Owner_DB\", \"ProviderOrRecieverOKPO_DB\", " +
            "\"TransporterOKPO_DB\", \"PackName_DB\", \"PackType_DB\", \"PackNumber_DB\", \"FormNum_DB\", " +
            "\"OperationCode_DB\", \"OperationDate_DB\", \"DocumentNumber_DB\", \"DocumentDate_DB\"";

        const string columnsWithoutEditableTypes13 =
            "\"AggregateState_DB\", \"PropertyCode_DB\", \"ReportId\", \"NumberInOrder_DB\", " +
            "\"NumberOfFields_DB\", \"OperationCode_Hidden_Priv\", \"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", " +
            "\"DocumentVid_Hidden_Priv\", \"DocumentNumber_Hidden_Priv\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_13_editableColumns\" (\"IdNew\", {columnsWithEditableTypes13}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes13} " +
                             "FROM \"form_13\"");

        migrationBuilder.Sql($"INSERT INTO \"form_13_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes13}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes13}" +
                             "FROM \"form_13\"");

        migrationBuilder.DropTable(name: "form_13");

        migrationBuilder.CreateTable(
            name: "form_13",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Type_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FactoryNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Activity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CreatorOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                AggregateState_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                Owner_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_13", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_13_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        #endregion

        #region form14

        const string columnsWithEditableTypes14 =
            "\"PassportNumber_DB\", \"Name_DB\", \"Radionuclids_DB\", \"Activity_DB\", \"ActivityMeasurementDate_DB\", " +
            "\"Volume_DB\", \"Mass_DB\", \"Owner_DB\", \"ProviderOrRecieverOKPO_DB\", \"TransporterOKPO_DB\", " +
            "\"PackName_DB\", \"PackType_DB\", \"PackNumber_DB\", \"FormNum_DB\", \"OperationCode_DB\", " +
            "\"OperationDate_DB\", \"DocumentNumber_DB\", \"DocumentDate_DB\"";

        const string columnsWithoutEditableTypes14 =
            "\"Sort_DB\", \"AggregateState_DB\", \"PropertyCode_DB\", \"ReportId\", \"NumberInOrder_DB\", " +
            "\"NumberOfFields_DB\", \"OperationCode_Hidden_Priv\", \"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", " +
            "\"DocumentVid_Hidden_Priv\", \"DocumentNumber_Hidden_Priv\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_14_editableColumns\" (\"IdNew\", {columnsWithEditableTypes14}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes14} " +
                             "FROM \"form_14\"");

        migrationBuilder.Sql($"INSERT INTO \"form_14_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes14}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes14}" +
                             "FROM \"form_14\"");

        migrationBuilder.DropTable(name: "form_14");

        migrationBuilder.CreateTable(
            name: "form_14",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Name_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Sort_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Activity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ActivityMeasurementDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Volume_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Mass_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                AggregateState_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                Owner_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_14", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_14_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        #endregion

        #region form15

        const string columnsWithEditableTypes15 =
            "\"PassportNumber_DB\", \"Type_DB\", \"Radionuclids_DB\", \"FactoryNumber_DB\", \"Activity_DB\", " +
            "\"CreationDate_DB\", \"StatusRAO_DB\", \"ProviderOrRecieverOKPO_DB\", \"TransporterOKPO_DB\", " + 
            "\"PackName_DB\", \"PackType_DB\", \"PackNumber_DB\", \"StoragePlaceName_DB\", \"StoragePlaceCode_DB\", " + 
            "\"RefineOrSortRAOCode_DB\", \"Subsidy_DB\", \"FcpNumber_DB\", \"FormNum_DB\", \"OperationCode_DB\", " + 
            "\"OperationDate_DB\", \"DocumentNumber_DB\", \"DocumentDate_DB\"";

        const string columnsWithoutEditableTypes15 =
            "\"Quantity_DB\", \"ReportId\", \"NumberInOrder_DB\", \"NumberOfFields_DB\", \"OperationCode_Hidden_Priv\", " + 
            "\"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", " + 
            "\"DocumentNumber_Hidden_Priv\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_15_editableColumns\" (\"IdNew\", {columnsWithEditableTypes15}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes15} " +
                             "FROM \"form_15\"");

        migrationBuilder.Sql($"INSERT INTO \"form_15_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes15}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes15}" +
                             "FROM \"form_15\"");

        migrationBuilder.DropTable(name: "form_15");

        migrationBuilder.CreateTable(
            name: "form_15",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Type_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FactoryNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Quantity_DB = table.Column<int>(type: "INTEGER", nullable: true),
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
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_15", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_15_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        #endregion

        #region form16

        const string columnsWithEditableTypes16 =
            "\"CodeRAO_DB\", \"StatusRAO_DB\", \"Volume_DB\", \"Mass_DB\", \"MainRadionuclids_DB\", " + 
            "\"TritiumActivity_DB\", \"BetaGammaActivity_DB\", \"AlphaActivity_DB\", \"TransuraniumActivity_DB\", " + 
            "\"ActivityMeasurementDate_DB\", \"QuantityOZIII_DB\", \"ProviderOrRecieverOKPO_DB\", \"TransporterOKPO_DB\", " + 
            "\"PackName_DB\", \"PackType_DB\", \"PackNumber_DB\", \"StoragePlaceName_DB\", \"StoragePlaceCode_DB\", " + 
            "\"Subsidy_DB\", \"FcpNumber_DB\", \"RefineOrSortRAOCode_DB\", \"FormNum_DB\", \"OperationCode_DB\", " + 
            "\"OperationDate_DB\", \"DocumentNumber_DB\", \"DocumentDate_DB\"";

        const string columnsWithoutEditableTypes16 =
            "\"ReportId\", \"NumberInOrder_DB\", \"NumberOfFields_DB\", \"OperationCode_Hidden_Priv\", " + 
            "\"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", " + 
            "\"DocumentNumber_Hidden_Priv\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_16_editableColumns\" (\"IdNew\", {columnsWithEditableTypes16}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes16} " +
                             "FROM \"form_16\"");

        migrationBuilder.Sql($"INSERT INTO \"form_16_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes16}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes16}" +
                             "FROM \"form_16\"");

        migrationBuilder.DropTable(name: "form_16");

        migrationBuilder.CreateTable(
            name: "form_16",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
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
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_16", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_16_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        #endregion

        #region form17

        const string columnsWithEditableTypes17 =
            "\"PackName_DB\", \"PackType_DB\", \"PackNumber_DB\", \"PackFactoryNumber_DB\", \"FormingDate_DB\", " +
            "\"Volume_DB\", \"Mass_DB\", \"PassportNumber_DB\", \"Radionuclids_DB\", \"SpecificActivity_DB\", " +
            "\"ProviderOrRecieverOKPO_DB\", \"TransporterOKPO_DB\", \"StoragePlaceName_DB\", \"StoragePlaceCode_DB\", " +
            "\"Subsidy_DB\", \"FcpNumber_DB\", \"CodeRAO_DB\", \"StatusRAO_DB\", \"VolumeOutOfPack_DB\", " +
            "\"MassOutOfPack_DB\", \"Quantity_DB\", \"TritiumActivity_DB\", \"BetaGammaActivity_DB\", " +
            "\"AlphaActivity_DB\", \"TransuraniumActivity_DB\", \"RefineOrSortRAOCode_DB\", \"FormNum_DB\", " +
            "\"OperationCode_DB\", \"OperationDate_DB\", \"DocumentNumber_DB\", \"DocumentDate_DB\"";

        const string columnsWithoutEditableTypes17 =
            "\"Sum_DB\", \"PackName_Hidden_Priv\", \"PackType_Hidden_Priv\", \"PackNumber_Hidden_Priv\", " +
            "\"PackFactoryNumber_Hidden_Priv\", \"FormingDate_Hidden_Priv\", \"Volume_Hidden_Priv\", " + 
            "\"Mass_Hidden_Priv\", \"PassportNumber_Hidden_Priv\", \"ProviderOrRecieverOKPO_Hidden_~\", " + 
            "\"TransporterOKPO_Hidden_Priv\", \"StoragePlaceName_Hidden_Priv\", \"StoragePlaceCode_Hidden_Priv\", " +
            "\"ReportId\", \"NumberInOrder_DB\", \"NumberOfFields_DB\", \"OperationCode_Hidden_Priv\", " +
            "\"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", " +
            "\"DocumentNumber_Hidden_Priv\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_17_editableColumns\" (\"IdNew\", {columnsWithEditableTypes17}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes17} " +
                             "FROM \"form_17\"");

        migrationBuilder.Sql($"INSERT INTO \"form_17_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes17}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes17}" +
                             "FROM \"form_17\"");

        migrationBuilder.DropTable(name: "form_17");

        migrationBuilder.CreateTable(
            name: "form_17",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PackFactoryNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackFactoryNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                FormingDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FormingDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Volume_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Volume_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Mass_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Mass_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PassportNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                SpecificActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_Hidden_ = table.Column<bool>(name: "ProviderOrRecieverOKPO_Hidden_~", type: "BOOLEAN", nullable: false),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
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
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_17", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_17_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        #endregion

        #region form18

        const string columnsWithEditableTypes18 =
            "\"IndividualNumberZHRO_DB\", \"PassportNumber_DB\", \"Volume6_DB\", \"Mass7_DB\", \"SaltConcentration_DB\", " +
            "\"Radionuclids_DB\", \"SpecificActivity_DB\", \"ProviderOrRecieverOKPO_DB\", \"TransporterOKPO_DB\", " +
            "\"StoragePlaceName_DB\", \"StoragePlaceCode_DB\", \"CodeRAO_DB\", \"StatusRAO_DB\", \"Volume20_DB\", " +
            "\"Mass21_DB\", \"TritiumActivity_DB\", \"BetaGammaActivity_DB\", \"AlphaActivity_DB\", " +
            "\"TransuraniumActivity_DB\", \"RefineOrSortRAOCode_DB\", \"Subsidy_DB\", \"FcpNumber_DB\", " +
            "\"FormNum_DB\", \"OperationCode_DB\", \"OperationDate_DB\",\"DocumentNumber_DB\", \"DocumentDate_DB\"";

        const string columnsWithoutEditableTypes18 =
            "\"Sum_DB\", \"IndividualNumberZHRO_Hidden_Pr~\", \"PassportNumber_Hidden_Priv\", \"Volume6_Hidden_Priv\", " +
            "\"Mass7_Hidden_Priv\", \"SaltConcentration_Hidden_Priv\", \"ProviderOrRecieverOKPO_Hidden_~\", " +
            "\"TransporterOKPO_Hidden_Priv\", \"StoragePlaceName_Hidden_Priv\", \"StoragePlaceCode_Hidden_Priv\", " +
            "\"ReportId\", \"NumberInOrder_DB\", \"NumberOfFields_DB\", \"OperationCode_Hidden_Priv\", " +
            "\"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", " +
            "\"DocumentNumber_Hidden_Priv\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_18_editableColumns\" (\"IdNew\", {columnsWithEditableTypes18}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes18} " +
                             "FROM \"form_18\"");

        migrationBuilder.Sql($"INSERT INTO \"form_18_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes18}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes18}" +
                             "FROM \"form_18\"");

        migrationBuilder.DropTable(name: "form_18");

        migrationBuilder.CreateTable(
            name: "form_18",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                IndividualNumberZHRO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                IndividualNumberZHRO_Hidden_Pr = table.Column<bool>(name: "IndividualNumberZHRO_Hidden_Pr~", type: "BOOLEAN", nullable: false),
                PassportNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PassportNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Volume6_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Volume6_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Mass7_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Mass7_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                SaltConcentration_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                SaltConcentration_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                SpecificActivity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ProviderOrRecieverOKPO_Hidden_ = table.Column<bool>(name: "ProviderOrRecieverOKPO_Hidden_~", type: "BOOLEAN", nullable: false),
                TransporterOKPO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                TransporterOKPO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StoragePlaceCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
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
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_18", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_18_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        #endregion

        #region form19

        const string columnsWithEditableTypes19 =
            "\"Radionuclids_DB\", \"Activity_DB\", \"FormNum_DB\", \"OperationCode_DB\", \"OperationDate_DB\", " +
            "\"DocumentNumber_DB\", \"DocumentDate_DB\"";

        const string columnsWithoutEditableTypes19 =
            "\"CodeTypeAccObject_DB\", \"ReportId\", \"NumberInOrder_DB\", \"NumberOfFields_DB\", " +
            "\"OperationCode_Hidden_Priv\", \"OperationDate_Hidden_Priv\", \"DocumentVid_DB\", " +
            "\"DocumentVid_Hidden_Priv\", \"DocumentNumber_Hidden_Priv\", \"DocumentDate_Hidden_Priv\"";

        migrationBuilder.Sql($"INSERT INTO \"form_19_editableColumns\" (\"IdNew\", {columnsWithEditableTypes19}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes19} " +
                             "FROM \"form_19\"");

        migrationBuilder.Sql($"INSERT INTO \"form_19_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes19}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes19}" +
                             "FROM \"form_19\"");

        migrationBuilder.DropTable(name: "form_19");

        migrationBuilder.CreateTable(
            name: "form_19",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                CodeTypeAccObject_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                Radionuclids_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                Activity_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                OperationCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                OperationDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                OperationDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentVid_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                DocumentVid_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                DocumentDate_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                DocumentDate_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_19", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_19_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        #endregion

        #region form20
        
        const string columnsWithEditableTypes20 =
            "\"RegNo_DB\", \"OrganUprav_DB\", \"SubjectRF_DB\", \"JurLico_DB\", \"ShortJurLico_DB\", " +
            "\"JurLicoAddress_DB\", \"JurLicoFactAddress_DB\", \"GradeFIO_DB\", \"Telephone_DB\", \"Fax_DB\", " +
            "\"Email_DB\", \"Okpo_DB\", \"Okved_DB\", \"Okogu_DB\", \"Oktmo_DB\", \"Inn_DB\", \"Kpp_DB\", " +
            "\"Okopf_DB\", \"Okfs_DB\", \"FormNum_DB\"";

        const string columnsWithoutEditableTypes20 = "\"ReportId\", \"NumberInOrder_DB\", \"NumberOfFields_DB\"";

        migrationBuilder.Sql($"INSERT INTO \"form_20_editableColumns\" (\"IdNew\", {columnsWithEditableTypes20}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes20} " +
                             "FROM \"form_20\"");

        migrationBuilder.Sql($"INSERT INTO \"form_20_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes20}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes20}" +
                             "FROM \"form_20\"");

        migrationBuilder.DropTable(name: "form_20");

        migrationBuilder.CreateTable(
            name: "form_20",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
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
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_20", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_20_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            }); 
        
        #endregion

        #region form21
        
        const string columnsWithEditableTypes21 = 
            "\"RefineMachineName_DB\", \"MachinePower_DB\", \"NumberOfHoursPerYear_DB\", \"CodeRAOIn_DB\", " +
            "\"StatusRAOIn_DB\", \"VolumeIn_DB\", \"MassIn_DB\", \"QuantityIn_DB\", \"TritiumActivityIn_DB\", " +
            "\"BetaGammaActivityIn_DB\", \"AlphaActivityIn_DB\", \"TransuraniumActivityIn_DB\", \"CodeRAOout_DB\", " +
            "\"StatusRAOout_DB\", \"VolumeOut_DB\", \"MassOut_DB\", \"QuantityOZIIIout_DB\", " +
            "\"TritiumActivityOut_DB\", \"BetaGammaActivityOut_DB\", \"AlphaActivityOut_DB\", " +
            "\"TransuraniumActivityOut_DB\", \"FormNum_DB\", \"NumberInOrderSum_DB\"";

        const string columnsWithoutEditableTypes21 = 
            "\"Sum_DB\", \"_RefineMachineName_Hidden_Get\", \"MachineCode_DB\", \"_MachineCode_Hidden_Get\", " +
            "\"_MachinePower_Hidden_Get\", \"_NumberOfHoursPerYear_Hidden_G~\", \"CodeRAOIn_Hidden_Priv\", " +
            "\"ReportId\", \"NumberInOrder_DB\", \"NumberOfFields_DB\", \"CorrectionNumber_DB\", " +
            "\"CodeRAOout_Hidden_Priv\", \"StatusRAOIn_Hidden_Priv\", \"StatusRAOout_Hidden_Priv\", " +
            "\"_MachineCode_Hidden_Set\", \"_MachinePower_Hidden_Set\", \"_NumberOfHoursPerYear_Hidden_S~\", " +
            "\"_RefineMachineName_Hidden_Set\", \"SumGroup_DB\", \"_BaseColor\"";
        
        migrationBuilder.Sql($"INSERT INTO \"form_21_editableColumns\" (\"IdNew\", {columnsWithEditableTypes21}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes21} " +
                             "FROM \"form_21\"");

        migrationBuilder.Sql($"INSERT INTO \"form_21_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes21}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes21}" +
                             "FROM \"form_21\"");

        migrationBuilder.DropTable(name: "form_21");

        migrationBuilder.CreateTable(
            name: "form_21",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                RefineMachineName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                _RefineMachineName_Hidden_Get = table.Column<bool>(type: "BOOLEAN", nullable: false),
                MachineCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                _MachineCode_Hidden_Get = table.Column<bool>(type: "BOOLEAN", nullable: false),
                MachinePower_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                _MachinePower_Hidden_Get = table.Column<bool>(type: "BOOLEAN", nullable: false),
                NumberOfHoursPerYear_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                _NumberOfHoursPerYear_Hidden_Get = table.Column<bool>(name: "_NumberOfHoursPerYear_Hidden_G~", type: "BOOLEAN", nullable: false),
                CodeRAOIn_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CodeRAOIn_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
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
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
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
                _BaseColor = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                NumberInOrderSum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_21", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_21_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            }); 
        
        #endregion

        #region form22
        
        const string columnsWithEditableTypes22 = 
            "\"StoragePlaceName_DB\", \"StoragePlaceCode_DB\", \"PackName_DB\", \"PackType_DB\", " +
            "\"PackQuantity_DB\", \"CodeRAO_DB\", \"StatusRAO_DB\", \"VolumeInPack_DB\", \"MassInPack_DB\", " +
            "\"VolumeOutOfPack_DB\", \"MassOutOfPack_DB\", \"QuantityOZIII_DB\", \"TritiumActivity_DB\", " +
            "\"BetaGammaActivity_DB\", \"AlphaActivity_DB\", \"TransuraniumActivity_DB\", \"MainRadionuclids_DB\", " +
            "\"Subsidy_DB\", \"FcpNumber_DB\", \"FormNum_DB\", \"NumberInOrderSum_DB\"";

        const string columnsWithoutEditableTypes22 = 
            "\"_PackName_Hidden_Get\", \"_PackName_Hidden_Set\", \"_PackType_Hidden_Get\", " +
            "\"_PackType_Hidden_Set\", \"_StoragePlaceCode_Hidden_Get\", \"_StoragePlaceCode_Hidden_Set\", " +
            "\"_StoragePlaceName_Hidden_Get\", \"_StoragePlaceName_Hidden_Set\", \"MassInPack_Hidden_Priv\", " +
            "\"MassInPack_Hidden_Priv2\", \"VolumeInPack_Hidden_Priv\", \"VolumeInPack_Hidden_Priv2\", " +
            "\"FcpNumber_Hidden_Priv\", \"Subsidy_Hidden_Priv\", \"Sum_DB\", \"CodeRAO_Hidden_Priv\", " +
            "\"StatusRAO_Hidden_Priv\", \"MainRadionuclids_Hidden_Priv\", \"ReportId\", \"NumberInOrder_DB\", " +
            "\"NumberOfFields_DB\", \"CorrectionNumber_DB\", \"SumGroup_DB\", \"_BaseColor\"";
            
        migrationBuilder.Sql($"INSERT INTO \"form_22_editableColumns\" (\"IdNew\", {columnsWithEditableTypes22}) " +
                             $"SELECT \"Id\", {columnsWithEditableTypes22} " +
                             "FROM \"form_22\"");

        migrationBuilder.Sql($"INSERT INTO \"form_22_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes22}) " +
                             $"SELECT \"Id\", {columnsWithoutEditableTypes22}" +
                             "FROM \"form_22\"");

        migrationBuilder.DropTable(name: "form_22");

        migrationBuilder.CreateTable(
            name: "form_22",
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
                StoragePlaceName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StoragePlaceCode_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackName_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackType_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                PackQuantity_DB = table.Column<int>(type: "VARCHAR(255)", nullable: true),
                CodeRAO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                CodeRAO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                StatusRAO_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                StatusRAO_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
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
                MainRadionuclids_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                Subsidy_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                FcpNumber_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                FormNum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true),
                NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false),
                SumGroup_DB = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false),
                _BaseColor = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                NumberInOrderSum_DB = table.Column<string>(type: "VARCHAR(255)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey(name: "PK_form_22", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_22_ReportCollection_Db~",
                    column: x => x.ReportId,
                    principalTable: "ReportCollection_DbSet",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            }); 
        
        #endregion
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}
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
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
}
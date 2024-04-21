using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel;

// Заполняем временные таблицы данными из таблиц форм, удаляем таблицы форм и создаём их заново с нужными типами данных
public partial class DataModel_29 : Migration
{
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
        
        const string columnsWithEditableTypes14 =
            "\"PassportNumber_DB\", \"Type_DB\", \"Radionuclids_DB\", \"FactoryNumber_DB\", \"Activity_DB\", " + 
            "\"CreationDate_DB\", \"CreatorOKPO_DB\", \"Owner_DB\", \"ProviderOrRecieverOKPO_DB\", " + 
            "\"TransporterOKPO_DB\", \"PackName_DB\", \"PackType_DB\", \"PackNumber_DB\", \"FormNum_DB\", " + 
            "\"OperationCode_DB\", \"OperationDate_DB\", \"DocumentNumber_DB\", \"DocumentDate_DB\"";

        const string columnsWithoutEditableTypes14 =
            "\"AggregateState_DB\", \"PropertyCode_DB\", \"ReportId\", \"NumberInOrder_DB\", " +
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
                PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                Name_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                Sort_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                Activity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                ActivityMeasurementDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                Volume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                Mass_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                AggregateState_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                PropertyCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                Owner_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                ProviderOrRecieverOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                TransporterOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                PackName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                PackType_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                PackNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
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
                table.PrimaryKey(name: "PK_form_14", columns: x => x.Id);
                table.ForeignKey(
                    name: "FK_form_14_ReportCollection_Db~",
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
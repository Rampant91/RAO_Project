using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_29 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            const string columnsWithEditableTypes = 
                "\"PassportNumber_DB\", \"Type_DB\", \"Radionuclids_DB\", \"FactoryNumber_DB\", \"Activity_DB\", " + 
                "\"CreationDate_DB\", \"CreatorOKPO_DB\", \"Owner_DB\", \"ProviderOrRecieverOKPO_DB\", " + 
                "\"TransporterOKPO_DB\", \"PackName_DB\", \"PackType_DB\", \"PackNumber_DB\", \"FormNum_DB\", " + 
                "\"OperationCode_DB\", \"OperationDate_DB\", \"DocumentNumber_DB\", \"DocumentDate_DB\"";

            const string columnsWithoutEditableTypes = 
                "\"Quantity_DB\", \"Category_DB\", \"SignedServicePeriod_DB\", \"PropertyCode_DB\", \"ReportId\", " + 
                "\"NumberInOrder_DB\", \"NumberOfFields_DB\", \"OperationCode_Hidden_Priv\", \"OperationDate_Hidden_Priv\", " + 
                "\"DocumentVid_DB\", \"DocumentVid_Hidden_Priv\", \"DocumentNumber_Hidden_Priv\", \"DocumentDate_Hidden_Priv\"";

            migrationBuilder.Sql($"INSERT INTO \"form_11_editableColumns\" (\"IdNew\", {columnsWithEditableTypes}) " + 
                                 $"SELECT \"Id\", {columnsWithEditableTypes} " + 
                                 "FROM \"form_11\"");

            migrationBuilder.Sql($"INSERT INTO \"form_11_withoutEditableColumns\" (\"Id\", {columnsWithoutEditableTypes}) " +
                                 $"SELECT \"Id\", {columnsWithoutEditableTypes}" +
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

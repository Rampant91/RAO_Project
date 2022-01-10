using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_20 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string attrNoCol = "\"Id\",\"PassportNumber_DB\",\"NameIOU_DB\",\"FactoryNumber_DB\",\"Mass_DB\",\"CreatorOKPO_DB\",\"CreationDate_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO FORM_12_NEW_COLUMN (\"IdNew\",\"SignedServicePeriod_DB\") SELECT \"Id\",CAST(\"SignedServicePeriod_DB\" AS BLOB SUB_TYPE TEXT) FROM \"form_12\";");
            migrationBuilder.Sql("INSERT INTO \"form_12_tmp\" (" + attrNoCol + ") SELECT " + attrNoCol + " FROM \"form_12\"");
            migrationBuilder.DropTable(name: "form_12");
            migrationBuilder.CreateTable(
                name: "form_12",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NameIOU_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FactoryNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreatorOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SignedServicePeriod_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
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
                    OperationCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
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
                    table.PrimaryKey("PK_form_12", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_12_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            string attrWCol = "\"Id\",\"PassportNumber_DB\",\"NameIOU_DB\",\"FactoryNumber_DB\",\"Mass_DB\",\"CreatorOKPO_DB\",\"CreationDate_DB\",\"SignedServicePeriod_DB\",\"PropertyCode_DB\",\"Owner_DB\",\"ProviderOrRecieverOKPO_DB\",\"TransporterOKPO_DB\",\"PackName_DB\",\"PackType_DB\",\"PackNumber_DB\",\"" +
            "ReportId\",\"FormNum_DB\",\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"OperationCode_DB\",\"OperationCode_Hidden_Priv\",\"OperationDate_DB\",\"OperationDate_Hidden_Priv\",\"" +
            "DocumentVid_DB\",\"DocumentVid_Hidden_Priv\",\"DocumentNumber_DB\",\"DocumentNumber_Hidden_Priv\",\"DocumentDate_DB\",\"" +
            "DocumentDate_Hidden_Priv\"";
            migrationBuilder.Sql("INSERT INTO \"FORM_12_TEMP\" (" + attrWCol + ") SELECT " + attrWCol + " FROM FORM_12_NEW_COLUMN INNER JOIN \"form_12_tmp\" ON \"Id\"=\"IdNew\"");
            migrationBuilder.DropTable("FORM_12_NEW_COLUMN");
            migrationBuilder.DropTable("form_12_tmp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)//NOT READY YET
        {

        }
    }
}

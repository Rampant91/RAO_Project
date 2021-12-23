using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string a = "\"Id\",\"MassInPack_Hidden_Priv\",\"MassInPack_Hidden_Priv2\",\"VolumeInPack_Hidden_Priv\",\"VolumeInPack_Hidden_Priv2\",\"FcpNumber_Hidden_Priv\",\"Subsidy_Hidden_Priv\",\"Sum_DB\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"PackName_DB\",\"PackType_DB\",\"CodeRAO_DB\",\"CodeRAO_Hidden_Priv\",\"StatusRAO_DB\",\"" +
               "StatusRAO_Hidden_Priv\",\"VolumeInPack_DB\",\"MassInPack_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"QuantityOZIII_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\","+
               "\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"MainRadionuclids_DB\",\"MainRadionuclids_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"ReportId\",\"FormNum_DB\","+
               "\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"CorrectionNumber_DB\"";
            migrationBuilder.Sql("INSERT INTO FORM_22_NEW_COLUMN (\"IdNew\",\"PackQuantity_DB\") SELECT \"Id\",CAST(\"PackQuantity_DB\" AS BLOB SUB_TYPE TEXT) FROM \"form_22\";");
            migrationBuilder.Sql("INSERT INTO \"form_22_tmp\" (" + a + ") SELECT " + a + " FROM \"form_22\"");
            migrationBuilder.DropTable(name: "form_22");
            migrationBuilder.CreateTable(
                name: "form_22",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
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
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_22", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_22_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            string b = "\"Id\",\"MassInPack_Hidden_Priv\",\"MassInPack_Hidden_Priv2\",\"VolumeInPack_Hidden_Priv\",\"VolumeInPack_Hidden_Priv2\",\"FcpNumber_Hidden_Priv\",\"Subsidy_Hidden_Priv\",\"Sum_DB\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"PackName_DB\",\"PackType_DB\",\"PackQuantity_DB\",\"CodeRAO_DB\",\"CodeRAO_Hidden_Priv\",\"StatusRAO_DB\",\"" +
               "StatusRAO_Hidden_Priv\",\"VolumeInPack_DB\",\"MassInPack_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"QuantityOZIII_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\"," +
               "\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"MainRadionuclids_DB\",\"MainRadionuclids_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"ReportId\",\"FormNum_DB\"," +
               "\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"CorrectionNumber_DB\"";
            migrationBuilder.Sql("INSERT INTO \"FORM_22_TEMP\" (" + b + ") SELECT " + b + " FROM FORM_22_NEW_COLUMN INNER JOIN \"form_22_tmp\" ON \"Id\"=\"IdNew\"");
            migrationBuilder.DropTable("FORM_22_NEW_COLUMN");
            migrationBuilder.DropTable("form_22_tmp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string a = "\"Id\",\"MassInPack_Hidden_Priv\",\"MassInPack_Hidden_Priv2\",\"VolumeInPack_Hidden_Priv\",\"VolumeInPack_Hidden_Priv2\",\"FcpNumber_Hidden_Priv\",\"Subsidy_Hidden_Priv\",\"Sum_DB\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"PackName_DB\",\"PackType_DB\",\"CodeRAO_DB\",\"CodeRAO_Hidden_Priv\",\"StatusRAO_DB\",\"" +
               "StatusRAO_Hidden_Priv\",\"VolumeInPack_DB\",\"MassInPack_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"QuantityOZIII_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\"," +
               "\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"MainRadionuclids_DB\",\"MainRadionuclids_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"ReportId\",\"FormNum_DB\"," +
               "\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"CorrectionNumber_DB\"";
            migrationBuilder.Sql("INSERT INTO FORM_22_NEW_COLUMN (\"IdNew\",\"PackQuantity_DB\") SELECT \"Id\",CAST(\"PackQuantity_DB\" AS INTEGER) FROM \"form_22\";");
            migrationBuilder.Sql("INSERT INTO \"form_22_tmp\" (" + a + ") SELECT " + a + " FROM \"form_22\"");
            migrationBuilder.DropTable(name: "form_22");
            migrationBuilder.CreateTable(
                name: "form_22",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
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
                    PackQuantity_DB = table.Column<int>(type: "INTEGER", nullable: true),
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
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_22", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_22_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            string b = "\"Id\",\"MassInPack_Hidden_Priv\",\"MassInPack_Hidden_Priv2\",\"VolumeInPack_Hidden_Priv\",\"VolumeInPack_Hidden_Priv2\",\"FcpNumber_Hidden_Priv\",\"Subsidy_Hidden_Priv\",\"Sum_DB\",\"StoragePlaceName_DB\",\"StoragePlaceCode_DB\",\"PackName_DB\",\"PackType_DB\",\"PackQuantity_DB\",\"CodeRAO_DB\",\"CodeRAO_Hidden_Priv\",\"StatusRAO_DB\",\"" +
               "StatusRAO_Hidden_Priv\",\"VolumeInPack_DB\",\"MassInPack_DB\",\"VolumeOutOfPack_DB\",\"MassOutOfPack_DB\",\"QuantityOZIII_DB\",\"TritiumActivity_DB\",\"BetaGammaActivity_DB\"," +
               "\"AlphaActivity_DB\",\"TransuraniumActivity_DB\",\"MainRadionuclids_DB\",\"MainRadionuclids_Hidden_Priv\",\"Subsidy_DB\",\"FcpNumber_DB\",\"ReportId\",\"FormNum_DB\"," +
               "\"NumberInOrder_DB\",\"NumberOfFields_DB\",\"CorrectionNumber_DB\"";
            migrationBuilder.Sql("INSERT INTO \"FORM_22_TEMP\" (" + b + ") SELECT " + b + " FROM FORM_22_NEW_COLUMN INNER JOIN \"form_22_tmp\" ON \"Id\"=\"IdNew\"");
            migrationBuilder.DropTable("FORM_22_NEW_COLUMN");
            migrationBuilder.DropTable("form_22_tmp");
        }
    }
}

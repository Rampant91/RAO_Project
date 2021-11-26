using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Quantity_DB = table.Column<int>(type: "BLOB SUB_TYPE TEXT", nullable: true),
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}

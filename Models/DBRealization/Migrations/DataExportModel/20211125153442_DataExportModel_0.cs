using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataExportModel
{
    public partial class DataExportModel_0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DBObservable_DbSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBObservable_DbSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportsCollection_DbSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Master_DBId = table.Column<int>(type: "INTEGER", nullable: true),
                    DBObservableId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsCollection_DbSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportsCollection_DbSet_DBO~",
                        column: x => x.DBObservableId,
                        principalTable: "DBObservable_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReportCollection_DbSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    IsCorrection_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false),
                    FIOexecutor_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    GradeExecutor_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ExecPhone_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ExecEmail_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Comments_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NotesId = table.Column<int>(type: "INTEGER", nullable: true),
                    PermissionNumber_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PermissionIssueDate_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PermissionDocumentName_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ValidBegin_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ValidThru_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PermissionNumber1_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PermissionIssueDate1_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PermissionDocumentName1_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ValidBegin1_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ValidThru1_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ContractNumber_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ContractIssueDate2_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OrganisationReciever_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ValidBegin2_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ValidThru2_28_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PermissionNumber27_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PermissionIssueDate27_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PermissionDocumentName27_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ValidBegin27_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ValidThru27_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SourcesQuantity26_DB = table.Column<int>(type: "INTEGER", nullable: true),
                    Year_DB = table.Column<int>(type: "INTEGER", nullable: true),
                    StartPeriod_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    EndPeriod_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ExportDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportsId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCollection_DbSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCollection_DbSet_Repo~",
                        column: x => x.ReportsId,
                        principalTable: "ReportsCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_10",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    RegNo_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OrganUprav_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SubjectRF_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    JurLico_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ShortJurLico_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    JurLicoAddress_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    JurLicoFactAddress_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    GradeFIO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Telephone_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Fax_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Email_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okpo_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okved_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okogu_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Oktmo_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Inn_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Kpp_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okopf_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okfs_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_10", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_10_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_11",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Type_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FactoryNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Quantity_DB = table.Column<int>(type: "INTEGER", nullable: true),
                    Activity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreatorOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Category_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    SignedServicePeriod_DB = table.Column<float>(type: "FLOAT", nullable: true),
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
                    table.PrimaryKey("PK_form_11", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_11_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    SignedServicePeriod_DB = table.Column<float>(type: "FLOAT", nullable: true),
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
                    table.PrimaryKey("PK_form_12", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_12_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_13",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Type_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FactoryNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Activity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreatorOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
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
                    table.PrimaryKey("PK_form_13", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_13_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    table.PrimaryKey("PK_form_14", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_14_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_15",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Type_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FactoryNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Quantity_DB = table.Column<int>(type: "INTEGER", nullable: true),
                    Activity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CreationDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StatusRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransporterOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackType_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RefineOrSortRAOCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Subsidy_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
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
                    table.PrimaryKey("PK_form_15", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_15_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_16",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    CodeRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StatusRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MainRadionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TritiumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    BetaGammaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AlphaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransuraniumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ActivityMeasurementDate_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    QuantityOZIII_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransporterOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackType_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PackNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Subsidy_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
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
                    table.PrimaryKey("PK_form_16", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_16_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "form_18",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    IndividualNumberZHRO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    IndividualNumberZHRO_Hidden_Pr = table.Column<bool>(name: "IndividualNumberZHRO_Hidden_Pr~", type: "BOOLEAN", nullable: false),
                    PassportNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PassportNumber_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Volume6_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume6_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    Mass7_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass7_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    SaltConcentration_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SaltConcentration_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
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
                    CodeRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StatusRAO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Volume20_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Mass21_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TritiumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    BetaGammaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AlphaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransuraniumActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RefineOrSortRAOCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Subsidy_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
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
                    table.PrimaryKey("PK_form_18", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_18_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_19",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    CodeTypeAccObject_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Activity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
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
                    table.PrimaryKey("PK_form_19", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_19_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_20",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    RegNo_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    OrganUprav_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SubjectRF_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    JurLico_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ShortJurLico_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    JurLicoAddress_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    JurLicoFactAddress_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    GradeFIO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Telephone_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Fax_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Email_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okpo_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okved_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okogu_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Oktmo_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Inn_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Kpp_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okopf_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Okfs_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_20", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_20_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_21",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    Sum_DB = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    RefineMachineName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RefineMachineName_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    MachineCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    MachineCode_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    MachinePower_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MachinePower_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    NumberOfHoursPerYear_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberOfHoursPerYear_Hidden_Pr = table.Column<bool>(name: "NumberOfHoursPerYear_Hidden_Pr~", type: "BOOLEAN", nullable: false),
                    CodeRAOIn_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CodeRAOIn_Hidden_Priv = table.Column<bool>(type: "BOOLEAN", nullable: false),
                    StatusRAOIn_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    VolumeIn_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MassIn_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    QuantityIn_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TritiumActivityIn_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    BetaGammaActivityIn_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AlphaActivityIn_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransuraniumActivityIn_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CodeRAOout_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StatusRAOout_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    VolumeOut_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MassOut_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    QuantityOZIIIout_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TritiumActivityOut_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    BetaGammaActivityOut_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AlphaActivityOut_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TransuraniumActivityOut_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_21", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_21_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_210",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    IndicatorName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PlotName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PlotKadastrNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PlotCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    InfectedArea_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AvgGammaRaysDosePower_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MaxGammaRaysDosePower_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    WasteDensityAlpha_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    WasteDensityBeta_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_210", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_210_ReportCollection_D~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_211",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PlotName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PlotKadastrNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PlotCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    InfectedArea_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SpecificActivityOfPlot_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SpecificActivityOfLiquidPart_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SpecificActivityOfDensePart_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_211", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_211_ReportCollection_D~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_212",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    OperationCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    ObjectTypeCode_DB = table.Column<short>(type: "SMALLINT", nullable: true),
                    Radionuclids_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Activity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ProviderOrRecieverOKPO_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_212", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_212_ReportCollection_D~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_22",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
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
                    QuantityOZIII_DB = table.Column<int>(type: "INTEGER", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "form_24",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    CodeOYAT_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MassCreated_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    QuantityCreated_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MassFromAnothers_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    QuantityFromAnothers_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MassFromAnothersImported_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    QuantityFromAnothersImported_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MassAnotherReasons_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    QuantityAnotherReasons_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MassTransferredToAnother_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    QuantityTransferredToAnother_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MassRefined_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    QuantityRefined_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    MassRemovedFromAccount_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    QuantityRemovedFromAccount_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_24", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_24_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_25",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    StoragePlaceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CodeOYAT_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    StoragePlaceCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FcpNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FuelMass_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    CellMass_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Quantity_DB = table.Column<int>(type: "INTEGER", nullable: true),
                    BetaGammaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AlphaActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_25", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_25_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_26",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    ObservedSourceNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ControlledAreaName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    SupposedWasteSource_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    DistanceToWasteSource_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    TestDepth_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RadionuclidName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AverageYearConcentration_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_26", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_26_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_27",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    ObservedSourceNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RadionuclidName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AllowedWasteValue_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FactedWasteValue_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    WasteOutbreakPreviousYear_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_27", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_27_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_28",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    WasteSourceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    WasteRecieverName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RecieverTypeCode_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PoolDistrictName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AllowedWasteRemovalVolume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RemovedWasteVolume_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_28", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_28_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "form_29",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    WasteSourceName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    RadionuclidName_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    AllowedActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    FactedActivity_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true),
                    FormNum_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    NumberInOrder_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfFields_DB = table.Column<int>(type: "INTEGER", nullable: false),
                    CorrectionNumber_DB = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_29", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_29_ReportCollection_Db~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    RowNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    GraphNumber_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    Comment_DB = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    ReportId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_notes_ReportCollection_DbSe~",
                        column: x => x.ReportId,
                        principalTable: "ReportCollection_DbSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_form_10_ReportId",
                table: "form_10",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_11_ReportId",
                table: "form_11",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_12_ReportId",
                table: "form_12",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_13_ReportId",
                table: "form_13",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_14_ReportId",
                table: "form_14",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_15_ReportId",
                table: "form_15",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_16_ReportId",
                table: "form_16",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_17_ReportId",
                table: "form_17",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_18_ReportId",
                table: "form_18",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_19_ReportId",
                table: "form_19",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_20_ReportId",
                table: "form_20",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_21_ReportId",
                table: "form_21",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_210_ReportId",
                table: "form_210",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_211_ReportId",
                table: "form_211",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_212_ReportId",
                table: "form_212",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_22_ReportId",
                table: "form_22",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_23_ReportId",
                table: "form_23",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_24_ReportId",
                table: "form_24",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_25_ReportId",
                table: "form_25",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_26_ReportId",
                table: "form_26",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_27_ReportId",
                table: "form_27",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_28_ReportId",
                table: "form_28",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_form_29_ReportId",
                table: "form_29",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_notes_ReportId",
                table: "notes",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCollection_DbSet_Repo~",
                table: "ReportCollection_DbSet",
                column: "ReportsId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsCollection_DbSet_DBO~",
                table: "ReportsCollection_DbSet",
                column: "DBObservableId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportsCollection_DbSet_Mas~",
                table: "ReportsCollection_DbSet",
                column: "Master_DBId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportsCollection_DbSet_Rep~",
                table: "ReportsCollection_DbSet",
                column: "Master_DBId",
                principalTable: "ReportCollection_DbSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportsCollection_DbSet_Rep~",
                table: "ReportsCollection_DbSet");

            migrationBuilder.DropTable(
                name: "form_10");

            migrationBuilder.DropTable(
                name: "form_11");

            migrationBuilder.DropTable(
                name: "form_12");

            migrationBuilder.DropTable(
                name: "form_13");

            migrationBuilder.DropTable(
                name: "form_14");

            migrationBuilder.DropTable(
                name: "form_15");

            migrationBuilder.DropTable(
                name: "form_16");

            migrationBuilder.DropTable(
                name: "form_17");

            migrationBuilder.DropTable(
                name: "form_18");

            migrationBuilder.DropTable(
                name: "form_19");

            migrationBuilder.DropTable(
                name: "form_20");

            migrationBuilder.DropTable(
                name: "form_21");

            migrationBuilder.DropTable(
                name: "form_210");

            migrationBuilder.DropTable(
                name: "form_211");

            migrationBuilder.DropTable(
                name: "form_212");

            migrationBuilder.DropTable(
                name: "form_22");

            migrationBuilder.DropTable(
                name: "form_23");

            migrationBuilder.DropTable(
                name: "form_24");

            migrationBuilder.DropTable(
                name: "form_25");

            migrationBuilder.DropTable(
                name: "form_26");

            migrationBuilder.DropTable(
                name: "form_27");

            migrationBuilder.DropTable(
                name: "form_28");

            migrationBuilder.DropTable(
                name: "form_29");

            migrationBuilder.DropTable(
                name: "notes");

            migrationBuilder.DropTable(
                name: "ReportCollection_DbSet");

            migrationBuilder.DropTable(
                name: "ReportsCollection_DbSet");

            migrationBuilder.DropTable(
                name: "DBObservable_DbSet");
        }
    }
}

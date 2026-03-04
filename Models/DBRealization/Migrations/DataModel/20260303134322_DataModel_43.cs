using System;
using FirebirdSql.EntityFrameworkCore.Firebird.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_43 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "package_passport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PassportNum = table.Column<string>(type: "VARCHAR(32)", maxLength: 32, nullable: true),
                    PassportDate = table.Column<DateOnly>(type: "DATE", nullable: false),
                    PackageType = table.Column<string>(type: "VARCHAR(128)", maxLength: 128, nullable: true),
                    StatusRaoCode = table.Column<string>(type: "VARCHAR(16)", maxLength: 16, nullable: true),
                    TechSpecification = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    NameRao = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    ClassRao = table.Column<short>(type: "SMALLINT", nullable: false),
                    RaoDisposalNum = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    RaoDisposalDate = table.Column<DateOnly>(type: "DATE", nullable: true),
                    PackageIdCode = table.Column<string>(type: "VARCHAR(32)", maxLength: 32, nullable: true),
                    TypeAndIdPuod = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    Owner = table.Column<string>(type: "VARCHAR(256)", maxLength: 256, nullable: true),
                    OwnerOkpo = table.Column<string>(type: "VARCHAR(14)", maxLength: 14, nullable: true),
                    Manufacturer = table.Column<string>(type: "VARCHAR(256)", maxLength: 256, nullable: true),
                    ManufacturerOkpo = table.Column<string>(type: "VARCHAR(14)", maxLength: 14, nullable: true),
                    CertificateConformityNum = table.Column<string>(type: "VARCHAR(32)", maxLength: 32, nullable: true),
                    ManufactureDate = table.Column<DateOnly>(type: "DATE", nullable: false),
                    CertificateConformityStartPeri = table.Column<DateOnly>(name: "CertificateConformityStartPeri~", type: "DATE", nullable: false),
                    CertificateConformityEndPeriod = table.Column<DateOnly>(type: "DATE", nullable: false),
                    ServiceLife = table.Column<int>(type: "INTEGER", nullable: false),
                    TransferDate = table.Column<DateOnly>(type: "DATE", nullable: true),
                    DisposalMethod = table.Column<string>(type: "VARCHAR(8)", maxLength: 8, nullable: true),
                    PhysicochemicalForm = table.Column<string>(type: "VARCHAR(1024)", maxLength: 1024, nullable: true),
                    MorphologicalComposition = table.Column<string>(type: "VARCHAR(1024)", maxLength: 1024, nullable: true),
                    Flammability = table.Column<string>(type: "VARCHAR(256)", maxLength: 256, nullable: true),
                    MatrixMaterialType = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    FillingWasteDate = table.Column<DateOnly>(type: "DATE", nullable: false),
                    Diameter = table.Column<int>(type: "INTEGER", nullable: true),
                    Height = table.Column<int>(type: "INTEGER", nullable: true),
                    Length = table.Column<int>(type: "INTEGER", nullable: true),
                    Width = table.Column<int>(type: "INTEGER", nullable: true),
                    PackageMass = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    RaoMass = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    PackageVolume = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    RaoVolume = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    RadiationDoseRate10cm = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    RadiationDoseRate1m = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    LevelNonFixedPollutionAlpha = table.Column<double>(type: "DOUBLE PRECISION", nullable: true),
                    LevelNonFixedPollutionBetaGamma = table.Column<double>(type: "DOUBLE PRECISION", nullable: true),
                    HeatOutput = table.Column<double>(type: "DOUBLE PRECISION", nullable: true),
                    Notes = table.Column<string>(type: "VARCHAR(2048)", maxLength: 2048, nullable: true),
                    ResponsibleTransfer = table.Column<string>(type: "VARCHAR(256)", maxLength: 256, nullable: true),
                    GradeAuthorizedPersonTransfer = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    FioAuthorizedPersonTransfer = table.Column<string>(type: "VARCHAR(256)", maxLength: 256, nullable: true),
                    ResponsibleReception = table.Column<string>(type: "VARCHAR(256)", maxLength: 256, nullable: true),
                    GradeAuthorizedPersonReception = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    FioAuthorizedPersonReception = table.Column<string>(type: "VARCHAR(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_package_passport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "characteristic_package",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    PassportId = table.Column<int>(type: "INTEGER", nullable: true),
                    PackageIdNum = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    ClassRao = table.Column<short>(type: "SMALLINT", nullable: false),
                    CodeRao = table.Column<string>(type: "BLOB SUB_TYPE TEXT", nullable: true),
                    PrimaryPackageQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    PrimaryPackageVolume = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    PrimaryPackageMass = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    LongLivingActivity = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    TransuraniumActivity = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    AlphaActivity = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    BetaGammaActivity = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    TritiumActivity = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    TotalActivity = table.Column<double>(type: "DOUBLE PRECISION", nullable: false),
                    NuclearHazardousFissileNuclides = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_characteristic_package", x => x.Id);
                    table.ForeignKey(
                        name: "FK_characteristic_package_pack~",
                        column: x => x.PassportId,
                        principalTable: "package_passport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "radionuclid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Fb:ValueGenerationStrategy", FbValueGenerationStrategy.IdentityColumn),
                    CharacteristicId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "VARCHAR(8)", maxLength: 8, nullable: true),
                    Activity = table.Column<double>(type: "DOUBLE PRECISION", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_radionuclid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_radionuclid_characteristic_~",
                        column: x => x.CharacteristicId,
                        principalTable: "characteristic_package",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_characteristic_package_Pass~",
                table: "characteristic_package",
                column: "PassportId");

            migrationBuilder.CreateIndex(
                name: "IX_radionuclid_CharacteristicId",
                table: "radionuclid",
                column: "CharacteristicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "radionuclid");

            migrationBuilder.DropTable(
                name: "characteristic_package");

            migrationBuilder.DropTable(
                name: "package_passport");
        }
    }
}

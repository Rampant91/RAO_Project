using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel;

/// <summary>
/// DataModel_51: слепок последней выгрузки отчёта
/// (LastExportedCorrectionNumber_DB + LastExportedFingerprint_DB)
/// для напоминания о номере корректировки; у уже выгружавшихся отчётов
/// номер слепка заполняется из текущего CorrectionNumber_DB.
/// </summary>
/// <remarks>
/// Firebird: UPDATE по только что добавленной колонке в той же транзакции
/// даёт «Column unknown» — поэтому Sql с suppressTransaction (как DataModel_42).
/// </remarks>
[DbContext(typeof(DBModel))]
[Migration("20260907120000_DataModel_51")]
public partial class DataModel_51 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<short>(
            name: "LastExportedCorrectionNumber_DB",
            table: "ReportCollection_DbSet",
            type: "SMALLINT",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "LastExportedFingerprint_DB",
            table: "ReportCollection_DbSet",
            type: "VARCHAR(64)",
            maxLength: 64,
            nullable: true);

        // У отчётов с датой выгрузки считаем текущий N номером последней выгрузки.
        migrationBuilder.Sql(@"
UPDATE ""ReportCollection_DbSet""
SET ""LastExportedCorrectionNumber_DB"" = ""CorrectionNumber_DB""
WHERE ""ExportDate_DB"" IS NOT NULL AND ""ExportDate_DB"" <> ''",
            suppressTransaction: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "LastExportedCorrectionNumber_DB",
            table: "ReportCollection_DbSet");

        migrationBuilder.DropColumn(
            name: "LastExportedFingerprint_DB",
            table: "ReportCollection_DbSet");
    }
}

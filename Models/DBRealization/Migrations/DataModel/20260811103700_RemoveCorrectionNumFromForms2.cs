using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel;

/// <summary>
/// Удаляет <c>CorrectionNumber_DB</c> с form_2.x.
/// Drop идемпотентный (Firebird EXECUTE BLOCK): колонка могла быть снята раньше
/// под ambiguous id <c>20260617074927_DataModel_42</c> — см. <see cref="DatabaseMigrationHelper"/>.
/// </summary>
public partial class RemoveCorrectionNumFromForms2 : Migration
{
    private static readonly string[] Form2Tables =
    {
        "form_21", "form_22", "form_23", "form_24", "form_25", "form_26",
        "form_27", "form_28", "form_29", "form_210", "form_211", "form_212"
    };

    protected override void Up(MigrationBuilder migrationBuilder)
    {
        foreach (var table in Form2Tables)
        {
            migrationBuilder.Sql($@"
EXECUTE BLOCK AS
BEGIN
  IF (EXISTS(
    SELECT 1 FROM RDB$RELATION_FIELDS
    WHERE TRIM(RDB$RELATION_NAME) = '{table}'
      AND TRIM(RDB$FIELD_NAME) = 'CorrectionNumber_DB'
  )) THEN
    EXECUTE STATEMENT 'ALTER TABLE ""{table}"" DROP ""CorrectionNumber_DB""';
END");
        }
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        foreach (var table in Form2Tables)
        {
            migrationBuilder.AddColumn<short>(
                name: "CorrectionNumber_DB",
                table: table,
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);
        }
    }
}

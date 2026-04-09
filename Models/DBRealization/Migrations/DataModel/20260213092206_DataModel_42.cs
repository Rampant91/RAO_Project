using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_42 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ------------------------------------------------------------
            // form_57.Name_DB
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "Name_DB_temp",
                table: "form_57",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_57"" SET ""Name_DB_temp"" = ""Name_DB""",
            suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "Name_DB",
                table: "form_57");

            migrationBuilder.RenameColumn(
                name: "Name_DB_temp",
                table: "form_57",
                newName: "Name_DB");

            // ------------------------------------------------------------
            // form_56.Name_DB
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "Name_DB_temp",
                table: "form_56",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_56"" SET ""Name_DB_temp"" = ""Name_DB""",
            suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "Name_DB",
                table: "form_56");

            migrationBuilder.RenameColumn(
                name: "Name_DB_temp",
                table: "form_56",
                newName: "Name_DB");

            // ------------------------------------------------------------
            // form_55.Name_DB
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "Name_DB_temp",
                table: "form_55",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_55"" SET ""Name_DB_temp"" = ""Name_DB""",
        suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "Name_DB",
                table: "form_55");

            migrationBuilder.RenameColumn(
                name: "Name_DB_temp",
                table: "form_55",
                newName: "Name_DB");

            // ------------------------------------------------------------
            // form_55.Name_DB
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "Name_DB_temp",
                table: "form_55",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_55"" SET ""Name_DB_temp"" = ""Name_DB""", suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "Name_DB",
                table: "form_55");

            migrationBuilder.RenameColumn(
                name: "Name_DB_temp",
                table: "form_55",
                newName: "Name_DB");

            // ------------------------------------------------------------
            // form_55.Name_DB
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "Name_DB_temp",
                table: "form_55",
                type: "varchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_55"" SET ""Name_DB_temp"" = ""Name_DB""", suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "Name_DB",
                table: "form_55");

            migrationBuilder.RenameColumn(
                name: "Name_DB_temp",
                table: "form_55",
                newName: "Name_DB");

            // ------------------------------------------------------------
            // form_53.TypeORI_DB
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<short>(
                name: "TypeORI_DB_temp",
                table: "form_53",
                type: "SMALLINT",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_53"" SET ""TypeORI_DB_temp"" = CAST(""TypeORI_DB"" AS SMALLINT)", suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "TypeORI_DB",
                table: "form_53");

            migrationBuilder.RenameColumn(
                name: "TypeORI_DB_temp",
                table: "form_53",
                newName: "TypeORI_DB");


            // ------------------------------------------------------------
            // form_53.TypeORI_DB
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<short>(
                name: "TypeORI_DB_temp",
                table: "form_53",
                type: "SMALLINT",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_53"" SET ""TypeORI_DB_temp"" = CAST(""TypeORI_DB"" AS SMALLINT)", suppressTransaction: true    );

            migrationBuilder.DropColumn(
                name: "TypeORI_DB",
                table: "form_53");

            migrationBuilder.RenameColumn(
                name: "TypeORI_DB_temp",
                table: "form_53",
                newName: "TypeORI_DB");


            // ------------------------------------------------------------
            // form_51.OperationCode_DB
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "OperationCode_DB_temp",
                table: "form_51",
                type: "varchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_51"" SET ""OperationCode_DB_temp"" = ""OperationCode_DB""", suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "OperationCode_DB",
                table: "form_51");

            migrationBuilder.RenameColumn(
                name: "OperationCode_DB_temp",
                table: "form_51",
                newName: "OperationCode_DB");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // ------------------------------------------------------------
            // form_57.Name_DB: varchar(64) -> BLOB SUB_TYPE TEXT
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "Name_DB_temp",
                table: "form_57",
                type: "BLOB SUB_TYPE TEXT",
                nullable: true);

            // Копирование данных (varchar автоматически конвертируется в BLOB)
            migrationBuilder.Sql(@"UPDATE ""form_57"" SET ""Name_DB_temp"" = ""Name_DB""", suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "Name_DB",
                table: "form_57");

            migrationBuilder.RenameColumn(
                name: "Name_DB_temp",
                table: "form_57",
                newName: "Name_DB");

            // ------------------------------------------------------------
            // form_56.Name_DB: varchar(64) -> BLOB SUB_TYPE TEXT
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "Name_DB_temp",
                table: "form_56",
                type: "BLOB SUB_TYPE TEXT",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_56"" SET ""Name_DB_temp"" = ""Name_DB""", suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "Name_DB",
                table: "form_56");

            migrationBuilder.RenameColumn(
                name: "Name_DB_temp",
                table: "form_56",
                newName: "Name_DB");

            // ------------------------------------------------------------
            // form_55.Name_DB: varchar(64) -> BLOB SUB_TYPE TEXT
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "Name_DB_temp",
                table: "form_55",
                type: "BLOB SUB_TYPE TEXT",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_55"" SET ""Name_DB_temp"" = ""Name_DB""", suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "Name_DB",
                table: "form_55");

            migrationBuilder.RenameColumn(
                name: "Name_DB_temp",
                table: "form_55",
                newName: "Name_DB");

            // ------------------------------------------------------------
            // form_55.OperationCode_DB: varchar(2) -> BLOB SUB_TYPE TEXT
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "OperationCode_DB_temp",
                table: "form_55",
                type: "BLOB SUB_TYPE TEXT",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_55"" SET ""OperationCode_DB_temp"" = ""OperationCode_DB""", suppressTransaction: true  );

            migrationBuilder.DropColumn(
                name: "OperationCode_DB",
                table: "form_55");

            migrationBuilder.RenameColumn(
                name: "OperationCode_DB_temp",
                table: "form_55",
                newName: "OperationCode_DB");

            // ------------------------------------------------------------
            // form_54.TypeORI_DB: SMALLINT -> BLOB SUB_TYPE TEXT
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "TypeORI_DB_temp",
                table: "form_54",
                type: "BLOB SUB_TYPE TEXT",
                nullable: true);

            // Преобразование числа в строку (Firebird выполняет автоматически,
            // но для надёжности используем CAST)
            migrationBuilder.Sql(@"UPDATE ""form_54 SET"" ""TypeORI_DB_temp"" = CAST(""TypeORI_DB"" AS VARCHAR(10))", suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "TypeORI_DB",
                table: "form_54");

            migrationBuilder.RenameColumn(
                name: "TypeORI_DB_temp",
                table: "form_54",
                newName: "TypeORI_DB");

            // ------------------------------------------------------------
            // form_53.TypeORI_DB: SMALLINT -> BLOB SUB_TYPE TEXT
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "TypeORI_DB_temp",
                table: "form_53",
                type: "BLOB SUB_TYPE TEXT",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_53"" SET ""TypeORI_DB_temp"" = CAST(""TypeORI_DB"" AS VARCHAR(10))", suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "TypeORI_DB",
                table: "form_53");

            migrationBuilder.RenameColumn(
                name: "TypeORI_DB_temp",
                table: "form_53",
                newName: "TypeORI_DB");

            // ------------------------------------------------------------
            // form_53.OperationCode_DB: varchar(2) -> BLOB SUB_TYPE TEXT
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "OperationCode_DB_temp",
                table: "form_53",
                type: "BLOB SUB_TYPE TEXT",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_53"" SET ""OperationCode_DB_temp"" = ""OperationCode_DB""", suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "OperationCode_DB",
                table: "form_53");

            migrationBuilder.RenameColumn(
                name: "OperationCode_DB_temp",
                table: "form_53",
                newName: "OperationCode_DB");

            // ------------------------------------------------------------
            // form_51.OperationCode_DB: varchar(2) -> BLOB SUB_TYPE TEXT
            // ------------------------------------------------------------
            migrationBuilder.AddColumn<string>(
                name: "OperationCode_DB_temp",
                table: "form_51",
                type: "BLOB SUB_TYPE TEXT",
                nullable: true);

            migrationBuilder.Sql(@"UPDATE ""form_51"" SET ""OperationCode_DB_temp"" = ""OperationCode_DB""", suppressTransaction: true);

            migrationBuilder.DropColumn(
                name: "OperationCode_DB",
                table: "form_51");

            migrationBuilder.RenameColumn(
                name: "OperationCode_DB_temp",
                table: "form_51",
                newName: "OperationCode_DB");
        }
    }
}

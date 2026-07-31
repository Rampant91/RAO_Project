using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Models.DBRealization.Migrations.DataModel;

public partial class DataModel_42 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        #region form_51
        // ------------------------------------------------------------
        // form_51.OperationCode_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "OperationCode_DB_temp",
            table: "form_51",
            type: "varchar(2)",
            maxLength: 64,
            nullable: true);

        migrationBuilder.Sql(@"UPDATE ""form_51"" SET ""OperationCode_DB_temp"" = ""OperationCode_DB""",
        suppressTransaction: true);

        migrationBuilder.DropColumn(
            name: "OperationCode_DB",
            table: "form_51");

        migrationBuilder.RenameColumn(
            name: "OperationCode_DB_temp",
            table: "form_51",
            newName: "OperationCode_DB");
        #endregion

        #region form_53
        // ------------------------------------------------------------
        // form_53.OperationCode_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "OperationCode_DB_temp",
            table: "form_53",
            type: "varchar(2)",
            maxLength: 2,
            nullable: true);

        migrationBuilder.Sql(@"UPDATE ""form_53"" SET ""OperationCode_DB_temp"" = ""OperationCode_DB""",
        suppressTransaction: true);

        migrationBuilder.DropColumn(
            name: "OperationCode_DB",
            table: "form_53");

        migrationBuilder.RenameColumn(
            name: "OperationCode_DB_temp",
            table: "form_53",
            newName: "OperationCode_DB");

        // ------------------------------------------------------------
        // form_53.TypeORI_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "TypeORI_DB_temp",
            table: "form_53",
            type: "varchar(32)",
            maxLength: 32,
            nullable: true);

        migrationBuilder.Sql(@"UPDATE ""form_53"" SET ""TypeORI_DB_temp"" = ""TypeORI_DB""",
        suppressTransaction: true);

        migrationBuilder.DropColumn(
            name: "TypeORI_DB",
            table: "form_53");

        migrationBuilder.RenameColumn(
            name: "TypeORI_DB_temp",
            table: "form_53",
            newName: "TypeORI_DB");
        #endregion

        #region form_54

        // ------------------------------------------------------------
        // form_54.TypeORI_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "TypeORI_DB_temp",
            table: "form_54",
            type: "varchar(32)",
            maxLength: 32,
            nullable: true);

        migrationBuilder.Sql(@"UPDATE ""form_54"" SET ""TypeORI_DB_temp"" = ""TypeORI_DB""",
            suppressTransaction: true);

        migrationBuilder.DropColumn(
            name: "TypeORI_DB",
            table: "form_54");

        migrationBuilder.RenameColumn(
            name: "TypeORI_DB_temp",
            table: "form_54",
            newName: "TypeORI_DB");
        #endregion

        #region form_55
        // ------------------------------------------------------------
        // form_55.OperationCode_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "OperationCode_DB_temp",
            table: "form_55",
            type: "varchar(2)",
            maxLength: 2,
            nullable: true);

        migrationBuilder.Sql(@"UPDATE ""form_55"" SET ""OperationCode_DB_temp"" = ""OperationCode_DB""",
        suppressTransaction: true);

        migrationBuilder.DropColumn(
            name: "OperationCode_DB",
            table: "form_55");

        migrationBuilder.RenameColumn(
            name: "OperationCode_DB_temp",
            table: "form_55",
            newName: "OperationCode_DB");

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
        #endregion

        #region form_56

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
        #endregion

        #region form_57
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
        #endregion

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_21");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_22");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_23");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_24");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_25");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_26");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_27");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_28");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_29");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_210");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_211");

        migrationBuilder.DropColumn(
            name: "CorrectionNumber_DB",
            table: "form_212");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        #region form_51
        // ------------------------------------------------------------
        // form_51.OperationCode_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "OperationCode_DB_temp",
            table: "form_51",
            type: "BLOB SUB_TYPE TEXT",
            nullable: true);

        migrationBuilder.Sql(@"UPDATE ""form_51"" SET ""OperationCode_DB_temp"" = ""OperationCode_DB""",
        suppressTransaction: true);

        migrationBuilder.DropColumn(
            name: "OperationCode_DB",
            table: "form_51");

        migrationBuilder.RenameColumn(
            name: "OperationCode_DB_temp",
            table: "form_51",
            newName: "OperationCode_DB");
        #endregion

        #region form_53
        // ------------------------------------------------------------
        // form_53.OperationCode_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "OperationCode_DB_temp",
            table: "form_53",
            type: "BLOB SUB_TYPE TEXT",
            nullable: true);

        migrationBuilder.Sql(@"UPDATE ""form_53"" SET ""OperationCode_DB_temp"" = ""OperationCode_DB""",
        suppressTransaction: true);

        migrationBuilder.DropColumn(
            name: "OperationCode_DB",
            table: "form_53");

        migrationBuilder.RenameColumn(
            name: "OperationCode_DB_temp",
            table: "form_53",
            newName: "OperationCode_DB");

        // ------------------------------------------------------------
        // form_53.TypeORI_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "TypeORI_DB_temp",
            table: "form_53",
            type: "BLOB SUB_TYPE TEXT",
            nullable: true);


        migrationBuilder.Sql(@"UPDATE ""form_53"" SET ""TypeORI_DB_temp"" = ""TypeORI_DB""",
        suppressTransaction: true);

        migrationBuilder.DropColumn(
            name: "TypeORI_DB",
            table: "form_53");

        migrationBuilder.RenameColumn(
            name: "TypeORI_DB_temp",
            table: "form_53",
            newName: "TypeORI_DB");
        #endregion

        #region form_54

        // ------------------------------------------------------------
        // form_54.TypeORI_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "TypeORI_DB_temp",
            table: "form_54",
            type: "BLOB SUB_TYPE TEXT",
            nullable: true);

        migrationBuilder.Sql(@"UPDATE ""form_54"" SET ""TypeORI_DB_temp"" = ""TypeORI_DB""",
        suppressTransaction: true);

        migrationBuilder.DropColumn(
            name: "TypeORI_DB",
            table: "form_54");

        migrationBuilder.RenameColumn(
            name: "TypeORI_DB_temp",
            table: "form_54",
            newName: "TypeORI_DB");
        #endregion

        #region form_55
        // ------------------------------------------------------------
        // form_55.OperationCode_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "OperationCode_DB_temp",
            table: "form_55",
            type: "BLOB SUB_TYPE TEXT",
            nullable: true);

        migrationBuilder.Sql(@"UPDATE ""form_55"" SET ""OperationCode_DB_temp"" = ""OperationCode_DB""",
        suppressTransaction: true);

        migrationBuilder.DropColumn(
            name: "OperationCode_DB",
            table: "form_55");

        migrationBuilder.RenameColumn(
            name: "OperationCode_DB_temp",
            table: "form_55",
            newName: "OperationCode_DB");

        // ------------------------------------------------------------
        // form_55.Name_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "Name_DB_temp",
            table: "form_55",
            type: "BLOB SUB_TYPE TEXT",
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
        #endregion

        #region form_56

        // ------------------------------------------------------------
        // form_56.Name_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "Name_DB_temp",
            table: "form_56",
            type: "BLOB SUB_TYPE TEXT",
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
        #endregion

        #region form_57
        // ------------------------------------------------------------
        // form_57.Name_DB
        // ------------------------------------------------------------
        migrationBuilder.AddColumn<string>(
            name: "Name_DB_temp",
            table: "form_57",
            type: "BLOB SUB_TYPE TEXT",
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
        #endregion

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_21",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_22",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_23",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_24",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_25",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_26",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_27",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_28",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_29",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_210",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_211",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);

        migrationBuilder.AddColumn<short>(
            name: "CorrectionNumber_DB",
            table: "form_212",
            type: "SMALLINT",
            nullable: false,
            defaultValue: (short)0);
    }
}
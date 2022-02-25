using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations.DataModel
{
    public partial class DataModel_23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ReportCollection_DbSet");

            //migrationBuilder.AlterColumn<long>(
            //    name: "Order",
            //    table: "notes",
            //    type: "BIGINT",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "INTEGER");

            migrationBuilder.AddColumn<bool>(
                name: "SumGroup_DB",
                table: "form_21",
                type: "BOOLEAN",
                nullable: false,
                defaultValue: false);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SumGroup_DB",
                table: "form_21");


            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ReportCollection_DbSet",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.AlterColumn<int>(
            //    name: "Order",
            //    table: "notes",
            //    type: "INTEGER",
            //    nullable: false,
            //    oldClrType: typeof(long),
            //    oldType: "BIGINT");

        }
    }
}

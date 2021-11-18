using Microsoft.EntityFrameworkCore.Migrations;

namespace Models.DBRealization.Migrations
{
    public partial class DataContext_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberInOrder_DB",
                table: "form_20",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberInOrder_DB",
                table: "form_10",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberInOrder_DB",
                table: "form_20");

            migrationBuilder.DropColumn(
                name: "NumberInOrder_DB",
                table: "form_10");
        }
    }
}

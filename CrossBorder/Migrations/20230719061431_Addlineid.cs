using Microsoft.EntityFrameworkCore.Migrations;

namespace CrossBorder.Migrations
{
    public partial class Addlineid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "lineid",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lineid",
                table: "Customer");
        }
    }
}

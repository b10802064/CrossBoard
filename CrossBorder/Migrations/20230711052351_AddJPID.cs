using Microsoft.EntityFrameworkCore.Migrations;

namespace CrossBorder.Migrations
{
    public partial class AddJPID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductJP",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductJP",
                table: "Product");
        }
    }
}

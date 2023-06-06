using Microsoft.EntityFrameworkCore.Migrations;

namespace CrossBorder.Migrations
{
    public partial class AddDataCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "CustomerId", "CusdtomerName", "Email", "Password" },
                values: new object[] { "001", "haah", "haah@gmail.com", "12345" });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "CustomerId", "CusdtomerName", "Email", "Password" },
                values: new object[] { "002", "haaj", "haaj@gmail.com", "123456" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "CustomerId",
                keyValue: "001");

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "CustomerId",
                keyValue: "002");
        }
    }
}

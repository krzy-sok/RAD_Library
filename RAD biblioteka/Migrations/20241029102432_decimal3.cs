using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAD_biblioteka.Migrations
{
    public partial class decimal3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Book");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Book",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}

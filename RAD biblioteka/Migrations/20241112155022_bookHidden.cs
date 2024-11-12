using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAD_biblioteka.Migrations
{
    public partial class bookHidden : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Hidden",
                table: "Book",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hidden",
                table: "Book");
        }
    }
}

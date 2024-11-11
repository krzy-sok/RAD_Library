using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAD_biblioteka.Migrations
{
    public partial class LeaseType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Leases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Leases");
        }
    }
}

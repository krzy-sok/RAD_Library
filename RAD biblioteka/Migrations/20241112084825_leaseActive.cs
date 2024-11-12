using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAD_biblioteka.Migrations
{
    public partial class leaseActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Leases",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Leases");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAD_biblioteka.Migrations
{
    public partial class uniqueUName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "userName",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_User_userName",
                table: "User",
                column: "userName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_userName",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "userName",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}

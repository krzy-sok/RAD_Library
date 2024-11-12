using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAD_biblioteka.Migrations
{
    public partial class leaseObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Leases_BookId",
                table: "Leases",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_UserId",
                table: "Leases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_Book_BookId",
                table: "Leases",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_User_UserId",
                table: "Leases",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leases_Book_BookId",
                table: "Leases");

            migrationBuilder.DropForeignKey(
                name: "FK_Leases_User_UserId",
                table: "Leases");

            migrationBuilder.DropIndex(
                name: "IX_Leases_BookId",
                table: "Leases");

            migrationBuilder.DropIndex(
                name: "IX_Leases_UserId",
                table: "Leases");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RAD_biblioteka.Migrations
{
    public partial class leaseNoOldKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leases_Book_BookId",
                table: "Leases");

            migrationBuilder.DropForeignKey(
                name: "FK_Leases_User_UserId",
                table: "Leases");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Leases",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Leases",
                newName: "bookId");

            migrationBuilder.RenameIndex(
                name: "IX_Leases_UserId",
                table: "Leases",
                newName: "IX_Leases_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Leases_BookId",
                table: "Leases",
                newName: "IX_Leases_bookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_Book_bookId",
                table: "Leases",
                column: "bookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_User_userId",
                table: "Leases",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leases_Book_bookId",
                table: "Leases");

            migrationBuilder.DropForeignKey(
                name: "FK_Leases_User_userId",
                table: "Leases");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Leases",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "bookId",
                table: "Leases",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Leases_userId",
                table: "Leases",
                newName: "IX_Leases_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Leases_bookId",
                table: "Leases",
                newName: "IX_Leases_BookId");

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
    }
}

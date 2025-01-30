using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_knjiznica.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Books",
                newName: "IsBorrowed");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "BorrowedByUserId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BorrowedDate",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Genre",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedDate",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BorrowedByUserId",
                table: "Books",
                column: "BorrowedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_BorrowedByUserId",
                table: "Books",
                column: "BorrowedByUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_BorrowedByUserId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BorrowedByUserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BorrowedByUserId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BorrowedDate",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PublishedDate",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "Books");

            migrationBuilder.RenameColumn(
                name: "IsBorrowed",
                table: "Books",
                newName: "IsAvailable");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

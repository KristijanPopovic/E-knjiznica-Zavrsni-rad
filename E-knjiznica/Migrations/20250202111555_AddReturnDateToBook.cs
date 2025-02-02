using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_knjiznica.Migrations
{
    /// <inheritdoc />
    public partial class AddReturnDateToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "Books",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "Books");
        }
    }
}

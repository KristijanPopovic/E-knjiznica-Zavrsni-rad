using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_knjiznica.Migrations
{
    /// <inheritdoc />
    public partial class AddOpenLibraryIdToAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OpenLibraryId",
                table: "Authors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenLibraryId",
                table: "Authors");
        }
    }
}

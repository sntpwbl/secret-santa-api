using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecretSanta.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordAndDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GiftDescription",
                table: "People",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GiftDescription",
                table: "Groups",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiftDescription",
                table: "People");

            migrationBuilder.DropColumn(
                name: "GiftDescription",
                table: "Groups");
        }
    }
}

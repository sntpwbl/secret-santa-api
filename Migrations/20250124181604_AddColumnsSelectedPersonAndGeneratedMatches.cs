using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecretSanta.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsSelectedPersonAndGeneratedMatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SelectedPersonId",
                table: "People",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGeneratedMatches",
                table: "Groups",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedPersonId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "IsGeneratedMatches",
                table: "Groups");
        }
    }
}

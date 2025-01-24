using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecretSanta.Migrations
{
    /// <inheritdoc />
    public partial class FixGroupDescriptionField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("UPDATE `Groups` SET Description = GiftDescription");

            migrationBuilder.DropColumn(
            name: "GiftDescription",
            table: "Groups");
            }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "GiftDescription",
            table: "Groups",
            nullable: true);

            migrationBuilder.Sql("UPDATE `Groups` SET GiftDescription = Description");

            migrationBuilder.DropColumn(
            name: "Description",
            table: "Groups");

        }
    }
}

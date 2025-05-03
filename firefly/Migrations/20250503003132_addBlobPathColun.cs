using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace firefly.Migrations
{
    /// <inheritdoc />
    public partial class addBlobPathColun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlobPath",
                table: "ImageAssets",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlobPath",
                table: "ImageAssets");
        }
    }
}

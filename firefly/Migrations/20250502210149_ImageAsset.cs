using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace firefly.Migrations
{
    /// <inheritdoc />
    public partial class ImageAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageAsset_ImageGenerationJobs_JobId1",
                table: "ImageAsset");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageAsset",
                table: "ImageAsset");

            migrationBuilder.RenameTable(
                name: "ImageAsset",
                newName: "ImageAssets");

            migrationBuilder.RenameIndex(
                name: "IX_ImageAsset_JobId1",
                table: "ImageAssets",
                newName: "IX_ImageAssets_JobId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageAssets",
                table: "ImageAssets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageAssets_ImageGenerationJobs_JobId1",
                table: "ImageAssets",
                column: "JobId1",
                principalTable: "ImageGenerationJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageAssets_ImageGenerationJobs_JobId1",
                table: "ImageAssets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ImageAssets",
                table: "ImageAssets");

            migrationBuilder.RenameTable(
                name: "ImageAssets",
                newName: "ImageAsset");

            migrationBuilder.RenameIndex(
                name: "IX_ImageAssets_JobId1",
                table: "ImageAsset",
                newName: "IX_ImageAsset_JobId1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImageAsset",
                table: "ImageAsset",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageAsset_ImageGenerationJobs_JobId1",
                table: "ImageAsset",
                column: "JobId1",
                principalTable: "ImageGenerationJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace firefly.Migrations
{
    /// <inheritdoc />
    public partial class FixForeignKeyType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageAssets_ImageGenerationJobs_JobId1",
                table: "ImageAssets");

            migrationBuilder.DropIndex(
                name: "IX_ImageAssets_JobId1",
                table: "ImageAssets");

            migrationBuilder.DropColumn(
                name: "JobId1",
                table: "ImageAssets");

            migrationBuilder.CreateIndex(
                name: "IX_ImageAssets_JobId",
                table: "ImageAssets",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageAssets_ImageGenerationJobs_JobId",
                table: "ImageAssets",
                column: "JobId",
                principalTable: "ImageGenerationJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageAssets_ImageGenerationJobs_JobId",
                table: "ImageAssets");

            migrationBuilder.DropIndex(
                name: "IX_ImageAssets_JobId",
                table: "ImageAssets");

            migrationBuilder.AddColumn<Guid>(
                name: "JobId1",
                table: "ImageAssets",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ImageAssets_JobId1",
                table: "ImageAssets",
                column: "JobId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageAssets_ImageGenerationJobs_JobId1",
                table: "ImageAssets",
                column: "JobId1",
                principalTable: "ImageGenerationJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

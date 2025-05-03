using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace firefly.Migrations
{
    /// <inheritdoc />
    public partial class newfieldType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageAsset_ImageGenerationJobs_JobId",
                table: "ImageAsset");

            migrationBuilder.DropIndex(
                name: "IX_ImageAsset_JobId",
                table: "ImageAsset");

            migrationBuilder.AddColumn<Guid>(
                name: "JobId1",
                table: "ImageAsset",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ImageAsset_JobId1",
                table: "ImageAsset",
                column: "JobId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageAsset_ImageGenerationJobs_JobId1",
                table: "ImageAsset",
                column: "JobId1",
                principalTable: "ImageGenerationJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageAsset_ImageGenerationJobs_JobId1",
                table: "ImageAsset");

            migrationBuilder.DropIndex(
                name: "IX_ImageAsset_JobId1",
                table: "ImageAsset");

            migrationBuilder.DropColumn(
                name: "JobId1",
                table: "ImageAsset");

            migrationBuilder.CreateIndex(
                name: "IX_ImageAsset_JobId",
                table: "ImageAsset",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageAsset_ImageGenerationJobs_JobId",
                table: "ImageAsset",
                column: "JobId",
                principalTable: "ImageGenerationJobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TOLED.Web.Migrations
{
    /// <inheritdoc />
    public partial class Devices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ImageCollections_ImageCollectionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_ImageCollections_ImageCollectionId",
                table: "Images");

            migrationBuilder.DropTable(
                name: "ImageCollections");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ImageCollectionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImageCollectionId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ImageCollectionId",
                table: "Images",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ImageCollectionId",
                table: "Images",
                newName: "IX_Images_OwnerId");

            migrationBuilder.AddColumn<int>(
                name: "ActiveImageId",
                table: "Devices",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Devices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PPImageId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_ActiveImageId",
                table: "Devices",
                column: "ActiveImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_OwnerId",
                table: "Devices",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PPImageId",
                table: "AspNetUsers",
                column: "PPImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Images_PPImageId",
                table: "AspNetUsers",
                column: "PPImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_AspNetUsers_OwnerId",
                table: "Devices",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Devices_Images_ActiveImageId",
                table: "Devices",
                column: "ActiveImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_AspNetUsers_OwnerId",
                table: "Images",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Images_PPImageId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_AspNetUsers_OwnerId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Devices_Images_ActiveImageId",
                table: "Devices");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_AspNetUsers_OwnerId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Devices_ActiveImageId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_Devices_OwnerId",
                table: "Devices");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PPImageId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ActiveImageId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "PPImageId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Images",
                newName: "ImageCollectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_OwnerId",
                table: "Images",
                newName: "IX_Images_ImageCollectionId");

            migrationBuilder.AddColumn<Guid>(
                name: "ImageCollectionId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ImageCollections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageCollections", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ImageCollectionId",
                table: "AspNetUsers",
                column: "ImageCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ImageCollections_ImageCollectionId",
                table: "AspNetUsers",
                column: "ImageCollectionId",
                principalTable: "ImageCollections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ImageCollections_ImageCollectionId",
                table: "Images",
                column: "ImageCollectionId",
                principalTable: "ImageCollections",
                principalColumn: "Id");
        }
    }
}

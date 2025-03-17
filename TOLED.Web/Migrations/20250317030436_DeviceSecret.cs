using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TOLED.Web.Migrations
{
    /// <inheritdoc />
    public partial class DeviceSecret : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeviceSecret",
                table: "Devices",
                type: "TEXT",
                nullable: false,
                defaultValue: Guid.NewGuid());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceSecret",
                table: "Devices");
        }
    }
}

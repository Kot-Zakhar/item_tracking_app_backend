using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItTrAp.ManagementService.Migrations
{
    /// <inheritdoc />
    public partial class DropCreatedAtColumnOfMovableInstances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "movable_instances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "movable_instances",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}

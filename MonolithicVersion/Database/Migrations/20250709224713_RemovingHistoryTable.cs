using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class RemovingHistoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movable_instance_history");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "movable_instance_history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    from_location_id = table.Column<long>(type: "bigint", nullable: true),
                    movable_instance_id = table.Column<long>(type: "bigint", nullable: false),
                    to_location_id = table.Column<long>(type: "bigint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    ended_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movable_instance_history", x => x.id);
                    table.ForeignKey(
                        name: "fk_movable_instance_history_locations_from_location_id",
                        column: x => x.from_location_id,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_movable_instance_history_locations_to_location_id",
                        column: x => x.to_location_id,
                        principalTable: "locations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_movable_instance_history_movable_instances_movable_instance",
                        column: x => x.movable_instance_id,
                        principalTable: "movable_instances",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_movable_instance_history_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_movable_instance_history_from_location_id",
                table: "movable_instance_history",
                column: "from_location_id");

            migrationBuilder.CreateIndex(
                name: "ix_movable_instance_history_movable_instance_id",
                table: "movable_instance_history",
                column: "movable_instance_id");

            migrationBuilder.CreateIndex(
                name: "ix_movable_instance_history_to_location_id",
                table: "movable_instance_history",
                column: "to_location_id");

            migrationBuilder.CreateIndex(
                name: "ix_movable_instance_history_user_id",
                table: "movable_instance_history",
                column: "user_id");
        }
    }
}

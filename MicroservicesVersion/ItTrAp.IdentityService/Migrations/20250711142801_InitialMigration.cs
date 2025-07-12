using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ItTrAp.IdentityService.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    password_hash = table.Column<byte[]>(type: "bytea", nullable: false),
                    salt = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "roles_permissions",
                columns: table => new
                {
                    permissions_id = table.Column<long>(type: "bigint", nullable: false),
                    roles_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_roles_permissions", x => new { x.permissions_id, x.roles_id });
                    table.ForeignKey(
                        name: "fk_roles_permissions_permissions_permissions_id",
                        column: x => x.permissions_id,
                        principalTable: "permissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_roles_permissions_roles_roles_id",
                        column: x => x.roles_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_sessions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    refresh_token = table.Column<Guid>(type: "uuid", nullable: false),
                    user_agent = table.Column<string>(type: "text", nullable: false),
                    fingerprint = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_sessions", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_sessions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_user_sessions_users_user_id1",
                        column: x => x.user_id1,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "users_roles",
                columns: table => new
                {
                    roles_id = table.Column<long>(type: "bigint", nullable: false),
                    users_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users_roles", x => new { x.roles_id, x.users_id });
                    table.ForeignKey(
                        name: "fk_users_roles_roles_roles_id",
                        column: x => x.roles_id,
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_users_roles_users_users_id",
                        column: x => x.users_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1L, "users:get" },
                    { 2L, "users:list" },
                    { 3L, "users:create" },
                    { 4L, "users:update" },
                    { 5L, "users:delete" },
                    { 6L, "users:reset_password" },
                    { 7L, "categories:list" },
                    { 8L, "categories:list_from_node" },
                    { 9L, "categories:create" },
                    { 10L, "categories:update" },
                    { 11L, "categories:delete" },
                    { 12L, "locations:list" },
                    { 13L, "locations:get" },
                    { 14L, "locations:create" },
                    { 15L, "locations:update" },
                    { 16L, "locations:delete" },
                    { 17L, "locations:get_qr_code" },
                    { 18L, "movable_items:list" },
                    { 19L, "movable_items:get" },
                    { 20L, "movable_items:create" },
                    { 21L, "movable_items:update" },
                    { 22L, "movable_items:delete" },
                    { 23L, "movable_instances:list" },
                    { 24L, "movable_instances:get" },
                    { 25L, "movable_instances:create" },
                    { 26L, "movable_instances:delete" },
                    { 27L, "movable_instances:book" },
                    { 28L, "movable_instances:cancel_booking" },
                    { 29L, "movable_instances:assign" },
                    { 30L, "movable_instances:take_by_code" },
                    { 31L, "movable_instances:release" },
                    { 32L, "movable_instances:move" },
                    { 33L, "movable_instances:get_qr_code" },
                    { 34L, "users:update_password" }
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1L, null, "admin" },
                    { 2L, null, "user" },
                    { 3L, null, "manager" }
                });

            migrationBuilder.InsertData(
                table: "roles_permissions",
                columns: new[] { "permissions_id", "roles_id" },
                values: new object[,]
                {
                    { 1L, 1L },
                    { 1L, 2L },
                    { 1L, 3L },
                    { 2L, 1L },
                    { 2L, 2L },
                    { 2L, 3L },
                    { 3L, 1L },
                    { 3L, 3L },
                    { 4L, 1L },
                    { 4L, 3L },
                    { 5L, 1L },
                    { 5L, 3L },
                    { 6L, 1L },
                    { 6L, 3L },
                    { 7L, 1L },
                    { 7L, 2L },
                    { 7L, 3L },
                    { 8L, 1L },
                    { 8L, 2L },
                    { 8L, 3L },
                    { 9L, 1L },
                    { 9L, 3L },
                    { 10L, 1L },
                    { 10L, 3L },
                    { 11L, 1L },
                    { 11L, 3L },
                    { 12L, 1L },
                    { 12L, 2L },
                    { 12L, 3L },
                    { 13L, 1L },
                    { 13L, 2L },
                    { 13L, 3L },
                    { 14L, 1L },
                    { 14L, 3L },
                    { 15L, 1L },
                    { 15L, 3L },
                    { 16L, 1L },
                    { 16L, 3L },
                    { 17L, 1L },
                    { 17L, 3L },
                    { 18L, 1L },
                    { 18L, 2L },
                    { 18L, 3L },
                    { 19L, 1L },
                    { 19L, 2L },
                    { 19L, 3L },
                    { 20L, 1L },
                    { 20L, 3L },
                    { 21L, 1L },
                    { 21L, 3L },
                    { 22L, 1L },
                    { 22L, 3L },
                    { 23L, 1L },
                    { 23L, 2L },
                    { 23L, 3L },
                    { 24L, 1L },
                    { 24L, 2L },
                    { 24L, 3L },
                    { 25L, 1L },
                    { 25L, 3L },
                    { 26L, 1L },
                    { 26L, 3L },
                    { 27L, 1L },
                    { 27L, 2L },
                    { 27L, 3L },
                    { 28L, 1L },
                    { 28L, 2L },
                    { 28L, 3L },
                    { 29L, 1L },
                    { 29L, 3L },
                    { 30L, 1L },
                    { 30L, 2L },
                    { 31L, 1L },
                    { 31L, 2L },
                    { 31L, 3L },
                    { 32L, 1L },
                    { 32L, 3L },
                    { 33L, 1L },
                    { 33L, 3L },
                    { 34L, 1L }
                });

            migrationBuilder.CreateIndex(
                name: "ix_permissions_name",
                table: "permissions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_roles_name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_roles_permissions_roles_id",
                table: "roles_permissions",
                column: "roles_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_sessions_user_id",
                table: "user_sessions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_sessions_user_id1",
                table: "user_sessions",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_roles_users_id",
                table: "users_roles",
                column: "users_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "roles_permissions");

            migrationBuilder.DropTable(
                name: "user_sessions");

            migrationBuilder.DropTable(
                name: "users_roles");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

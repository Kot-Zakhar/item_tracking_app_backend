using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class DefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { 6L, "users:update_password" },
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
                    { 33L, "movable_instances:get_qr_code" }
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
                    { 33L, 3L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 1L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 1L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 1L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 2L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 2L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 2L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 3L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 3L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 4L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 4L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 5L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 5L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 6L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 6L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 7L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 7L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 7L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 8L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 8L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 8L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 9L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 9L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 10L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 10L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 11L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 11L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 12L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 12L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 12L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 13L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 13L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 13L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 14L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 14L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 15L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 15L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 16L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 16L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 17L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 17L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 18L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 18L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 18L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 19L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 19L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 19L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 20L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 20L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 21L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 21L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 22L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 22L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 23L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 23L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 23L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 24L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 24L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 24L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 25L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 25L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 26L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 26L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 27L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 27L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 27L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 28L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 28L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 28L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 29L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 29L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 30L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 30L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 31L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 31L, 2L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 31L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 32L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 32L, 3L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 33L, 1L });

            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 33L, 3L });

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "id",
                keyValue: 3L);
        }
    }
}

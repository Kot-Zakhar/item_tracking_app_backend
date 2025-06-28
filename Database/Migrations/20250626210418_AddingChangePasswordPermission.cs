using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddingChangePasswordPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 6L,
                column: "name",
                value: "users:reset_password");

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "id", "name" },
                values: new object[] { 34L, "users:update_password" });

            migrationBuilder.InsertData(
                table: "roles_permissions",
                columns: new[] { "permissions_id", "roles_id" },
                values: new object[] { 34L, 1L });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles_permissions",
                keyColumns: new[] { "permissions_id", "roles_id" },
                keyValues: new object[] { 34L, 1L });

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 34L);

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "id",
                keyValue: 6L,
                column: "name",
                value: "users:update_password");
        }
    }
}

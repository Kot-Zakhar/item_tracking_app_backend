using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class DefaultRoles : Migration
    {

        private enum PredefinedRoles
        {
            Admin = 1,
            User = 2,
            Manager = 3
        }
        #region User Management
        private const string GetUser = "users:get";
        private const string GetAllUsers = "users:list";
        private const string CreateUser = "users:create";
        private const string UpdateUser = "users:update";
        private const string DeleteUser = "users:delete";
        private const string UpdateUserPassword = "users:update_password";
        #endregion

        #region Category Management
        private const string GetCategoryTree = "categories:list";
        private const string GetCategoryTreeFromNode = "categories:list_from_node";
        private const string CreateCategory = "categories:create";
        private const string UpdateCategory = "categories:update";
        private const string DeleteCategory = "categories:delete";
        #endregion

        #region Location Management
        private const string GetAllFilteredLocations = "locations:list";
        private const string GetLocationById = "locations:get";
        private const string CreateLocation = "locations:create";
        private const string UpdateLocation = "locations:update";
        private const string DeleteLocation = "locations:delete";
        private const string GetLocationQrCode = "locations:get_qr_code";
        #endregion

        #region Movable Item Management
        private const string GetAllFilteredMovableItems = "movable_items:list";
        private const string GetMovableItemById = "movable_items:get";
        private const string CreateMovableItem = "movable_items:create";
        private const string UpdateMovableItem = "movable_items:update";
        private const string DeleteMovableItem = "movable_items:delete";
        #endregion

        #region Movable Instance Management
        private const string GetAllFilteredMovableInstances = "movable_instances:list";
        private const string GetMovableInstanceById = "movable_instances:get";
        private const string CreateMovableInstance = "movable_instances:create";
        private const string DeleteMovableInstance = "movable_instances:delete";
        #endregion

        #region Movable Instance Booking
        private const string BookMovableInstance = "movable_instances:book";
        private const string CancelBookingOfMovableInstance = "movable_instances:cancel_booking";
        private const string TakeMovableInstance = "movable_instances:take";
        private const string TakeMovableByCodeInstance = "movable_instances:take_by_code";
        private const string ReleaseMovableInstance = "movable_instances:release";
        private const string MoveMovableInstance = "movable_instances:move";
        private const string GetMovableInstanceQrCode = "movable_instances:get_qr_code";
        #endregion

        private static readonly Dictionary<PredefinedRoles, List<string>> RolePermissions = new()
        {
            {
                PredefinedRoles.Manager,
                new List<string> {
                    GetUser, GetAllUsers, CreateUser, UpdateUser, DeleteUser, UpdateUserPassword,
                    GetCategoryTree, GetCategoryTreeFromNode, CreateCategory, UpdateCategory, DeleteCategory,
                    GetAllFilteredLocations, GetLocationById, CreateLocation, UpdateLocation, DeleteLocation, GetLocationQrCode,
                    GetAllFilteredMovableItems, GetMovableItemById, CreateMovableItem, UpdateMovableItem, DeleteMovableItem,
                    GetAllFilteredMovableInstances, GetMovableInstanceById, CreateMovableInstance, DeleteMovableInstance,
                    BookMovableInstance, CancelBookingOfMovableInstance, TakeMovableInstance, ReleaseMovableInstance, MoveMovableInstance, GetMovableInstanceQrCode
                }
            },
            {
                PredefinedRoles.User,
                new List<string> {
                    GetUser, GetAllUsers,
                    GetCategoryTree, GetCategoryTreeFromNode,
                    GetAllFilteredLocations, GetLocationById,
                    GetAllFilteredMovableItems, GetMovableItemById,
                    GetAllFilteredMovableInstances, GetMovableInstanceById,
                    BookMovableInstance, CancelBookingOfMovableInstance,
                    TakeMovableByCodeInstance, ReleaseMovableInstance
                }
            },
        };

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            foreach (var (role, permissions) in RolePermissions)
            {
                var roleName = Enum.GetName(role)?.ToLowerInvariant();
                if (string.IsNullOrEmpty(roleName))
                    continue;

                migrationBuilder.Sql($"INSERT INTO roles (id, name) VALUES ({(int)role}, '{roleName}') ON CONFLICT DO NOTHING;");

                foreach (var permission in permissions)
                {
                    migrationBuilder.Sql($"INSERT INTO permissions (name) VALUES ('{permission}') ON CONFLICT DO NOTHING;");
                    migrationBuilder.Sql($@"
                        INSERT INTO roles_permissions (roles_id, permissions_id)
                        SELECT {(int)role}, id FROM permissions WHERE name = '{permission}'
                        ON CONFLICT DO NOTHING;");
                }
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var (role, permissions) in RolePermissions)
            {
                var roleName = Enum.GetName(role)?.ToLowerInvariant();
                if (string.IsNullOrEmpty(roleName))
                    continue;

                migrationBuilder.Sql($"DELETE FROM roles_permissions WHERE roles_id = {(int)role};");
                migrationBuilder.Sql($"DELETE FROM roles WHERE id = {(int)role};");

                foreach (var permission in permissions)
                {
                    migrationBuilder.Sql($"DELETE FROM permissions WHERE name = '{permission}';");
                }
            }
        }
    }
}

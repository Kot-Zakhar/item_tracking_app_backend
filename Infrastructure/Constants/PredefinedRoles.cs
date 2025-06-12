namespace Infrastructure.Constants;

public enum PredefinedRoles
{
    Admin = 1,
    User = 2,
    Manager = 3
}

public static class PredefinedPermissions
{
    #region User Management
    public const string GetUser = "users:get";
    public const string GetAllUsers = "users:list";
    public const string CreateUser = "users:create";
    public const string UpdateUser = "users:update";
    public const string DeleteUser = "users:delete";
    public const string UpdateUserPassword = "users:update_password";
    #endregion

    #region Category Management
    public const string GetCategoryTree = "categories:list";
    public const string GetCategoryTreeFromNode = "categories:list_from_node";
    public const string CreateCategory = "categories:create";
    public const string UpdateCategory = "categories:update";
    public const string DeleteCategory = "categories:delete";
    #endregion

    #region Location Management
    public const string GetAllFilteredLocations = "locations:list";
    public const string GetLocationById = "locations:get";
    public const string CreateLocation = "locations:create";
    public const string UpdateLocation = "locations:update";
    public const string DeleteLocation = "locations:delete";
    public const string GetLocationQrCode = "locations:get_qr_code";
    #endregion

    #region Movable Item Management
    public const string GetAllFilteredMovableItems = "movable_items:list";
    public const string GetMovableItemById = "movable_items:get";
    public const string CreateMovableItem = "movable_items:create";
    public const string UpdateMovableItem = "movable_items:update";
    public const string DeleteMovableItem = "movable_items:delete";
    #endregion

    #region Movable Instance Management
    public const string GetAllFilteredMovableInstances = "movable_instances:list";
    public const string GetMovableInstanceById = "movable_instances:get";
    public const string CreateMovableInstance = "movable_instances:create";
    public const string DeleteMovableInstance = "movable_instances:delete";
    #endregion

    #region Movable Instance Booking
    public const string BookMovableInstance = "movable_instances:book";
    public const string CancelBookingOfMovableInstance = "movable_instances:cancel_booking";
    public const string TakeMovableInstance = "movable_instances:take";
    public const string TakeMovableByCodeInstance = "movable_instances:take_by_code";
    public const string ReleaseMovableInstance = "movable_instances:release";
    public const string MoveMovableInstance = "movable_instances:move";
    public const string GetMovableInstanceQrCode = "movable_instances:get_qr_code";
    #endregion

    public static readonly Dictionary<PredefinedRoles, List<string>> RolePermissions = new()
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
}
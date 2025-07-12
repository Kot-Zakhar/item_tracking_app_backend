namespace ItTrAp.IdentityService.Constants;

// These classes are used for seeding data with explicit IDs
public record PermissionSeed(uint Id, string Name);
public record RoleSeed(uint Id, string Name);

public static class SecurityConstants
{
    public static class Roles
    {
        public const string Admin = "admin";
        public const string User = "user";
        public const string Manager = "manager";

        // List of all predefined roles for seeding
        public static readonly IReadOnlyList<RoleSeed> PredefinedRoles = new[]
        {
            new RoleSeed(1, Admin),
            new RoleSeed(2, User),
            new RoleSeed(3, Manager)
        };
    }


    public static class Permissions
    {
        public static class Users
        {
            public const string Get = "users:get";
            public const string List = "users:list";
            public const string Create = "users:create";
            public const string Update = "users:update";
            public const string Delete = "users:delete";
            public const string UpdatePassword = "users:update_password";
            public const string ResetPassword = "users:reset_password";
        }

        public static class Categories
        {
            public const string List = "categories:list";
            public const string ListFromNode = "categories:list_from_node";
            public const string Create = "categories:create";
            public const string Update = "categories:update";
            public const string Delete = "categories:delete";
        }

        public static class Locations
        {
            public const string List = "locations:list";
            public const string Get = "locations:get";
            public const string Create = "locations:create";
            public const string Update = "locations:update";
            public const string Delete = "locations:delete";
            public const string GetQrCode = "locations:get_qr_code";
        }

        public static class MovableItems
        {
            public const string List = "movable_items:list";
            public const string Get = "movable_items:get";
            public const string Create = "movable_items:create";
            public const string Update = "movable_items:update";
            public const string Delete = "movable_items:delete";
        }

        public static class MovableInstances
        {
            public const string List = "movable_instances:list";
            public const string Get = "movable_instances:get";
            public const string Create = "movable_instances:create";
            public const string Delete = "movable_instances:delete";
            public const string Book = "movable_instances:book";
            public const string CancelBooking = "movable_instances:cancel_booking";
            public const string Assign = "movable_instances:assign";
            public const string TakeByCode = "movable_instances:take_by_code";
            public const string Release = "movable_instances:release";
            public const string Move = "movable_instances:move";
            public const string GetQrCode = "movable_instances:get_qr_code";
        }

        // List of all permissions with explicit IDs for seeding
        public static readonly IReadOnlyList<PermissionSeed> AllPermissions = new[]
        {
            // Users
            new PermissionSeed(1, Users.Get),
            new PermissionSeed(2, Users.List),
            new PermissionSeed(3, Users.Create),
            new PermissionSeed(4, Users.Update),
            new PermissionSeed(5, Users.Delete),
            new PermissionSeed(6, Users.ResetPassword),
            
            // Categories
            new PermissionSeed(7, Categories.List),
            new PermissionSeed(8, Categories.ListFromNode),
            new PermissionSeed(9, Categories.Create),
            new PermissionSeed(10, Categories.Update),
            new PermissionSeed(11, Categories.Delete),
            
            // Locations
            new PermissionSeed(12, Locations.List),
            new PermissionSeed(13, Locations.Get),
            new PermissionSeed(14, Locations.Create),
            new PermissionSeed(15, Locations.Update),
            new PermissionSeed(16, Locations.Delete),
            new PermissionSeed(17, Locations.GetQrCode),
            
            // MovableItems
            new PermissionSeed(18, MovableItems.List),
            new PermissionSeed(19, MovableItems.Get),
            new PermissionSeed(20, MovableItems.Create),
            new PermissionSeed(21, MovableItems.Update),
            new PermissionSeed(22, MovableItems.Delete),
            
            // MovableInstances
            new PermissionSeed(23, MovableInstances.List),
            new PermissionSeed(24, MovableInstances.Get),
            new PermissionSeed(25, MovableInstances.Create),
            new PermissionSeed(26, MovableInstances.Delete),
            new PermissionSeed(27, MovableInstances.Book),
            new PermissionSeed(28, MovableInstances.CancelBooking),
            new PermissionSeed(29, MovableInstances.Assign),
            new PermissionSeed(30, MovableInstances.TakeByCode),
            new PermissionSeed(31, MovableInstances.Release),
            new PermissionSeed(32, MovableInstances.Move),
            new PermissionSeed(33, MovableInstances.GetQrCode),

            // User Self Service
            new PermissionSeed(34, Users.UpdatePassword)
        };
    }


    // Role-Permission mappings
    public static readonly IDictionary<string, IReadOnlyList<string>> RolePermissions = new Dictionary<string, IReadOnlyList<string>>
    {
        {
            Roles.Admin,
            Permissions.AllPermissions.Select(p => p.Name).ToList()
        },
        {
            Roles.Manager,
            new List<string> {
                Permissions.Users.Get, Permissions.Users.List, Permissions.Users.Create, 
                Permissions.Users.Update, Permissions.Users.Delete, Permissions.Users.ResetPassword,
                
                Permissions.Categories.List, Permissions.Categories.ListFromNode, 
                Permissions.Categories.Create, Permissions.Categories.Update, Permissions.Categories.Delete,
                
                Permissions.Locations.List, Permissions.Locations.Get, Permissions.Locations.Create,
                Permissions.Locations.Update, Permissions.Locations.Delete, Permissions.Locations.GetQrCode,
                
                Permissions.MovableItems.List, Permissions.MovableItems.Get, Permissions.MovableItems.Create,
                Permissions.MovableItems.Update, Permissions.MovableItems.Delete,
                
                Permissions.MovableInstances.List, Permissions.MovableInstances.Get, 
                Permissions.MovableInstances.Create, Permissions.MovableInstances.Delete,
                Permissions.MovableInstances.Book, Permissions.MovableInstances.CancelBooking, 
                Permissions.MovableInstances.Assign, Permissions.MovableInstances.Release,
                Permissions.MovableInstances.Move, Permissions.MovableInstances.GetQrCode
            }
        },
        {
            Roles.User,
            new List<string> {
                Permissions.Users.Get, Permissions.Users.List,
                Permissions.Categories.List, Permissions.Categories.ListFromNode,
                Permissions.Locations.List, Permissions.Locations.Get,
                Permissions.MovableItems.List, Permissions.MovableItems.Get,
                Permissions.MovableInstances.List, Permissions.MovableInstances.Get,
                Permissions.MovableInstances.Book, Permissions.MovableInstances.CancelBooking,
                Permissions.MovableInstances.TakeByCode, Permissions.MovableInstances.Release
            }
        }
    };
}

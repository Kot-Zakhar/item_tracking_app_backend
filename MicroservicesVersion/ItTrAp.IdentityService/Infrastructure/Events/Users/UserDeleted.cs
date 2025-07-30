namespace ItTrAp.IdentityService.Infrastructure.Events.Users;

public record UserDeleted(uint UserId) : EventBase(nameof(UserDeleted))
{
    public UserDeleted() : this(0) { } // Default constructor for deserialization
}
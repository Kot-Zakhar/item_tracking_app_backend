namespace ItTrAp.IdentityService.Infrastructure.Events.Users;

public record UserCreated(uint UserId, string UserEmail) : EventBase(nameof(UserCreated))
{
    public UserCreated() : this(0, string.Empty) { } // Default constructor for deserialization
}
namespace ItTrAp.QueryService.Infrastructure.Events.OutboundEvents;

public record UserCreated(uint UserId) : EventBase(nameof(UserCreated))
{
    public UserCreated() : this(0) { } // Default constructor for deserialization
}
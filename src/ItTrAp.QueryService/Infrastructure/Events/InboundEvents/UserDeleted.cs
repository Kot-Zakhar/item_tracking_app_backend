namespace ItTrAp.QueryService.Infrastructure.Events.OutboundEvents;

public record UserDeleted(uint UserId) : EventBase(nameof(UserDeleted))
{
    public UserDeleted() : this(0) { } // Default constructor for deserialization
}
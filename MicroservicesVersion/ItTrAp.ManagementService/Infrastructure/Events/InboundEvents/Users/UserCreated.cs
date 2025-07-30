using ItTrAp.ManagementService.Infrastructure.Events.InboundEvents;

namespace ItTrAp.ManagementService.Infrastructure.Events.OutboundEvents;

public record UserCreated(uint UserId, string UserEmail) : EventBase(nameof(UserCreated))
{
    public UserCreated() : this(0, string.Empty) { } // Default constructor for deserialization
}
namespace ItTrAp.UserService.Domain.Events.OutboundEvents;

public record UserDeleted(uint UserId) : EventBase(nameof(UserDeleted))
{
    public UserDeleted() : this(0) { }
}
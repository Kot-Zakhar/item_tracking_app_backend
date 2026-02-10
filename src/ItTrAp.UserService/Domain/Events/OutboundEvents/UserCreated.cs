namespace ItTrAp.UserService.Domain.Events.OutboundEvents;

public record UserCreated(uint UserId, string UserEmail) : EventBase(nameof(UserCreated))
{
    public UserCreated() : this(0, string.Empty) { }
}
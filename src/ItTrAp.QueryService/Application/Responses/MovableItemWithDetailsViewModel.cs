using ItTrAp.QueryService.Domain.Enums;

namespace ItTrAp.QueryService.Application.Responses;

public class MovableItemWithDetailsViewModel : MovableItemViewModel
{
    public uint TotalAmount { get; set; }
    public Dictionary<MovableInstanceStatus, List<UserViewModel>> UsersByStatus { get; set; } = new();
}
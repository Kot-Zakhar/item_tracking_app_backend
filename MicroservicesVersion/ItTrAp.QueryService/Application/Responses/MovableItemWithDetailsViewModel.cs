using ItTrAp.QueryService.Domain.Enums;

namespace ItTrAp.QueryService.Application.Responses;

public class MovableItemWithDetailsViewModel : MovableItemViewModel
{
    public Dictionary<MovableInstanceStatus, int> AmountsByStatus { get; set; } = new();
}
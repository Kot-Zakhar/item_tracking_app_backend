using Application.Common.DTOs;
using Domain.MovableItems;

namespace Application.MovableItems.DTOs;

public class MovableItemWithAmountsByStatusDto : MovableItemDto
{
    public Dictionary<MovableInstanceStatus, int> AmountsByStatus { get; set; } = new();
}
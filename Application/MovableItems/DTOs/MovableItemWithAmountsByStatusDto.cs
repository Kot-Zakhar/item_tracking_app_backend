using Application.Common.DTOs;
using Domain.Enums;

namespace Application.MovableItems.DTOs;

public class MovableItemWithAmountsByStatusDto : MovableItemDto
{
    public Dictionary<MovableInstanceStatus, int> AmountsByStatus { get; set; } = new();
}
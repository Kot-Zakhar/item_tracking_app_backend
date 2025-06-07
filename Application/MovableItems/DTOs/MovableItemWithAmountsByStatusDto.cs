using Application.Common.DTOs;
using Domain.Models;

namespace Application.MovableItems.DTOs;

public class MovableItemWithAmountsByStatusDto : MovableItemDto
{
    public Dictionary<MovableInstanceStatus, int> AmountsByStatus { get; set; } = new();
}
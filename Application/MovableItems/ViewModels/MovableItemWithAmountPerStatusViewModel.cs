using Application.Common.ViewModels;
using Domain.MovableItems;

namespace Application.MovableItems.ViewModels;

public class MovableItemWithAmountsByStatusViewModel : MovableItemViewModel
{
    public Dictionary<MovableInstanceStatus, int> AmountsByStatus { get; set; } = new();
}
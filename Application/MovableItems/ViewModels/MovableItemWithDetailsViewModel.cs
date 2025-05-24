using Application.Common.ViewModels;

namespace Application.MovableItems.ViewModels;

public class MovableItemWithDetailsViewModel : MovableItemViewModel
{
    public int TotalAmount { get; set; }
    public List<UserViewModel> BookedBy { get; set; } = new();
    public List<UserViewModel> TakenBy { get; set; } = new();
}
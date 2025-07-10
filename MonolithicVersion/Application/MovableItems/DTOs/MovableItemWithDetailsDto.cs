using Application.Common.DTOs;

namespace Application.MovableItems.DTOs;

public class MovableItemWithDetailsDto : MovableItemDto
{
    public int TotalAmount { get; set; }
    public List<UserDto> BookedBy { get; set; } = new();
    public List<UserDto> TakenBy { get; set; } = new();
}
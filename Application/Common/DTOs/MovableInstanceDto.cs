using Domain.MovableItems;

namespace Application.Common.DTOs;

public class MovableInstanceDto
{
    public uint Id { get; set; }
    public uint MovableItemId { get; set; }
    public Guid Code { get; set; }
    public MovableInstanceStatus Status { get; set; }
    public LocationDto? Location { get; set; }
    public UserDto? User { get; set; }
    public DateTime CreatedAt { get; set; }
}
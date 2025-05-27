using Domain.Locations;
using Domain.MovableItems;

namespace Application.Common.ViewModels;

public class MovableInstanceViewModel
{
    public uint Id { get; set; }
    public uint MovableItemId { get; set; }
    public Guid Code { get; set; }
    public MovableInstanceStatus Status { get; set; }
    public LocationViewModel? Location { get; set; }
    public UserViewModel? User { get; set; }
    public DateTime CreatedAt { get; set; }
}
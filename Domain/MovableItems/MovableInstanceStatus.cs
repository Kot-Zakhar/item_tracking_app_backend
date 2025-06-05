namespace Domain.MovableItems;

public enum MovableInstanceStatus
{
    Unavailable = 0, // TODO: new default value, previously was Available
    Available = 1,
    Booked = 2,
    Taken = 3,
}
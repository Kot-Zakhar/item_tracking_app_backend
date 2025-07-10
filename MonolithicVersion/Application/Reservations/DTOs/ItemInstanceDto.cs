using Application.Common.DTOs;

namespace Application.Reservations.DTOs;

public record ItemInstanceDto
{
    public required MovableItemDto Item { get; init; }
    public required MovableInstanceDto Instance { get; init; }
}
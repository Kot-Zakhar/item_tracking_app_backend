using ItTrAp.ManagementService.Domain.Enums;

namespace ItTrAp.ManagementService.Application.DTOs.Reservations;

public class UserStatusDto
{
    public uint UserId { get; set; }
    public MovableInstanceStatus Status { get; set; }
}
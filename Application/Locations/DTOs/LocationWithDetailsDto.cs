using Application.Common.DTOs;

namespace Application.Locations.DTOs;

public class LocationWithDetailsDto : LocationDto
{
    public int ItemsAmount { get; set; }
}
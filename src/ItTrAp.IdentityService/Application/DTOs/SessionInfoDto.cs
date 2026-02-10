namespace ItTrAp.IdentityService.Application.DTOs;

public record SessionInfoDto(string AccessToken, string RefreshToken, DateTime ExpiresAt);

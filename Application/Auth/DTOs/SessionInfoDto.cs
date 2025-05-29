namespace Application.Auth.DTOs;

public record SessionInfoDto(string AccessToken, string RefreshToken, DateTime ExpiresAt);

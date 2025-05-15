namespace Application.Auth.Dtos;

public record SessionInfoDto(string AccessToken, string RefreshToken, DateTime ExpiresAt);

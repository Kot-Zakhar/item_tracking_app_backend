namespace ItTrAp.IdentityService.Models;

public class UserSession
{
    public static readonly TimeSpan RefreshTokenAge = TimeSpan.FromDays(60);
    public uint Id { get; set; }
    public Guid RefreshToken { get; set; }
    public required string UserAgent { get; set; }
    public required string Fingerprint { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

    public virtual required User User { get; set; }

    public static UserSession Create(User user, string fingerprint, string userAgent)
    {
        return new UserSession
        {
            User = user,
            Fingerprint = fingerprint,
            UserAgent = userAgent,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.Add(RefreshTokenAge),
            RefreshToken = Guid.NewGuid()
        };
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > ExpiresAt;
    }
}

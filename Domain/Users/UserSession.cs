namespace Domain.Users;

public class UserSession
{
    public uint Id { get; set; }
    public Guid RefreshToken { get; set; }
    public required string UserAgent { get; set; }
    public required string Fingerprint { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

    public virtual required User User { get; set; }
}

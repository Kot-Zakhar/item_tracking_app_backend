namespace ItTrAp.IdentityService.Models;

public class User
{
    public uint Id { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual List<UserSession> Sessions { get; set; } = new();
    public virtual List<Role> Roles { get; set; } = new();

    private byte[]? _passwordHash;
    private byte[]? _salt;

    public static User Create(
        string email,
        byte[] passwordHash,
        byte[] salt)
    {
        var u = new User
        {
            Email = email,
            CreatedAt = DateTime.UtcNow,
        };

        u.SetAuthenticationData(passwordHash, salt);

        return u;
    }

    public void SetAuthenticationData(byte[] passwordHash, byte[] salt)
    {
        _passwordHash = passwordHash;
        _salt = salt;
    }

    public (byte[] Hash, byte[] Salt) GetAuthenticationData()
    {
        if (_passwordHash == null || _salt == null)
        {
            throw new InvalidOperationException("Password data is not set.");
        }
        return (_passwordHash, _salt);
    }
}
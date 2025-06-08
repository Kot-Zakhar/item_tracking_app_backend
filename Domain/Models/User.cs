namespace Domain.Models;

public class User
{
    public static readonly int MaxMovableInstances = 10;

    public uint Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual List<UserSession> Sessions { get; set; } = new();
    public virtual List<MovableInstance> MovableInstances { get; set; } = new();
    public virtual List<MovableInstanceHistory> HistoryOfReservations { get; set; } = new();
    public virtual List<Role> Roles { get; set; } = new();

    private byte[]? _passwordHash;
    private byte[]? _salt;

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string phone,
        byte[] passwordHash,
        byte[] salt)
    {
        var u = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
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

    public void UpdateDetails(string? firstName, string? lastName, string? phone)
    {
        if (firstName != null) FirstName = firstName;
        if (lastName != null) LastName = lastName;
        if (phone != null) Phone = phone;
    }

    public bool IsMaxMovableInstancesReached() => MovableInstances.Count == MaxMovableInstances;
}

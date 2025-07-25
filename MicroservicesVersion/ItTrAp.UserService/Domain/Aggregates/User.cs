namespace ItTrAp.UserService.Domain.Aggregates;

public class User
{
    public uint Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Avatar { get; set; }
    public DateTime CreatedAt { get; set; }

    public static User Create(
        string firstName,
        string lastName,
        string email,
        string phone,
        string avatar)
    {
        return new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone,
            Avatar = avatar,
            CreatedAt = DateTime.UtcNow,
        };
    }
    public void Update(
        string firstName,
        string lastName,
        string phone,
        string avatar)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Avatar = avatar;
    }
}

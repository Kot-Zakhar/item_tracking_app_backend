namespace DataFactories;

using Bogus;
using Domain.Aggregates.Users;

public class UserDataFactory
{
    readonly List<User> _users = new List<User>();
    readonly Faker<User> _faker = new Faker<User>()
        .RuleFor(u => u.FirstName, f => f.Name.FirstName())
        .RuleFor(u => u.LastName, f => f.Name.LastName())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
        .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
        .RuleFor(u => u.CreatedAt, f => f.Date.Past(1))
        .RuleFor(u => u.Id, f => (uint)f.IndexFaker);

    public User CreateUser()
    {
        var user = _faker.Generate();
        _users.Add(user);
        return user;
    }

    public User GetRandomUser()
    {
        var newUser = new Random().Next(2);
        if (_users.Count == 0 || newUser == 0)
        {
            return CreateUser();
        }
        return _users[new Random().Next(_users.Count)];
    }
}

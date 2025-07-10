namespace DataFactories;

using Bogus;
using Domain.Aggregates.Locations;

public class LocationDataFactory
{
    readonly Faker<Location> _faker = new Faker<Location>()
        .RuleFor(l => l.Id, f => (uint)f.IndexFaker)
        .RuleFor(l => l.Code, f => Guid.NewGuid())
        .RuleFor(l => l.Floor, f => f.Random.SByte(-2, 10))
        .RuleFor(l => l.Name, f => f.Lorem.Word() + " " + f.Random.Int(1, 100))
        .RuleFor(l => l.Department, f => f.Random.Bool() ? f.Lorem.Word() : null)
        .RuleFor(l => l.CreatedAt, f => f.Date.Past(f.Random.Int(1, 5)));

    public Location CreateLocation()
    {
        return _faker.Generate();
    }
}
namespace DataFactories;

using Bogus;
using Domain.Aggregates.Locations;
using Domain.Aggregates.MovableInstances;
using Domain.Aggregates.MovableItems;
using Domain.Enums;

public class MovableInstanceFactory
{
    readonly List<MovableInstance> _instances = new List<MovableInstance>();

    readonly Faker<MovableInstance> _faker = new Faker<MovableInstance>()
        .RuleFor(m => m.Id, f => (uint)f.IndexFaker)
        .RuleFor(m => m.Code, f => Guid.NewGuid())
        .RuleFor(m => m.Status, f => MovableInstanceStatus.Available)
        .RuleFor(m => m.CreatedAt, f => f.Date.Past(1));

    public MovableInstance CreateMovableInstance(MovableItem movableItem)
    {
        var instance = _faker.Generate();
        instance.MovableItem = movableItem;
        movableItem.Instances.Add(instance);
        return instance;
    }

    public MovableInstance GetRandomMovableInstance()
    {
        var newInstance = new Random().Next(2);
        if (_instances.Count == 0 || newInstance == 0)
            return CreateMovableInstance(new MovableItemFactory().GetRandomMovableItem());

        return _instances[new Random().Next(_instances.Count)];
    }

    public List<MovableInstance> GetMovableInstancesOfItem(MovableItem item, int count, Location? location = default)
    {
        var list = Enumerable.Range(0, count)
            .Select(_ => CreateMovableInstance(item))
            .ToList();

        list.ForEach(instance =>
        {
            _instances.Add(instance);
            item.Instances.Add(instance);
            if (location != null)
            {
                instance.Location = location;
                location.Instances.Add(instance);
            }   
        });

        return list;   
    }
}
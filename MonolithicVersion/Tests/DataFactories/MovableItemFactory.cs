namespace DataFactories;

using Bogus;
using Domain.Aggregates.Categories;
using Domain.Aggregates.MovableItems;

public  class MovableItemFactory
{
     readonly List<MovableItem> _movableItems = new List<MovableItem>();

     readonly Faker<MovableItem> _faker = new Faker<MovableItem>()
        .RuleFor(m => m.Id, f => (uint)f.IndexFaker)
        .RuleFor(m => m.Name, f => f.Lorem.Word() + " " + f.Random.Int(1, 100))
        .RuleFor(m => m.Description, f => f.Lorem.Sentence())
        .RuleFor(m => m.CreatedAt, f => f.Date.Past(f.Random.Int(1, 5)));

    public  MovableItem CreateMovableItem(Category category)
    {
        var item = _faker.Generate();
        item.Category = category;
        _movableItems.Add(item);
        return item;
    }

    public  MovableItem GetRandomMovableItem()
    {
        var newMovableItem = new Random().Next(2);
        if (_movableItems.Count == 0 || newMovableItem == 0)
            return CreateMovableItem(new CategoryDataFactory().GetRandomCategory());

        return _movableItems[new Random().Next(_movableItems.Count)];
    }
}
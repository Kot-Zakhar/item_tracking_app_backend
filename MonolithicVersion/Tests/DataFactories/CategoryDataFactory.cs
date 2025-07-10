namespace DataFactories;

using Bogus;
using Domain.Aggregates.Categories;

public class CategoryDataFactory
{
    readonly List<Category> _categories = new List<Category>();

    readonly Faker<Category> _faker = new Faker<Category>()
        .RuleFor(c => c.Id, f => (uint)f.IndexFaker)
        .RuleFor(c => c.Name, f => f.Lorem.Word() + " " + f.Random.Int(1, 100))
        .RuleFor(c => c.Icon, f => f.Random.Bool() ? f.Image.PicsumUrl() : null);

    public Category CreateCategory()
    {
        var category = _faker.Generate();
        if (category.Icon != null && _categories.Any())
        {
            var existingCategory = _categories[new Random().Next(_categories.Count)];
            category.Parent = existingCategory;
            existingCategory.Children.Add(category);
        }
        _categories.Add(category);
        return category;
    }

    public Category GetRandomCategory()
    {
        var newCategory = new Random().Next(2);
        if (_categories.Count == 0 || newCategory == 0)
            return CreateCategory();
        return _categories[new Random().Next(_categories.Count)];
    }
}
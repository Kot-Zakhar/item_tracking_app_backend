using Domain.Aggregates.Categories;

namespace Domain.Interfaces;

public interface ICategoryUniquenessChecker : INameUniquenessChecker<Category>;
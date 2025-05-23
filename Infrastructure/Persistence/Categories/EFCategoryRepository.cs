using Domain.Categories;
using Infrastructure.Interfaces.Categories;
using Infrastructure.Persistence.Common;

namespace Infrastructure.Persistence.Categories;

public class EFCategoryRepository(AppDbContext dbContext) : EFRepository<Category>(dbContext), ICategoryRepository
{ }

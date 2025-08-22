using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;
using ItTrAp.InventoryService.Domain.Aggregates;

namespace ItTrAp.InventoryService.Infrastructure.Persistence.Repositories;

public class EFCategoryRepository(AppDbContext dbContext) : EFRepository<Category, uint>(dbContext), ICategoryRepository;
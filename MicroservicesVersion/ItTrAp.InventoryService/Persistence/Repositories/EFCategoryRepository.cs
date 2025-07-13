using ItTrAp.InventoryService.Interfaces.Persistence.Repositories;
using ItTrAp.InventoryService.Models;

namespace ItTrAp.InventoryService.Persistence.Repositories;

public class EFCategoryRepository(AppDbContext dbContext) : EFRepository<Category, uint>(dbContext), ICategoryRepository;
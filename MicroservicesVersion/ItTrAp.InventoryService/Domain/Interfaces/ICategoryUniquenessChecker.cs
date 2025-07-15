using ItTrAp.InventoryService.Domain.Models;

namespace ItTrAp.InventoryService.Domain.Interfaces;

public interface ICategoryUniquenessChecker : INameUniquenessChecker<Category, uint>;
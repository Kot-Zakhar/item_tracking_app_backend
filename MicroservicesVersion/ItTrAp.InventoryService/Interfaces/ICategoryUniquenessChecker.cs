using ItTrAp.InventoryService.Models;

namespace ItTrAp.InventoryService.Interfaces;

public interface ICategoryUniquenessChecker : INameUniquenessChecker<Category, uint>;
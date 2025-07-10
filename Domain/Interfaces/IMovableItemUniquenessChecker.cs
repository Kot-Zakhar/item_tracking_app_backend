using Domain.Aggregates.MovableItems;

namespace Domain.Interfaces;

public interface IMovableItemUniquenessChecker : INameUniquenessChecker<MovableItem>;
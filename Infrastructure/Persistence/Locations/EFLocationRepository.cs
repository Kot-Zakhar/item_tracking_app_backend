using Domain.Locations;
using Infrastructure.Interfaces.Locations;
using Infrastructure.Persistence.Common;

namespace Infrastructure.Persistence.Locations;

public class EFLocationRepository(AppDbContext dbContext) : EFRepository<Location>(dbContext), ILocationRepository;

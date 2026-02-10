using Microsoft.EntityFrameworkCore;
using ItTrAp.LocationService.Domain.Aggregates;
using ItTrAp.LocationService.Infrastructure.Persistence.Interfaces.Repositories;

namespace ItTrAp.LocationService.Infrastructure.Persistence.Repositories;

public class EFLocationRepository(AppDbContext context) : EFRepository<Location, uint>(context), ILocationRepository;
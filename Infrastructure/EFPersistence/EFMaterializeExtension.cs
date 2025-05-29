using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFPersistence;

public static class EFMaterializeExtension
{
    public static async Task<List<TEntity>> MaterializeAsync<TEntity>(this IQueryable<TEntity> query, CancellationToken ct = default)
    {
        return await query.ToListAsync(ct);
    }
}
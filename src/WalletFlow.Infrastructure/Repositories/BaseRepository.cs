using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WalletFlow.Application.Repositories;
using WalletFlow.Domain.Entities;

namespace WalletFlow.Infrastructure.Repositories;

public abstract class BaseRepository<TEntity>(DbContext context) : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public virtual async Task<TEntity?> GetAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual Task<List<TEntity>> FindByFullTextAsync(Guid userId, string? search, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public virtual Task<(List<TEntity> items, int total)> FindByFullTextWithTotalItemsAsync(Guid userId, string? search, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        DbSet.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateManyAsync(IEnumerable<TEntity> entities)
    {
        context.UpdateRange(entities);
        await context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await GetAsync(id);
        if (entity != null)
        {
            DbSet.Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await FindPredicate(predicate).ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, int page,
        int pageSize)
    {
        return await FindPredicate(predicate)
            .Skip(pageSize * page)
            .Take(pageSize)
            .ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> FindAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await FindPredicate(predicate).AsNoTracking().ToListAsync();
    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await FindPredicate(predicate)
            .FirstOrDefaultAsync();
    }

    public virtual async Task<bool> HasAny(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.Where(predicate).AnyAsync();
    }

    public virtual async Task<int> DeleteFromQueryAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.Where(predicate).ExecuteDeleteAsync();
    }

    public virtual async Task CreateManyAsync(IEnumerable<TEntity> entities)
    {
        if (!entities.Any())
            return;

        await DbSet.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet.Where(predicate);
    }

    public virtual IQueryable<TResult> SelectProperties<TResult>(Expression<Func<TEntity, TResult>> selector)
    {
        return DbSet.Select(selector);
    }

    private IQueryable<TEntity> FindPredicate(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet.Where(predicate);
    }

    public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
        await context.SaveChangesAsync();
    }

    public virtual Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet.CountAsync(predicate);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        await context.SaveChangesAsync();
    }
}
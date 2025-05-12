using System.Linq.Expressions;

namespace WalletFlow.Application.Repositories;

public interface IBaseRepository<TBaseEntity>
{
    Task DeleteAsync(Guid id);
    Task DeleteAsync(TBaseEntity entity);
    Task DeleteManyAsync(IEnumerable<TBaseEntity> entities);
    Task<TBaseEntity?> GetAsync(Guid id);
    Task<IEnumerable<TBaseEntity>> GetAllAsync();
    Task<TBaseEntity> CreateAsync(TBaseEntity entity);
    Task CreateManyAsync(IEnumerable<TBaseEntity> entities);
    Task<TBaseEntity> UpdateAsync(TBaseEntity entity);
    Task UpdateManyAsync(IEnumerable<TBaseEntity> entities);
    Task<bool> HasAny(Expression<Func<TBaseEntity, bool>> predicate);
    Task<int> GetCountAsync(Expression<Func<TBaseEntity, bool>> predicate);
    Task<TBaseEntity?> GetAsync(Expression<Func<TBaseEntity, bool>> predicate);
    Task<int> DeleteFromQueryAsync(Expression<Func<TBaseEntity, bool>> predicate);
    IQueryable<TResult> SelectProperties<TResult>(Expression<Func<TBaseEntity, TResult>> selector);
    Task<IEnumerable<TBaseEntity>> FindAsync(Expression<Func<TBaseEntity, bool>> predicate);
    Task<IEnumerable<TBaseEntity>> FindAsync(Expression<Func<TBaseEntity, bool>> predicate, int page, int pageSize);
    Task<List<TBaseEntity>> FindByFullTextAsync(Guid userId, string? search, int page, int pageSize);

    Task<(List<TBaseEntity> items, int total)> FindByFullTextWithTotalItemsAsync(Guid userId, string? search, int page,
        int pageSize);

    Task<IEnumerable<TBaseEntity>> FindAsNoTrackingAsync(Expression<Func<TBaseEntity, bool>> predicate);
}
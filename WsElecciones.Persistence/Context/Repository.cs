using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using WsElecciones.Domain;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;

namespace WsElecciones.Persistence.Context;

public class Repository<T> : IRepository<T> where T : class
{
    internal readonly DbSet<T> _dbSet;
    protected readonly CuentaCorrienteContext _context;

    protected Repository(CuentaCorrienteContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public virtual IQueryable<T> Query(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
            query = query.Where(filter);

        if (orderBy != null)
            query = orderBy(query);

        return query;
    }

    public virtual async Task<IEnumerable<T>> GetByFilters(Expression<Func<T, bool>> predicate = null)
    {
        IQueryable<T> query = _dbSet;
        query = predicate != null ? query.Where(predicate) : query;

        return await query.ToListAsync();
    }

    public virtual async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        foreach (var include in includes)
            query = query.Include(include);

        return await query.FirstOrDefaultAsync(filter ?? throw new ArgumentNullException(nameof(filter)));
    }

    public virtual async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _dbSet;

        foreach (var include in includes)
            query = query.Include(include);

        if (filter != null)
            query = query.Where(filter);

        if (orderBy != null)
            query = orderBy(query);

        return await query.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> GetV2(Expression<Func<T, bool>> predicate = null,
        string includeString = null)
    {
        IQueryable<T> query = _dbSet;

        if (!string.IsNullOrWhiteSpace(includeString))
            query = includeString.Split(',')
                .ToList()
                .Aggregate(query, (current, inc) => current.Include(inc));

        if (predicate != null)
            query = query.Where(predicate);

        return await query.ToListAsync();
    }

    public virtual async Task<(int, IReadOnlyList<T>)> Get(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        string includeString = null,
        bool disableTracking = true,
        int page = 1,
        int limit = 10)
    {
        IQueryable<T> query = _dbSet;

        if (disableTracking)
            query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString))
            query = includeString.Split(',')
                .ToList()
                .Aggregate(query, (current, inc) => current.Include(inc));

        if (predicate != null)
            query = query.Where(predicate);

        if (orderBy != null)
            query = orderBy(query);

        var totalRecords = query.Count();
        var skip = (page - 1) * limit;

        query = limit != 0 ? query.Skip(skip).Take(limit) : query;

        return (totalRecords, await query.ToListAsync());
    }

    public async Task<T> GetFirst(Expression<Func<T, bool>> predicate = null,
        string includeString = null,
        bool disableTracking = true)
    {
        IQueryable<T> query = _dbSet;

        if (disableTracking)
            query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString))
            query = includeString.Split(',')
                .ToList()
                .Aggregate(query, (current, inc) => current.Include(inc));

        if (predicate != null)
            query = query.Where(predicate);

        return await query.FirstOrDefaultAsync();
    }

    public virtual async Task<T> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<int> Add(T entity, IDbTransaction transaction)
    {
        if (_context.Database.CurrentTransaction == null)
        {
            await _context.Database.UseTransactionAsync((DbTransaction)transaction);
        }

        await _dbSet.AddAsync(entity);
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<int> Add(T entity)
    {
        await _dbSet.AddAsync(entity);
        return _context.SaveChanges();
    }

    public virtual Task<int> DeleteAll()
    {
        _dbSet.ExecuteDelete();
        return _context.SaveChangesAsync();
    }

    public virtual async Task<int> AddRange(IEnumerable<T> entities, IDbTransaction transaction)
    {
        if (_context.Database.CurrentTransaction == null)
        {
            await _context.Database.UseTransactionAsync((DbTransaction)transaction);
        }

        await _dbSet.AddRangeAsync(entities);
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<int> AddRange(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<int> Update(T entity, IDbTransaction transaction)
    {
        if (_context.Database.CurrentTransaction == null)
        {
            await _context.Database.UseTransactionAsync((DbTransaction)transaction);
        }

        _dbSet.Update(entity);
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<int> Update(T entity)
    {
        _dbSet.Update(entity);
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<int> UpdateRange(IEnumerable<T> entities, IDbTransaction transaction)
    {
        if (_context.Database.CurrentTransaction == null)
        {
            await _context.Database.UseTransactionAsync((DbTransaction)transaction);
        }

        _dbSet.UpdateRange(entities);
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<int> Delete(T entity, IDbTransaction transaction)
    {
        if (_context.Database.CurrentTransaction == null)
        {
            await _context.Database.UseTransactionAsync((DbTransaction)transaction);
        }

        _dbSet.Remove(entity);
        return await _context.SaveChangesAsync();
    }

    public virtual async Task<int> DeleteRange(IEnumerable<T> entities, IDbTransaction transaction)
    {
        if (_context.Database.CurrentTransaction == null)
        {
            await _context.Database.UseTransactionAsync((DbTransaction)transaction);
        }

        _dbSet.RemoveRange(entities);
        return await _context.SaveChangesAsync();
    }

    public virtual async Task BulkInsert(List<T> entities, IDbTransaction transaction)
    {
        if (_context.Database.CurrentTransaction == null)
        {
            await _context.Database.UseTransactionAsync((DbTransaction)transaction);
        }

        await _context.BulkInsertAsync(entities);
    }

    public virtual async Task BulkInsert(List<T> entities)
    {
        await _context.BulkInsertAsync(entities);
    }

    public virtual async Task BulkUpdate(List<T> entities, IDbTransaction transaction)
    {
        if (_context.Database.CurrentTransaction == null)
        {
            await _context.Database.UseTransactionAsync((DbTransaction)transaction);
        }

        await _context.BulkUpdateAsync(entities);
    }

    public virtual async Task BulkUpdate(List<T> entities)
    {
        await _context.BulkUpdateAsync(entities);
    }

    public virtual async Task BulkDelete(List<T> entities, IDbTransaction transaction)
    {
        if (_context.Database.CurrentTransaction == null)
        {
            await _context.Database.UseTransactionAsync((DbTransaction)transaction);
        }

        await _context.BulkDeleteAsync(entities);
    }

    public virtual async Task BulkDelete(List<T> entities)
    {
        await _context.BulkDeleteAsync(entities);
    }

    public virtual async Task BulkInsertOrUpdate(List<T> entities, IDbTransaction transaction)
    {
        if (_context.Database.CurrentTransaction == null)
        {
            await _context.Database.UseTransactionAsync((DbTransaction)transaction);
        }

        await _context.BulkInsertOrUpdateAsync(entities,
            new BulkConfig { BulkCopyTimeout = 0, IncludeGraph = true, UseTempDB = true });
    }

    public virtual async Task BulkInsertOrUpdate(List<T> entities)
    {
        await _context.BulkInsertOrUpdateAsync(entities,
            new BulkConfig { BulkCopyTimeout = 0, IncludeGraph = true, UseTempDB = true });
    }
}
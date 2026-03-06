using System.Data;
using System.Linq.Expressions;

namespace WsElecciones.Domain;

public interface IRepository<T> where T : class
    {
        IQueryable<T> Query(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);

        Task<IEnumerable<T>> GetByFilters(Expression<Func<T, bool>> predicate = null);

        Task<IEnumerable<T>> GetV2(Expression<Func<T, bool>> predicate = null,
            string includeString = null);

        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter = null,
            params Expression<Func<T, object>>[] includes);

        Task<T> GetFirst(Expression<Func<T, bool>> predicate = null,
            string includeString = null,
            bool disableTracking = true);

        Task<(int, IReadOnlyList<T>)> Get(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeString = null,
            bool disableTracking = true,
            int page = 1,
            int limit = 10);

        Task<IEnumerable<T>> GetAll();
        Task<int> Add(T entity, IDbTransaction transaction);
        Task<int> Add(T entity);
        Task<int> AddRange(IEnumerable<T> entities, IDbTransaction transaction);
        Task<int> Update(T entity, IDbTransaction transaction);
        Task<int> Update(T entity);
        Task<int> UpdateRange(IEnumerable<T> entities, IDbTransaction transaction);
        Task<int> AddRange(IEnumerable<T> entities);
        Task<int> Delete(T entity, IDbTransaction transaction);
        Task<int> DeleteRange(IEnumerable<T> entities, IDbTransaction transaction);
        Task BulkInsert(List<T> entities, IDbTransaction transaction);
        Task BulkInsert(List<T> entities);
        Task BulkUpdate(List<T> entities, IDbTransaction transaction);
        Task BulkUpdate(List<T> entities);
        Task BulkDelete(List<T> entities, IDbTransaction transaction);
        Task BulkDelete(List<T> entities);
        Task BulkInsertOrUpdate(List<T> entities, IDbTransaction transaction);
        Task BulkInsertOrUpdate(List<T> entities);
    }
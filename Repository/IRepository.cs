using System.Linq.Expressions;

namespace APICatalago.Repository;

public interface IRepository<T>
{
    IQueryable<T> Get();
    Task<T> GetbyId(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}

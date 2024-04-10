using System.Linq.Expressions;

namespace Referral.Repositories;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T Get(Guid id);
    void Add(T entity);
    int Count(Expression<Func<T, bool>> filter = null);
    bool Exists(Expression<Func<T, bool>> filter);
    void Remove(T entity);
    //void Edit(T entity);
    T GetSpecial(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
}
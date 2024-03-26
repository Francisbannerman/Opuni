using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Referral.Settings;

namespace Referral.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _db;
    internal DbSet<T> _dbSet;
    public Repository(ApplicationDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }

    public T Get(Guid id)
    {
        return _dbSet.Find(id);
    }
    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }
    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }
    public int Count(Expression<Func<T, bool>> filter = null)
    {
        IQueryable<T> query = _dbSet;
        if (filter != null)
        {
            query = query.Where(filter);
        }
        return query.Count();
    }
    public bool Exists(Expression<Func<T, bool>> filter)
    {
        return _dbSet.Any(filter);
    }
    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
    public void Edit(T entity)
    {
        _dbSet.Attach(entity);
        _db.Entry(entity).State = EntityState.Modified;
    }
    public T GetSpecial(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
    {
        IQueryable<T> query;
        if (tracked)
        {
            query = _dbSet;
        }
        else
        {
            query = _dbSet.AsNoTracking();
                
        }
        query = query.Where(filter);
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.
                         Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        return query.FirstOrDefault();
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MediaStorage.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IMediaContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(IMediaContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(ICollection<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Count(predicate);
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public Task<int> CountAsync()
        {
            return _dbSet.CountAsync();
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.CountAsync(predicate);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(object id)
        {
            var entity = Find(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public void DeleteAll(Expression<Func<T, bool>> predicate)
        {
            var entities = GetAll(predicate).ToList();
            if (entities.Count > 0)
                _dbSet.RemoveRange(entities);
        }

        public void DeleteRange(ICollection<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public T Find(object key)
        {
            return _dbSet.Find(key);
        }

        public Task<T> FindAsync(object key)
        {
            return _dbSet.FindAsync(key);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public T Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return GetAll(includes).FirstOrDefault(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = _dbSet.AsQueryable();

            if (includes != null)
            {
                foreach (var item in includes)
                    query = query.Include(item);
            }

            return query;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            return GetAll(includes).Where(predicate);
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefaultAsync(predicate);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}

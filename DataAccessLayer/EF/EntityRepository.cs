using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model;

namespace DataAccessLayer.EF
{
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly DBContext _context;
        private readonly DbSet<T> _dbSet;

        public EntityRepository(DBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public EntityRepository() : this(new DBContext())
        {
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = ReadById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public T? ReadById(int id)
        {
            return _dbSet.FirstOrDefault(e => e.Id == id);
        }

        public List<T> ReadAll()
        {
            return _dbSet.ToList();
        }
    }

    public class EntityRepository : EntityRepository<Student>, IRepository
    {
        public EntityRepository(DBContext context) : base(context) { }
        public EntityRepository() : base() { }
    }
}

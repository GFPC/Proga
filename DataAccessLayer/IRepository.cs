using System.Collections.Generic;
using Model;

namespace DataAccessLayer
{
    public interface IRepository<T> where T : class, IDomainObject
    {
        void Add(T entity);
        void Delete(int id);
        void Update(T entity);
        T? ReadById(int id);
        List<T> ReadAll();
    }

    public interface IRepository : IRepository<Student>
    {
    }
}

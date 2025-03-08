using System.Linq.Expressions;
using TiendaLibros.Models;

namespace TiendaLibros.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);  

        void Update(T entity);

        void Delete(T entity);

        IEnumerable<T> GetAllWithIncludes(params Expression<Func<T, object>>[] includes);

        void Add(T entity);

        void Save();
    }
}


using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TiendaLibros.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        
        private DbSet<T> data;
        private TiendaLibrosContext _context;
        public Repository(TiendaLibrosContext context) 
        { 
            _context = context;
             data =  _context.Set<T>();
        }
        public void Delete(T entity)
        { 
            data.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return data.ToList();
        }
        public IEnumerable<T> GetAllWithIncludes(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = data; 

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();
        }
        public void Add(T entity)
        {
            data.Add(entity);
        }

        public T GetById(Guid id)
        {
            return data.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            data.Update(entity);
        }
    }
}

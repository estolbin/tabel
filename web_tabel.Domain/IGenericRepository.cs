using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace web_tabel.Domain
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T SingleOrDefault(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(object id);
    }
}

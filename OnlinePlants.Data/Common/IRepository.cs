using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MazzGlobal.Data.Common
{
    public interface IRepository<T>
    {
        int Insert(T entity);

        int Delete(object ID);

        int Delete(T entity);

        IEnumerable<T> GetAll();

        T GetById(object Id);

        int Update(T entity);

        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);

        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
    }
}

using OnlinePlants.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace MazzGlobal.Data.Common
{
    public class Repository<T> : IRepository<T> where T : class
    {
        OnlinePlantsContext context = new OnlinePlantsContext();
        public DbSet<T> dbSet;
        public Repository(OnlinePlantsContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }
        
        public int Insert(T entity)
        {
            try
            {
                dbSet.Add(entity);
                context.SaveChanges();
                return 1;
            }
            catch (DbEntityValidationException e)
            {

                Exception raise = e;
                foreach (var validationErrors in e.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                    throw raise;
                }
                return 0;
            }
        }        
        public int Delete(object ID)
        {
            try
            {
                var entity = dbSet.Find(ID);
                if (entity != null)
                {
                    dbSet.Remove(entity);
                    context.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }        
        public int Delete(T entity)
        {
            try
            {
                if (entity != null)
                {
                    dbSet.Remove(entity);
                    context.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }        
        public IEnumerable<T> GetAll()
        {
            return dbSet;
        }

        public T GetById(object Id)
        {
            return dbSet.Find(Id);
        }

        public int Update(T entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return context.SaveChanges();
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate);
        }

        public virtual IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
    }
}

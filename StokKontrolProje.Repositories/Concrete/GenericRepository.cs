using Microsoft.EntityFrameworkCore;
using StokKontrolProje.Entities.Entities;
using StokKontrolProje.Repositories.Abstract;
using StokKontrolProje.Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StokKontrolProje.Repositories.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StokKontrolContext context;

        public GenericRepository(StokKontrolContext context)
        {
            this.context = context;
        }

        public bool Activate(int id)
        {
            T item = GetByID(id);
            item.IsActive = true;
            return Update(item);
        }

        public bool Add(T item)
        {
            try
            {
                context.Set<T>().Add(item);
                return Save() > 0;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Add(List<T> items)
        {
            try
            {
                using(TransactionScope ts=new TransactionScope())
                {
                    foreach (T item in items)
                    {
                        context.Set<T>().Add(item);
                    }
                    ts.Complete();
                    return Save() > 0;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Any(System.Linq.Expressions.Expression<Func<T, bool>> exp) => context.Set<T>().Any(exp);
        

        public void DetachEntity(T item)
        {
            context.Entry<T>(item).State = EntityState.Detached;//Bir entryi takip etmeyi bırakmak için kullanılır.
        }

        public List<T> GetActive() => context.Set<T>().Where(x => x.IsActive == true).ToList();
       
        public IQueryable<T> GetActive(params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().Where(x => x.IsActive == true);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;

        }

        public List<T> GetAll() => context.Set<T>().ToList();
        public IQueryable<T> GetAll(params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            }
            return query;
        }

        public IQueryable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> exp, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().Where(exp);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public T GetByDefault(System.Linq.Expressions.Expression<Func<T, bool>> exp) => context.Set<T>().FirstOrDefault(exp);

        public T GetByID(int id) => context.Set<T>().Find(id);

        public IQueryable<T> GetByID(int id, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            var query = context.Set<T>().Where(x => x.ID == id);
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }

        public List<T> GetDefault(System.Linq.Expressions.Expression<Func<T, bool>> exp) => context.Set<T>().Where(exp).ToList();
        public bool Remove(T item)
        {
            item.IsActive = false;
            return Update(item);
        }

        public bool Remove(int id)
        {
            try
            {
                using(TransactionScope ts=new TransactionScope())
                {
                    T item = GetByID(id);
                    item.IsActive = false;
                    ts.Complete();
                    return Update(item);
                }

            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool RemoveAll(System.Linq.Expressions.Expression<Func<T, bool>> exp)
        {
            try
            {
                using(TransactionScope ts=new TransactionScope())
                {
                    var collection = GetDefault(exp);
                    int counter = 0;
                    foreach (var item in collection)
                    {
                        item.IsActive = false;
                        bool operationResult = Update(item);
                        if (operationResult) counter++;

                    }
                    if (collection.Count == counter) ts.Complete();
                    else return false;
                }
                return true;

            }
            catch (Exception)
            {

                return false;
            }
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public bool Update(T item)
        {
            try
            {
                item.ModifiedDate = DateTime.Now;
                context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}

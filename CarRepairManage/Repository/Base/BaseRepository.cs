using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EntityModels;
using System.Data.Entity;

namespace Repository
{
    public abstract class BaseRepository<T, DB> : IBaseRepository<T> where T : BaseEntity,new() where DB : DbContext, new()
    {
        #region 查询普通实现方案 （基于lambda的where查询）
        //根据条件查出第一个符合条件的 对象
        public T GetEntity(Expression<Func<T, bool>> query=null)
        {
            using (DB db = new DB())
            {
                DbSet<T> dbSet = db.Set<T>();
                IQueryable<T> queryable = dbSet.AsQueryable();
                if (query != null)
                    queryable = dbSet.Where(query);
                return queryable.FirstOrDefault();
            }
        }

        //根据条件查出第一个符合条件的 对象 [可以根据条件排序]
        public T GetEntityOrder(Expression<Func<T, bool>> query = null, Expression<Func<T, long>> order = null, bool isAsc = false)
        {
            using (DB db = new DB())
            {
                DbSet<T> dbSet = db.Set<T>();
                IQueryable<T> queryable = dbSet.AsQueryable();
                if (query != null)
                    queryable = dbSet.Where(query);
                if (order != null)
                {
                    if (isAsc)
                    {
                        queryable = queryable.OrderBy(order);
                    }
                    else
                    {
                        queryable = queryable.OrderByDescending(order);
                    }
                }
                return queryable.FirstOrDefault();
            }
        }


        public T GetEntityByID(long id)
        {
            using (DB db = new DB())
            {
                DbSet<T> dbSet = db.Set<T>();
                IQueryable<T> queryable = dbSet.AsQueryable();
                queryable = dbSet.Where(p=>p.ID==id);
                return queryable.FirstOrDefault();
            }
        }

        //根据条件查出所有数据 适合数据量 小
        public IEnumerable<T> GetEntities(Expression<Func<T, bool>> query =null, Expression<Func<T, long>> order = null, bool isAsc = false)
        {
            using (DB db = new DB())
            {
                DbSet<T> dbSet = db.Set<T>();
                IQueryable<T> queryable = dbSet.AsQueryable();
                if (query!=null)
                    queryable = dbSet.Where(query);
                if (order != null)
                {
                    if (isAsc)
                    {
                        queryable = queryable.OrderBy(order);
                    }
                    else
                    {
                        queryable = queryable.OrderByDescending(order);
                    }
                }
                return queryable.ToList();
            }
        }

        public int GetEntitiesCount(Expression<Func<T, bool>> query=null)
        {
            using (DB db = new DB())
            {
                DbSet<T> dbSet = db.Set<T>();
                IQueryable<T> queryable = dbSet.AsQueryable();
                if (query != null)
                    queryable = dbSet.Where(query);
                return queryable.Count();
            }
        }

        public IEnumerable<T> GetEntitiesForPaging(ref int total, int pageIndex = 1, int pageSize = 10, Expression<Func<T, bool>> query = null, Expression<Func<T, long>> order = null, bool isAsc = false)
        {
            using (DB db = new DB())
            {
                DbSet<T> dbSet = db.Set<T>();
                IQueryable<T> queryable = dbSet.AsQueryable();
                if (query != null)
                    queryable = dbSet.Where(query);
                total = queryable.Count();
                if (order!=null)
                {
                    if (isAsc)
                    {
                        queryable = queryable.OrderBy(order);
                    }
                    else
                    {
                        queryable = queryable.OrderByDescending(order);
                    }
                }
                queryable = queryable.Skip(pageSize*(pageIndex-1)).Take(pageSize);
                return queryable.ToList();
            }
        }

        public IEnumerable<T> GetEntitiesForPaging(ref int total, int pageIndex = 1, int pageSize = 10, Expression<Func<T, bool>> query = null, Expression<Func<T, DateTime?>> order = null, bool isAsc = false)
        {
            using (DB db = new DB())
            {
                DbSet<T> dbSet = db.Set<T>();
                IQueryable<T> queryable = dbSet.AsQueryable();
                if (query != null)
                    queryable = dbSet.Where(query);
                total = queryable.Count();
                if (order != null)
                {
                    if (isAsc)
                    {
                        queryable = queryable.OrderBy(order);
                    }
                    else
                    {
                        queryable = queryable.OrderByDescending(order);
                    }
                }
                queryable = queryable.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
                return queryable.ToList();
            }
        }

        #endregion

        #region 基础增删改

        public long Insert(T entity)
        {
            using (DB db = new DB())
            {
                DbSet<T> dbSet = db.Set<T>();
                entity.CreateTime = DateTime.Now;
                entity.UpdateTime =DateTime.Now;
                dbSet.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
        }

        public long Update(T entity)
        {
            using (DB db = new DB())
            {
                DbSet<T> dbSet = db.Set<T>();
                entity.UpdateTime = DateTime.Now;
                T dbmodel = GetEntityByID(entity.ID);
                entity.CreateTime = dbmodel.CreateTime;

                dbSet.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                return db.SaveChanges()>0? entity.ID: 0;
               
            }
        }

        public int Delete(T entity)
        {
            using (DB db = new DB())
            {
                DbSet<T> dbSet = db.Set<T>();
                //dbSet.Remove(entity);
                dbSet.Attach(entity);
                db.Entry(entity).State = EntityState.Deleted;
                return db.SaveChanges();
            }
        }

        #endregion
    }
}

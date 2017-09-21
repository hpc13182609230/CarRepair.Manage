using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModels;
using System.Data.Entity;

namespace Repository
{
    public class BaseOptionsRepository : BaseRepository<BaseOptions, CarRepairEntities>
    {
        public List<BaseOptions> GetEntitiesByParentID(long ParentID)
        {
            using (CarRepairEntities db = new CarRepairEntities())
            {
                DbSet<BaseOptions> dbSet = db.Set<BaseOptions>();
                IQueryable<BaseOptions> queryable = dbSet.AsQueryable();
                queryable = queryable.Where(p => p.ParentID == ParentID);
                List<BaseOptions> list= queryable.ToList();
                return list;
            }
        }
    }
}

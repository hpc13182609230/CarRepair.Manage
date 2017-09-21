using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModels;
using System.Data.Entity;

namespace Repository
{
    public class PartsClassifyRepository : BaseRepository<PartsClassify, CarRepairEntities>
    {
        public List<PartsClassify> GetEntitiesByParentID(long ParentID)
        {
            using (CarRepairEntities db = new CarRepairEntities())
            {
                DbSet<PartsClassify> dbSet = db.Set<PartsClassify>();
                IQueryable<PartsClassify> queryable = dbSet.AsQueryable();
                queryable = queryable.Where(p => p.OptionID == ParentID);
                List<PartsClassify> list = queryable.ToList();
                return list;
            }
        }
    }
}

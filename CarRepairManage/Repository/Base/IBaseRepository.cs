using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IBaseRepository<T> where T : new ()
    {
        #region 查询普通实现方案 （基于lambda的where查询）

        //根据条件获取第一个 entity
        T GetEntity(Expression<Func<T, bool>> query = null);

        //根据条件获取第一个 entity
        //T GetEntityByID(long id);

        //根据条件获取所有的 entity 【数据较少的时候使用】
        IEnumerable<T> GetEntities(Expression<Func<T, bool>> query = null);

        //查询 总个数【分页】
        int GetEntitiesCount(Expression<Func<T, bool>> query = null);

        //分页查询
        IEnumerable<T> GetEntitiesForPaging(ref int totalCount, int pageIndex = 1, int pageSize = 10, Expression<Func<T, bool>> query = null, Expression<Func<T, long>> order = null, bool isAsc = false);

        #endregion

        #region 基础增删改

        long Insert(T entity);

        int Update(T entity);

        int Delete(T entity);

        #endregion
    }
}

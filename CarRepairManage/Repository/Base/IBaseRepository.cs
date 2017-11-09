using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace Repository
{
    public interface IBaseRepository<T> where T : BaseModel,new ()
    {

        #region 基础增删改

        long Insert(T model);

        bool Insert(IEnumerable<T> models);

        bool Delete(T model);

        bool Delete(IEnumerable<T> models);

        bool DeleteByID(long id);

        bool Update(T model);

        bool Update(IEnumerable<T> models);

        T GetByID(long id);

        IEnumerable<T> GetAll();

        #endregion
    }
}

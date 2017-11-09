using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CarRepair;
using AutoMapperLib;
using Repository;
using ViewModels;
using HelperLib;
using DapperLib;
using DapperExtensions;

namespace Service
{
    public class PartsCompanyService
    {
        private static PartsCompanyRepository repository = new PartsCompanyRepository();

        public PartsCompany GetByID(long id)
        {
            return repository.GetByID(id);
        }

        //public List<PartsCompany> GetByIDs(List<long> ids)
        //{
        //    List<ViewModels.CarRepair.PartsCompany> models = new List<ViewModels.CarRepair.PartsCompany>();
        //    PartsCompanyRepository repository = new PartsCompanyRepository();
        //    var res = repository.GetEntities(p=>ids.Contains(p.ID));
        //    foreach (var item in res)
        //    {
        //        ViewModels.CarRepair.PartsCompany model = AutoMapperClient.MapTo<EntityModels.PartsCompany, ViewModels.CarRepair.PartsCompany>(item);
        //        models.Add(model);
        //    }          
        //    return models;
        //}

        public long SavePartsCompany(PartsCompany model)
        {
            long id = 0;
            if (model.ID==0)
            {
                id = repository.Insert(model);
            }
            else
            {
                id = repository.Update(model)? model.ID: 0;
            }
            return id;
        }

        public bool DeleteByID(long id)
        {
            return repository.DeleteByID(id);
        }

        public  List<PartsCompany> GetListByPage(string keyword, DateTime startTime, DateTime endTime,ref PageInfoModel page)
        {
            int total = 0;
            List<PartsCompany> models = new List<PartsCompany>();
            using (var coon = DapperExtensionClient.Conn)
            {
                coon.Open();
                IList<IPredicate> predList = new List<IPredicate>();
                predList.Add(Predicates.Field<PartsCompany>(p => p.Name, Operator.Like, "%"+keyword+"%"));
                predList.Add(Predicates.Field<PartsCompany>(p => p.CreateTime, Operator.Ge, startTime));
                predList.Add(Predicates.Field<PartsCompany>(p => p.CreateTime, Operator.Le, endTime));
                IPredicateGroup predGroup = Predicates.Group(GroupOperator.And, predList.ToArray());

                IList<ISort> sort = new List<ISort>();
                sort.Add(new Sort { PropertyName = "ID", Ascending = false });

                total = coon.Count<PartsCompany>(predGroup);
                page.TotalCount = total;
                models = coon.GetPage<PartsCompany>(predGroup,sort,page.PageIndex,page.PageSize).OrderByDescending(p => p.ID).ToList();

                foreach (var model in models)
                {
                    model.PicURL = ConfigureHelper.Get("ImageShowURL") + model.PicURL;
                }
                coon.Close();
            }
            return models;
        } 

        public List<PartsCompany> GetListByPage(string keyword,long partsClassifyID, DateTime startTime, DateTime endTime, ref PageInfoModel page)
        {


            int total = 0;
            List<PartsCompany> models = new List<PartsCompany>();
            using (var coon = DapperExtensionClient.Conn)
            {
                coon.Open();

                IList<IPredicate> predList1 = new List<IPredicate>();
                predList1.Add(Predicates.Field<PartsCompany>(p => p.PartsClassifyID, Operator.Eq, partsClassifyID));
                predList1.Add(Predicates.Field<PartsCompany>(p => p.CreateTime, Operator.Ge, startTime));
                predList1.Add(Predicates.Field<PartsCompany>(p => p.CreateTime, Operator.Le, endTime));
                IPredicateGroup predGroup1 = Predicates.Group(GroupOperator.And, predList1.ToArray());

                IList<IPredicate> predList2 = new List<IPredicate>();
                predList2.Add(Predicates.Field<PartsCompany>(p => p.Name, Operator.Like, "%" + keyword + "%"));
                IPredicateGroup predGroup2 = Predicates.Group(GroupOperator.Or, predList2.ToArray());

                var predGroup = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };
                predGroup.Predicates.Add(predGroup1);
                predGroup.Predicates.Add(predGroup2);

                IList<ISort> sort = new List<ISort>();
                sort.Add(new Sort { PropertyName = "ID", Ascending = false });

                total = coon.Count<PartsCompany>(predGroup);
                page.TotalCount = total;
                models = coon.GetPage<PartsCompany>(predGroup, sort, page.PageIndex, page.PageSize).OrderByDescending(p => p.ID).ToList();

                foreach (var model in models)
                {
                    model.PicURL = ConfigureHelper.Get("ImageShowURL") + model.PicURL;
                }
                coon.Close();
            }
            return model;
        }

    }
}

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityModels;

using ViewModels.CarRepair;


namespace AutoMapperLib
{
    public static  class AutoMapperClient
    {

        private static MapperConfiguration config { get { return Init(); } }

        private static object _locker = new object();
        private static MapperConfiguration _config { get; set; }

        private static MapperConfiguration Init()
        {
            if (_config == null)
            {
                lock (_locker)
                {
                    if (_config == null)
                    {
                        _config = InitConfigures();
                    }
                }
            }
            return _config;
        }

        private static MapperConfiguration InitConfigures()
        {
            var config = new MapperConfiguration(cfg => {
                //注册 需要转换的对象
                cfg.CreateMap<PartsCompany, PartsCompanyModel>();
                cfg.CreateMap<BaseOptions, BaseOptionsModel>();

            }); 
            return config;
        }

        //单体 对象映射
        public static TDestination MapTo<TSource, TDestination>(TSource obj)
        {
            if (obj == null) return default(TDestination);
            //初始化的时候存在顺序 问题 TSource => TDestination
            Mapper.Initialize(cfg => { cfg.CreateMap(typeof(TSource), typeof(TDestination)); });
            return  Mapper.Map<TDestination>(obj);
        }

        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            Mapper.Initialize(cfg => { cfg.CreateMap(typeof(TSource), typeof(TDestination)); });
            return Mapper.Map<List<TDestination>>(source);
        }
    }
}

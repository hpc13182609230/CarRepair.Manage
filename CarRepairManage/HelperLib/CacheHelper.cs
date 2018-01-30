using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Collections;
using System.Web.Caching;

namespace HelperLib
{
    public class CacheHelper
    {
        /// 获取数据缓存
        /// </summary>  
        /// <param name="cacheKey">键</param>  
        public static T GetCache<T>(string cacheKey)
        {
            var objCache = HttpRuntime.Cache.Get(cacheKey);
            return objCache == null? default(T): TransformHelper.DeserializeObject<T>(objCache.ToString());

        }
        /// <summary>  
        /// 设置数据缓存  
        /// </summary>  
        public static void SetCache<T>(string cacheKey, T objObject)
        {
            HttpRuntime.Cache.Insert(cacheKey, TransformHelper.SerializeObject(objObject));
        }
        /// <summary>  
        /// 设置数据缓存  
        /// </summary>  
        public static void SetCache<T>(string cacheKey, T objObject, int timeout = 7200)
        {
            try
            {
                if (objObject == null) return;
                //相对过期  
                //objCache.Insert(cacheKey, objObject, null, DateTime.MaxValue, timeout, CacheItemPriority.NotRemovable, null);  
                //绝对过期时间  
                HttpRuntime.Cache.Insert(cacheKey, TransformHelper.SerializeObject(objObject), null, DateTime.Now.AddSeconds(timeout), TimeSpan.Zero, CacheItemPriority.High, null);
            }
            catch (Exception ex)
            {
                //throw;  
            }
        }
        /// <summary>  
        /// 移除指定数据缓存  
        /// </summary>  
        public static void RemoveAllCache(string cacheKey)
        {
            var cache = HttpRuntime.Cache;
            cache.Remove(cacheKey);
        }
        /// <summary>  
        /// 移除全部缓存  
        /// </summary>  
        public static void RemoveAllCache()
        {
            var cache = HttpRuntime.Cache;
            var cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                cache.Remove(cacheEnum.Key.ToString());
            }
        }
    }
}

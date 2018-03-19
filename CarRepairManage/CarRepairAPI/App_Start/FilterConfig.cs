using System.Web;
using System.Web.Mvc;

namespace CarRepairAPI
{
    //
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //在FilterConfig 这个类里面，只是对MVC配置起效。所以此处注入 ExceptionFilter 自定义的Filter会会报错
            //我们加过滤器的代码要加入到webapi的配置而非mvc的配置 [具体的位置在 App_Start/WebApiConfig]
            //filters.Add(new ExceptionFilter());
        }
    }
}

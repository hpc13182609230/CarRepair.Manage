using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;

namespace TestConsole
{
    class Program
    {
       public static  WechatUserService service = new WechatUserService();
        static void Main(string[] args)
        {   
            var model =  service.GetByID(1);
        }
    }
}

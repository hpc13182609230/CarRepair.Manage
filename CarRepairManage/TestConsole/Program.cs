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
        static void Main(string[] args)
        {
            WechatUserService service = new WechatUserService();
            var model =  service.GetByID(1);
        }
    }
}

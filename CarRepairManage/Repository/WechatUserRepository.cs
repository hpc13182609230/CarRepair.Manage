using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CarRepair;
using DapperLib;
using DapperExtensions;

namespace Repository
{
    public class WechatUserRepository : BaseRepository<WechatUser>
    {
        public WechatUser GetByLoginToken(string LoginToken)
        {
            WechatUser model = new WechatUser();
            using (var coon =DapperExtensionClient.Conn)
            {
                coon.Open();
                IList<IPredicate> predList = new List<IPredicate>();
                predList.Add(Predicates.Field<WechatUser>(p => p.LoginToken, Operator.Eq, LoginToken));
                model = coon.GetList<WechatUser>(predList).FirstOrDefault();
                coon.Close();
                return model;
            }
        }

        public WechatUser GetByOpenid(string LoginToken)
        {
            WechatUser model = new WechatUser();
            using (var coon = DapperExtensionClient.Conn)
            {
                coon.Open();
                IList<IPredicate> predList = new List<IPredicate>();
                predList.Add(Predicates.Field<WechatUser>(p => p.Openid, Operator.Eq, LoginToken));
                model = coon.GetList<WechatUser>(predList).FirstOrDefault();
                coon.Close();
                return model;
            }
        }

    }
}

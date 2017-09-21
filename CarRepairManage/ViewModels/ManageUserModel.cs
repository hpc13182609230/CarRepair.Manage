using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class ManageUserModel
    {
        public long id { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public System.DateTime CreateTime { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public Nullable<System.DateTime> dBeginTime { get; set; }
        public Nullable<System.DateTime> dEndTime { get; set; }
        public string UserName { get; set; }
        public string vcStatus { get; set; }
        public string vcSalt { get; set; }
        public Nullable<System.DateTime> dLastLoginTime { get; set; }
        public int iUserType { get; set; }
        public string vcUserInfo { get; set; }
        public string vcMids { get; set; }
        public string vcAppids { get; set; }
    }
}

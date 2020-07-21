using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int.BaseInfo;

namespace Yongrong.Model.Int.Org
{
    public class UserPermissionAddReq : BaseInfoReq
    {

        public List<PagePermission> UserPermissionLsit { get; set; }

        public Userinfo Userinfo { get; set; }


        //public string Op { get; set; }


        public override string Check()
        {
            return "0";
        }
    }

}

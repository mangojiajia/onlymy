using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseOrganizationRsp: BasePageRsp
    {
        public List<BaseOrganization> List { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Sys
{
   public class SysConfigGetRsp : BaseRsp
    {
        public List<SysConfig> Data { get; set; }
    }
}

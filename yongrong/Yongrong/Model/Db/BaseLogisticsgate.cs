using System;
using System.Collections.Generic;

namespace Yongrong.Model.Db
{
    public partial class BaseLogisticsgate
    {
        public decimal Id { get; set; }
        public string Gateid { get; set; }
        public string Gatename { get; set; }
        public string Createtime { get; set; }

        public void Upd(BaseLogisticsgate old)
        {
            old.Gateid = this.Gateid;
            old.Gatename = this.Gatename;
        }
    }
}

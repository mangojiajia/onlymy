using System;
using System.Collections.Generic;

namespace Yongrong.Model.Db
{
    public partial class BaseLoadingplace
    {
        public decimal Id { get; set; }
        public string Placeid { get; set; }
        public string Placename { get; set; }
        public string Createtime { get; set; }

        public void Upd(BaseLoadingplace old)
        {
            old.Placeid = this.Placeid;
            old.Placename = this.Placename;
        }
    }
}

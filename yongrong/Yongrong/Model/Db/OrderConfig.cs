using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Yongrong.Model.Db
{
    public partial class OrderConfig
    {
        public decimal Id { get; set; }
        public string Ordertime { get; set; }
        public string Goodsname { get; set; }
        public string Typelimit { get; set; }
        public decimal? Maxcarnumber { get; set; }
        public string Cardtime { get; set; }
        public string Firstweightime { get; set; }
        public string Secondweightime { get; set; }
        public string Worktime { get; set; }
        public string Outtime { get; set; }
        public string Createtime { get; set; }
        public string Configtype { get; set; }
        public string Loadarea { get; set; }
        public string Unloadarea { get; set; }

        /// <summary>
        /// 0501预约后台管理数量限制总和
        /// </summary>
        [NotMapped]
        public string Sumcarnumber { get; set; }

        public void Upd(OrderConfig old)
        {
            old.Ordertime = this.Ordertime;
            old.Goodsname = this.Goodsname;
            old.Typelimit = this.Typelimit;
            old.Maxcarnumber = this.Maxcarnumber;
            old.Cardtime = this.Cardtime;
            old.Firstweightime = this.Firstweightime;
            old.Secondweightime = this.Secondweightime;
            old.Worktime = this.Worktime;
            old.Outtime = this.Outtime;
            old.Configtype = this.Configtype;
            old.Loadarea = this.Loadarea;
            old.Unloadarea = this.Unloadarea;
        }
    }
}

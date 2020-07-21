using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Yongrong.Model.Db
{
    public partial class BillGoodsOut
    {
        public decimal Id { get; set; }
        public string Billid { get; set; }
        public string Company { get; set; }
        public decimal? Goodsnumber { get; set; }
        public string Goodsname { get; set; }
        public string Goodsstat { get; set; }
        public string Starttime { get; set; }
        public string Endtime { get; set; }
        public string Createtime { get; set; }
        public string Tractorids { get; set; }
        public string Trailerids { get; set; }
        public string Salesman { get; set; }
        public string Drivers { get; set; }
        public string Abholung { get; set; }
        public string Transport { get; set; }
        public string Supercargo { get; set; }
        public string Billstat { get; set; }
        public string Freezestatus { get; set; }
        public string Status { get; set; }

        public decimal? Levelnumber { get; set; }

        public string FinishStatus { get; set; }
        public string Goodsid { get; set; }
        public string Remark { get; set; }
        public void Upd(BillGoodsOut old)
        {
            old.Billid = this.Billid;
            old.Company = this.Company;
            old.Goodsnumber = this.Goodsnumber;
            old.Goodsname = this.Goodsname;
            old.Goodsstat = this.Goodsstat;
            old.Starttime = this.Starttime;
            old.Endtime = this.Endtime;
            old.Tractorids = this.Tractorids;
            old.Trailerids = this.Trailerids;
            old.Salesman = this.Salesman;
            old.Drivers = this.Drivers;
            old.Abholung = this.Abholung;
            old.Transport = this.Transport;
            old.Supercargo = this.Supercargo;
            old.Billstat = this.Billstat;
            old.Freezestatus = this.Freezestatus;
            old.Status = this.Status;
            old.FinishStatus = this.FinishStatus;
            if (null != this.Levelnumber)
            {
                old.Levelnumber = this.Levelnumber;
            }
            old.Goodsid = this.Goodsid;
            old.Remark = this.Remark;
        }
    }
}

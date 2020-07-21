using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Db
{
    public partial class OrderGoodsRefund
    {
        public decimal Id { get; set; }
        public string Orderid { get; set; }
        public string Driver { get; set; }
        public string Tractorid { get; set; }
        public string Trailerid { get; set; }
        public string Loadweight { get; set; }
        public string Realweight { get; set; }
        public string Supercargo { get; set; }
        public string Ordername { get; set; }
        public string Ordercompany { get; set; }
        public string Ordertime { get; set; }
        public string Orderstat { get; set; }
        public string Remark { get; set; }
        public string Goodsname { get; set; }
        public string Goodstypr { get; set; }
        public string Waybillid { get; set; }
        public string Goodsid { get; set; }
        public string Unit { get; set; }
        public string Unitext { get; set; }
        public string Netweight { get; set; }
        public string Capacity { get; set; }
        public string Printid { get; set; }
        public string Printtime { get; set; }
        public string Printman { get; set; }
        public string Checkman { get; set; }
        public string Indoorman { get; set; }
        public string Outdoorman { get; set; }
        public string Grosstime { get; set; }
        public string Grossman { get; set; }
        public string Taretime { get; set; }
        public string Tareman { get; set; }
        public string Storeman { get; set; }
        public string Transport { get; set; }
        public string Grossweight { get; set; }
        public string Tareweight { get; set; }
        public string Createtime { get; set; }
        public string Billid { get; set; }

        public void Upd(OrderGoodsRefund old)
        {
            old.Orderid = this.Orderid;
            old.Driver = this.Driver;
            old.Tractorid = this.Tractorid;
            old.Trailerid = this.Trailerid;
            old.Loadweight = this.Loadweight;
            old.Realweight = this.Realweight;
            old.Supercargo = this.Supercargo;
            old.Orderstat = this.Orderstat;
            old.Remark = this.Remark;
            old.Ordercompany = this.Ordercompany;
            old.Ordertime = this.Ordertime;
            old.Orderstat = this.Orderstat;
            old.Goodsname = this.Goodsname;
            old.Goodstypr = this.Goodstypr;
            old.Waybillid = this.Waybillid;
            old.Goodsid = this.Goodsid;
            old.Unit = this.Unit;
            old.Unitext = this.Unitext;
            old.Netweight = this.Netweight;
            old.Capacity = this.Capacity;
            old.Printid = this.Printid;
            old.Printtime = this.Printtime;
            old.Printman = this.Printman;
            old.Checkman = this.Checkman;
            old.Indoorman = this.Indoorman;
            old.Outdoorman = this.Outdoorman;
            old.Grosstime = this.Grosstime;
            old.Grossman = this.Grossman;
            old.Taretime = this.Taretime;
            old.Tareman = this.Tareman;
            old.Storeman = this.Storeman;
            old.Transport = this.Transport;
            old.Grossweight = this.Grossweight;
            old.Tareweight = this.Tareweight;
            old.Billid = this.Billid;
        }
    }
}

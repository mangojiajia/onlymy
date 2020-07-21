﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Yongrong.Model.Db
{
    public partial class OrderGoods
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
        public string Company { get; set; }
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

        [NotMapped]
        public bool IsSafeCheck { get; set; }
        /// <summary>
        /// 是否退回:1.不是退回,2.是退回
        /// </summary>
        public string Istoexit { get; set; }
        /// <summary>
        /// 是进场出厂:1.进场2.出厂
        /// </summary>
        public string Issendback { get; set; }

        [NotMapped]
        public string Salesman { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        [NotMapped]
        public string SerialNumber { get; set; }

        public string Billcheck { get; set; }

        public string Customercheck { get; set; }

        /// <summary>
        /// 总单里面可申请剩余量
        /// </summary>
        [NotMapped]
        public decimal Levelnumber { get; set; }

        [NotMapped]
        public string Starttime { get; set; }

        [NotMapped]
        public string Endtime { get; set; }

        public string Safecheckflag { get; set; }

        /// <summary>
        /// 是否提货
        /// </summary>
        public bool IsTakeGoods
        {
            get
            {
                if ("2".Equals(Issendback))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 获取进厂  出厂 退货
        /// </summary>
        public string GetBillType
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(Issendback) 
                    && !string.IsNullOrWhiteSpace(Istoexit) 
                    && "1".Equals(Istoexit) 
                    && "1".Equals(Issendback))
                {
                    return "进厂";
                }

                if (!string.IsNullOrWhiteSpace(Issendback) 
                    && !string.IsNullOrWhiteSpace(Istoexit) 
                    && "1".Equals(Istoexit) 
                    && "2".Equals(Issendback))
                {
                    return "出厂";
                }

                return "退货";
            }
        }

        public void Upd(OrderGoods old)
        {
            old.Orderid = this.Orderid;
            old.Driver = this.Driver;
            old.Tractorid = this.Tractorid;
            old.Trailerid = this.Trailerid;
            old.Loadweight = this.Loadweight;
            old.Realweight = this.Realweight;           
            old.Supercargo = this.Supercargo;
            old.Ordername = this.Ordername;
            old.Company = this.Company;
            old.Orderstat = this.Orderstat;
            old.Remark = this.Remark;
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
            old.Issendback = this.Issendback;
            old.Istoexit = this.Istoexit;
            old.Billcheck = this.Billcheck;
            old.Customercheck = this.Customercheck;
            old.Safecheckflag = this.Safecheckflag;
        }
    }
}

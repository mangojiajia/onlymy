using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICcard
{
    public class ICQueryOrderRsp
    {
        public string Stat { get; set; }

        public OrderGoods Data { get; set; }
    }

    public class OrderGoods
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
        /// <summary>
        /// 是否退回:1.不是退回,2.是退回
        /// </summary>
        public string Istoexit { get; set; }
        /// <summary>
        /// 是进场出厂:1.进场2.出厂
        /// </summary>
        public string Issendback { get; set; }

        public string Salesman { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 总单里面可申请剩余量
        /// </summary>
        public decimal Levelnumber { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder()
                .AppendLine($" 预约号:{this.Orderid}")
                .AppendLine($" 司机:{this.Driver}")
                .AppendLine($" 车牌:{Tractorid}");

            return sb.ToString();
        }
    }
}

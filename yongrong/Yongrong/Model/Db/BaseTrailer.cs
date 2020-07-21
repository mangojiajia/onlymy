using System;
using System.Collections.Generic;
using Yongrong.Model.Int;

namespace Yongrong.Model.Db
{
    public partial class BaseTrailer
    {
        public int Id { get; set; }
        public string TrailerId { get; set; }
        public string Region { get; set; }
        public string Degree { get; set; }
        public string Carflag { get; set; }
        public string Validdate { get; set; }
        public decimal? Maxweight { get; set; }
        public string Carid { get; set; }
        public string Transport { get; set; }
        public string Createtime { get; set; }
         
        public void Upd(BaseTrailer old)
        {
            old.TrailerId = this.TrailerId;
            old.Region = this.Region;
            old.Degree = this.Degree;
            old.Carflag = this.Carflag;
            old.Validdate = this.Validdate;
            old.Maxweight = this.Maxweight;
            old.Carid = this.Carid;
            old.Transport = this.Transport;

        }

        /// <summary>
        /// 必要字段校验
        /// </summary>
        /// <param name="tractor"></param>
        /// <returns></returns>
        public string Check()
        {
            if (string.IsNullOrWhiteSpace(TrailerId) )
            {
                return "挂车号牌错误";
            }

            TrailerId = TrailerId.Trim();

            if (string.IsNullOrWhiteSpace(Transport))
            {
                return "隶属客商不能为空";
            }

            Transport = Transport.Trim();

            if (string.IsNullOrWhiteSpace(Carflag))
            {
                return "挂车证件标号不能为空";
            }

            Carflag = Carflag.Trim();

            if (string.IsNullOrWhiteSpace(Carid))
            {
                return "运输车编号不能为空";
            }

            Carid = Carid.Trim();


            if (string.IsNullOrWhiteSpace(Validdate))
            {
                return "有效期不能为空";
            }

            Validdate = Validdate.Trim();


            if (null== Maxweight ||0 >= Maxweight)
            {
                return "最大荷载量应大于0";
            }

            return BaseReq.Success;
        }
    }
}

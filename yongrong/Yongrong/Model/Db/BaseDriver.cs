using System;
using System.Collections.Generic;
using Yongrong.Model.Int;

namespace Yongrong.Model.Db
{
    public partial class BaseDriver
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string Cardid { get; set; }
        public string Driverid { get; set; }
        public string Driverdegree { get; set; }
        public string Drivervalid { get; set; }
        public string TractorId { get; set; }
        public string TrailerId { get; set; }
        public string Transport { get; set; }
        public string Code { get; set; }
        public string Createtime { get; set; }
        public string Otherid { get; set; }
        public string Othervalid { get; set; }
        public string Tel { get; set; }
        public string Whiteflag { get; set; }



        public void Upd(BaseDriver old)
        {
            old.Name = this.Name;
            old.Cardid = this.Cardid;
            old.Driverid = this.Driverid;
            old.Driverdegree = this.Driverdegree;
            old.Drivervalid = this.Drivervalid;
            old.TractorId = this.TractorId;
            old.TrailerId = this.TrailerId;
            old.Transport = this.Transport;
            old.Code = this.Code;
            old.Otherid = this.Otherid;
            old.Othervalid = this.Othervalid;
            old.Tel = this.Tel;
            old.Whiteflag = this.Whiteflag;
        }

        /// <summary>
        /// 必要字段校验
        /// </summary>
        /// <returns></returns>
        public  string Check()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return "姓名不能为空";
            }
            
            Name = Name.Trim();

            if (string.IsNullOrWhiteSpace(Tel))
            {
                return "手机号码不能为空";
            }

            Tel = Tel.Trim();
            
            if (11 != Tel.Length )
            {
                return "手机号码为11位";
            }

            if (string.IsNullOrWhiteSpace(Cardid))
            {
                return "身份证号不能为空";
            }

            Cardid = Cardid.Trim();


            if (string.IsNullOrWhiteSpace(Driverid))
            {
                return "驾照编号不能为空";
            }

            Driverid = Driverid.Trim();

            if (string.IsNullOrWhiteSpace(Driverdegree))
            {
                return "驾照等级不能为空";
            }

            Driverdegree = Driverdegree.Trim();

            if (string.IsNullOrWhiteSpace(Drivervalid))
            {
                return "驾照有效期不能为空";
            }

            Drivervalid = Drivervalid.Trim();


            if (string.IsNullOrWhiteSpace(TractorId))
            {
                return "牵引车车牌不能为空";
            }

            TractorId = TractorId.Trim();

            if (string.IsNullOrWhiteSpace(TrailerId))
            {
                return "挂车车牌不能为空";
            }

            TrailerId = TrailerId.Trim();


            if (string.IsNullOrWhiteSpace(Transport))
            {
                return "隶属客商不能为空";
            }

            Transport = Transport.Trim();

            return BaseReq.Success;
        }
    }
}

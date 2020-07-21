using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Int;

namespace Yongrong.Model.Db
{
    public partial class BaseSupercargo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cardid { get; set; }
        public string Driverid { get; set; }
        public string Drivervalid { get; set; }
        public string Otherid { get; set; }
        public string Othervalid { get; set; }
        public string Driver { get; set; }
        public string TractorId { get; set; }
        public string TrailerId { get; set; }
        public string Transport { get; set; }
        public string Code { get; set; }
        public string Createtime { get; set; }
        public string Whiteflag { get; set; }

        public void Upd(BaseSupercargo old)
        {
            old.Name = this.Name;
            old.Cardid = this.Cardid;
            old.Driverid = this.Driverid;
            old.Drivervalid = this.Drivervalid;
            old.Otherid = this.Otherid;
            old.Othervalid = this.Othervalid;
            old.Driver = this.Driver;
            old.TractorId = this.TractorId;
            old.TrailerId = this.TrailerId;
            old.Transport = this.Transport;
            old.Code = this.Code;
            old.Whiteflag = this.Whiteflag;
        }

        public string Check()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return "姓名不能为空";
            }

            Name = Name.Trim();


            if (string.IsNullOrWhiteSpace(Transport))
            {
                return "隶属客商不能为空";
            }

            Transport = Transport.Trim();

            if (string.IsNullOrWhiteSpace(Cardid))
            {
                return "身份证号不能为空";
            }

            Cardid = Cardid.Trim();

            if (string.IsNullOrWhiteSpace(Driverid))
            {
                return "押运证件编号不能为空";
            }

            Driverid = Driverid.Trim();

            if (string.IsNullOrWhiteSpace(Drivervalid))
            {
                return "证件有效期不能为空";
            }

            Drivervalid = Drivervalid.Trim();

            if (string.IsNullOrWhiteSpace(Driver))
            {
                return "对应司机不能为空";
            }

            Driver = Driver.Trim();

            return BaseReq.Success;
        }
    }
}

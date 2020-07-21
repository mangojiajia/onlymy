using System;
using System.Collections.Generic;
using Yongrong.Model.Int;

namespace Yongrong.Model.Db
{
    public partial class BaseTractor
    {
        public int Id { get; set; }
        public string TractorId { get; set; }
        public string Region { get; set; }
        public string Degree { get; set; }
        public string Carid { get; set; }
        public string Validdate { get; set; }
        public string Transport { get; set; }
        public string Createtime { get; set; }
         
        public void Upd(BaseTractor old)
        {
            old.TractorId = this.TractorId;
            old.Region = this.Region;
            old.Degree = this.Degree;
            old.Carid = this.Carid;
            old.Validdate = this.Validdate;
            old.TractorId = this.TractorId;
            old.Transport = this.Transport;
            
        }

        public string Check()
        {
            if (string.IsNullOrWhiteSpace(TractorId) )
            {
                return "牵引车车牌错误";
            }

            TractorId = TractorId.Trim();

            if (string.IsNullOrWhiteSpace(Transport))
            {
                return "隶属客商不能为空";
            }

            Transport = Transport.Trim();

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

            return BaseReq.Success;
        }
    }
}

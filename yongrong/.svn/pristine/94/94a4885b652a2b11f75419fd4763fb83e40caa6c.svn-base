using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseTractorAddReq : BaseInfoReq
    {
        public BaseTractor Tractor { get; set; }

        public override string Check()
        {
            if ("del".Equals(Op))
            {
                return Success;
            }

            if(null == Tractor)
            {
                return "参数为空";
            }

            if(string.IsNullOrWhiteSpace(Tractor.TractorId))
            {
                return "车牌为空";
            }

            Tractor.TractorId = Tractor.TractorId.Trim();

            if (string.IsNullOrWhiteSpace(Tractor.Transport))
            {
                return "运输公司为空";
            }

            Tractor.Transport = Tractor.Transport.Trim();

            if (string.IsNullOrWhiteSpace(Tractor.Carid))
            {
                return "运输车编号不能为空";
            }

            Tractor.Carid = Tractor.Carid.Trim();


            if (string.IsNullOrWhiteSpace(Tractor.Validdate))
            {
                return "有效期不能为空";
            }

            Tractor.Validdate = Tractor.Validdate.Trim();

            return Success;
        }
    }
}

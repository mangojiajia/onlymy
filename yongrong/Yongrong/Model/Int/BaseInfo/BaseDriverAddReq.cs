using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseDriverAddReq : BaseInfoReq
    {
        public BaseDriver Driver { get; set; }
        public override string Check()
        {
            if ("add".Equals(Op) || "upd".Equals(Op))
            {
                if (null == Driver)
                {
                    return "参数为空";
                }
                if (string.IsNullOrWhiteSpace(Driver.Name))
                {
                    return "司机姓名不能为空";
                }

                Driver.Name = Driver.Name.Trim();

                if (string.IsNullOrWhiteSpace(Driver.Cardid))
                {
                    return "司机身份证不能为空";
                }

                Driver.Cardid = Driver.Cardid.Trim();

                if (string.IsNullOrWhiteSpace(Driver.Driverid))
                {
                    return "司机身驾照编号不能为空";
                }

                Driver.Driverid = Driver.Driverid.Trim();

                if (string.IsNullOrWhiteSpace(Driver.TractorId))
                {
                    return "牵引车车牌不能为空";
                }

                Driver.TractorId = Driver.TractorId.Trim();

                if (string.IsNullOrWhiteSpace(Driver.TrailerId))
                {
                    return "挂车车牌不能为空";
                }
                
                Driver.TrailerId = Driver.TrailerId.Trim();

                if (string.IsNullOrWhiteSpace(Driver.Tel))
                {
                    return "司机手机号码不能为空";
                }

                Driver.Tel = Driver.Tel.Trim();

                if (11 != Driver.Tel.Length)
                {
                    return "请输入正确的11位手机号码";
                }

                if (string.IsNullOrWhiteSpace(Driver.Drivervalid))
                {
                    return "驾照有效期不能为空";
                }

                Driver.Drivervalid = Driver.Drivervalid.Trim();

                if (string.IsNullOrWhiteSpace(Driver.Driverdegree))
                {
                    return "驾照等级不能为空";
                }

                Driver.Driverdegree = Driver.Driverdegree.Trim();

                if (string.IsNullOrWhiteSpace(Driver.Transport))
                {
                    return "隶属运输公司不能为空";
                }

                Driver.Transport = Driver.Transport.Trim();
            }

            return Success;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseSupercargoAddReq : BaseInfoReq
    {
        public BaseSupercargo Supercargo { get; set; }

        public override string Check()
        {
            if ("del".Equals(Op))
            {
                return Success;
            }

            if(null == Supercargo)
            {
                return "参数为空";
            }


            if(string.IsNullOrWhiteSpace(Supercargo.Name))
            {
                return "押运员姓名为空";
            }

            Supercargo.Name = Supercargo.Name.Trim();

            if (string.IsNullOrWhiteSpace(Supercargo.Transport))
            {
                return "隶属运输公司为空";
            }

            Supercargo.Transport = Supercargo.Transport.Trim();

            if (string.IsNullOrWhiteSpace(Supercargo.Drivervalid))
            {
                return "驾照有效期不能为空";
            }

            Supercargo.Drivervalid = Supercargo.Drivervalid.Trim();


            if (string.IsNullOrWhiteSpace(Supercargo.Cardid))
            {
                return "身份证号不能为空";
            }

            Supercargo.Cardid = Supercargo.Cardid.Trim();

            if (string.IsNullOrWhiteSpace(Supercargo.Driverid))
            {
                return "押运证件编号不能为空";
            }

            Supercargo.Driverid = Supercargo.Driverid.Trim();

            if (string.IsNullOrWhiteSpace(Supercargo.Drivervalid))
            {
                return "证件有效期不能为空";
            }

            Supercargo.Drivervalid = Supercargo.Drivervalid.Trim();

            if (string.IsNullOrWhiteSpace(Supercargo.Driver))
            {
                return "对应司机不能为空";
            }

            Supercargo.Driver = Supercargo.Driver.Trim();

            return Success;
        }
    }
}

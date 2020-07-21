using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BillGoodsInAddReq : BaseInfoReq
    {
        public BillGoodsIn Goodsextenterbill { get; set; }
        public override string Check()
        {
            if ("del".Equals(Op))
            {
                return Success;
            }

            if(string.IsNullOrWhiteSpace(Goodsextenterbill.Billid))
            {
                return "总单编号为空";
            }
            if(string.IsNullOrWhiteSpace(Goodsextenterbill.Company))
            {
                return "公司名称为空";
            }
            if(string.IsNullOrWhiteSpace(Goodsextenterbill.Starttime))
            {
                return "开始时间为空";
            }
            if(Goodsextenterbill.Starttime.Trim().Length != 10)
            {
                return "开始时间格式不符合yyyy-MM-dd的形式";
            }
            if(string.IsNullOrWhiteSpace(Goodsextenterbill.Endtime))
            {
                return "结束时间为空";
            }
            if (Goodsextenterbill.Endtime.Trim().Length != 10)
            {
                return "结束时间格式不符合yyyy-MM-dd的形式";
            }
            if (string.IsNullOrWhiteSpace(Goodsextenterbill.Goodsname))
            {
                return "货物名称为空";
            }
            if(string.IsNullOrWhiteSpace(Goodsextenterbill.Drivers))
            {
                return "司机为空";
            }
            if(string.IsNullOrWhiteSpace(Goodsextenterbill.Tractorids))
            {
                return "牵引车为空";
            }
            if(string.IsNullOrWhiteSpace(Goodsextenterbill.Trailerids))
            {
                return "挂车为空";
            }
            if(string.IsNullOrWhiteSpace(Goodsextenterbill.Transport))
            {
                return "运输公司为空";
            }
            /*if(string.IsNullOrWhiteSpace(Goodsextenterbill.Supercargo))
            {
                return "押运员为空";
            }*/
            if(string.IsNullOrWhiteSpace(Goodsextenterbill.Maker))
            {
                return "经办销售员为空";
            }
            if(0 >= Goodsextenterbill.Goodsnumber)
            {
                return "总量为0";
            }
            return "0";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BillGoodsRefundAddReq : BaseInfoReq
    {
        public BillGoodsRefund GoodsRefund { get; set; }

        public override string Check()
        {
            if ("del".Equals(Op))
            {
                return Success;
            }

            if (string.IsNullOrWhiteSpace(GoodsRefund.Billid))
            {
                return "总单编号为空";
            }
            if (string.IsNullOrWhiteSpace(GoodsRefund.Company))
            {
                return "公司名称为空";
            }
            if (string.IsNullOrWhiteSpace(GoodsRefund.Starttime))
            {
                return "开始时间为空";
            }
            if (GoodsRefund.Starttime.Trim().Length != 10)
            {
                return "开始时间格式不符合yyyy-MM-dd的形式";
            }
            if (string.IsNullOrWhiteSpace(GoodsRefund.Endtime))
            {
                return "结束时间为空";
            }
            if (GoodsRefund.Endtime.Trim().Length != 10)
            {
                return "结束时间格式不符合yyyy-MM-dd的形式";
            }
            if (string.IsNullOrWhiteSpace(GoodsRefund.Goodsname))
            {
                return "货物名称为空";
            }
            if (string.IsNullOrWhiteSpace(GoodsRefund.Drivers))
            {
                return "司机为空";
            }
            if (string.IsNullOrWhiteSpace(GoodsRefund.Tractorids))
            {
                return "牵引车为空";
            }
            if (string.IsNullOrWhiteSpace(GoodsRefund.Trailerids))
            {
                return "挂车为空";
            }
            if (string.IsNullOrWhiteSpace(GoodsRefund.Transport))
            {
                return "运输公司为空";
            }
            /*if (string.IsNullOrWhiteSpace(GoodsRefund.Supercargo))
            {
                return "押运员为空";
            }*/
            if (string.IsNullOrWhiteSpace(GoodsRefund.Maker))
            {
                return "经办销售员为空";
            }
            if (0 >= GoodsRefund.Goodsnumber)
            {
                return "总量为0";
            }
            return "0";
        }
    }
}

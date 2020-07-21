using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BillGoodsOutAddReq : BaseInfoReq
    {
        public BillGoodsOut GoodsOut { get; set; }
        public override string Check()
        {
            if ("del".Equals(Op))
            {
                return Success;
            }
            
            if (null == GoodsOut)
            {
                return "添加对象为空";
            }

            if (string.IsNullOrWhiteSpace(GoodsOut.Billid))
            {
                return "总单编号为空";
            }
            if (string.IsNullOrWhiteSpace(GoodsOut.Company))
            {
                return "公司名称为空";
            }
            if (string.IsNullOrWhiteSpace(GoodsOut.Starttime))
            {
                return "开始时间为空";
            }
            if (GoodsOut.Starttime.Trim().Length != 10)
            {
                return "开始时间格式不符合yyyy-MM-dd的形式";
            }
            if (string.IsNullOrWhiteSpace(GoodsOut.Endtime))
            {
                return "结束时间为空";
            }
            if (GoodsOut.Endtime.Trim().Length != 10)
            {
                return "结束时间格式不符合yyyy-MM-dd的形式";
            }
            if (string.IsNullOrWhiteSpace(GoodsOut.Goodsname))
            {
                return "货物名称为空";
            }
            if (string.IsNullOrWhiteSpace(GoodsOut.Drivers))
            {
                return "司机为空";
            }
            if (string.IsNullOrWhiteSpace(GoodsOut.Tractorids))
            {
                return "牵引车为空";
            }
            if (string.IsNullOrWhiteSpace(GoodsOut.Trailerids))
            {
                return "挂车为空";
            }
            if (string.IsNullOrWhiteSpace(GoodsOut.Transport))
            {
                return "运输公司为空";
            }

            /*if (string.IsNullOrWhiteSpace(GoodsOut.Supercargo))
            {
                return "押运员为空";
            }*/
            

            if (string.IsNullOrWhiteSpace(GoodsOut.Salesman))
            {
                return "经办销售员为空";
            }
            
            if (0 >= GoodsOut.Goodsnumber)
            {
                return "总量为0";
            }
            return "0";
        }
    }
}

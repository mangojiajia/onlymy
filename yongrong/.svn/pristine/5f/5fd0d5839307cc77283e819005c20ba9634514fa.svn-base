using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseGoodsAddReq : BaseInfoReq
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseGoods Goods { get; set; }

        /// <summary>
        /// 返回0表示成功
        /// </summary>
        /// <returns></returns>
        public override string Check()
        {
            if ("del".Equals(Op))
            {
                return Success;
            }
            if(string.IsNullOrWhiteSpace(Goods.Goodsname))
            {
                return "物料名称为空";
            }
            if(string.IsNullOrWhiteSpace(Goods.Goodsid))
            {
                return "物料编号为空";
            }
            return "0";
        }
    }
}

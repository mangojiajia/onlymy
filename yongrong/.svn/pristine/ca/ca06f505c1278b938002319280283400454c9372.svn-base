using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Phone
{
    public class PhoneQueryBillRsp : BaseRsp
    {
        /// <summary>
        /// 进厂总单
        /// </summary>
        public List<BillGoodsIn> GoodsInList { get; set; } = new List<BillGoodsIn>();

        /// <summary>
        /// 出厂总单
        /// </summary>
        public List<BillGoodsOut> GoodsOutList { get; set; } = new List<BillGoodsOut>();

        /// <summary>
        /// 退货总单
        /// </summary>
        public List<BillGoodsRefund> GoodsRefundList { get; set; } = new List<BillGoodsRefund>();

        /// <summary>
        /// 司机
        /// </summary>
        public BaseDriver Driver { get; set; }

        /// <summary>
        /// 挂车
        /// </summary>
        public BaseTrailer Trailer { get; set; }

        /// <summary>
        /// 押运员
        /// </summary>
        public BaseSupercargo Supercargo { get; set; }
    }
}

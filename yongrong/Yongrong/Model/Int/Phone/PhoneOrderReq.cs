﻿using BaseS.String;
using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Phone
{
    public class PhoneOrderReq : BasePageReq
    {
        /// <summary>
        /// 时间戳参数time 
        /// 时间戳是指UTC时间1970年01月01日00时00分00秒(北京时间1970年01月01日08时00分00秒)起至现在的总毫秒数
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 预约对象
        /// </summary>
        public OrderGoods AddObj { get; set; }






        public override string Check()
        {
            if (string.IsNullOrWhiteSpace(Time))
            {
                return "时间戳为空";
            }

            if (string.IsNullOrWhiteSpace(Sign))
            {
                return "签名为空";
            }

            var t = BString.Get1970ToNowMilliseconds(DateTime.Now);

            long.TryParse(Time, out var src);

            if (Yongrong.Conf.BaseConf.Sys.SysTimeSpan < Math.Abs(t - src))
            {
                return "时间戳差距过大";
            }

            if (null == AddObj)
            {
                return "预约信息为空";
            }

            if (string.IsNullOrWhiteSpace(AddObj.Billid))
            {
                return "总单编号为空";
            }

            if (string.IsNullOrWhiteSpace(AddObj.Driver))
            {
                return "司机为空";
            }
            if (string.IsNullOrWhiteSpace(AddObj.Tractorid))
            {
                return "牵引车为空";
            }
            if (string.IsNullOrWhiteSpace(AddObj.Trailerid))
            {
                return "挂车为空";
            }
            if (string.IsNullOrWhiteSpace(AddObj.Ordertime))
            {
                return "预约时间为空";
            }

            if (string.IsNullOrWhiteSpace(AddObj.Realweight))
            {
                return "预约重量为空";
            }
            /*if(string.IsNullOrWhiteSpace(AddObj.Supercargo))
            {
                return "押运员为空";
            }*/

            AddObj.Realweight = AddObj.Realweight.Trim('吨');
            AddObj.Realweight = AddObj.Realweight.Trim('T');
            AddObj.Realweight = AddObj.Realweight.Trim('t');
            AddObj.Realweight = AddObj.Realweight.Trim();

            if (Convert.ToDecimal(AddObj.Realweight) <= 0)
            {
                return "预约重量为空";
            }

            return Success;
        }
    }
}

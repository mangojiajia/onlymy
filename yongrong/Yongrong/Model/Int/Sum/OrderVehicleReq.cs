using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int.Sum
{
    public class OrderVehicleReq : BaseReq
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string Starttime { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public string Endtime { get; set; }

        public override string Check()
        {
            if (string.IsNullOrWhiteSpace(Starttime) && string.IsNullOrWhiteSpace(Endtime))
            {
                return Success;
            }

            if (string.IsNullOrWhiteSpace(Starttime))
            {
                return "开始时间不能为空";
            }

            if (string.IsNullOrWhiteSpace(Endtime))
            {
                return "结束日期不能为空";
            }

            if (!DateTime.TryParseExact(Starttime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startTime))
            {
                return "开始时间日期格式不正确";
            }

            if (!DateTime.TryParseExact(Endtime, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime endTime))
            {
                return "结束时间日期格式不正确";
            }
            return Success;
        }
    }
}

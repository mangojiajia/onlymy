using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Gate
{
    [DataContract]
    public class GateGetCarInfoReq
    {
        [DataMember(Name = "appkey")]
        public string Appkey { get; set; }

        [DataMember(Name = "time")]
        public long Time { get; set; }

        [DataMember(Name = "pageNo")]
        public int PageNo { get; set; } = 1;

        [DataMember(Name = "pageSize")]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 操作用户UUID
        /// </summary>
        [DataMember(Name = "opUserUuid")]
        public string OpUserUuid { get; set; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        [DataMember(Name = "plateNo")]
        public string PlateNo { get; set; }
    }
}

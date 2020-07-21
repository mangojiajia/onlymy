using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Gate
{
    [DataContract]
    public class GateRechargeCarReq
    {
        [DataMember(Name = "appkey")]
        public string Appkey { get; set; }

        [DataMember(Name = "time")]
        public long Time { get; set; }

        /// <summary>
        /// 操作用户UUID
        /// </summary>
        [DataMember(Name = "opUserUuid")]
        public string OpUserUuid { get; set; }

        [DataMember(Name = "parkUuid")]
        public string ParkUuid { get; set; }

        [DataMember(Name = "plateNo")]
        public string PlateNo { get; set; }

        [DataMember(Name = "startTime")]
        public long StartTime { get; set; }

        [DataMember(Name = "endTime")]
        public long EndTime{ get; set; }

        [DataMember(Name = "money")]
        public string Money{ get; set; }

        [DataMember(Name = "remark")]
        public string Remark { get; set; }
    }
}

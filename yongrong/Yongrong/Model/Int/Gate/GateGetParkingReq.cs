using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Gate
{
    [DataContract]
    public class GateGetParkingReq
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
    }
}

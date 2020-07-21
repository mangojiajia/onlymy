using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Gate
{
    [DataContract]
    public class GateAddCarReq
    {
        [DataMember(Name ="appkey")]
        public string Appkey { get; set; }

        [DataMember(Name = "time")]
        public long Time { get; set; }

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

        public string CardNo { get; set; }

        public long OwnerId { get; set; }

        public long PlateType { get; set; }

        public long PlateColor { get; set; }

        /// <summary>
        /// 车辆类型
        /// </summary>
        [DataMember(Name = "carType")]
        public long CarType { get; set; }

        public long CarColor { get; set; }

        public long PlateStart { get; set; }

        public string PersonName { get; set; }

        public string PhoneNo { get; set; }

        public long MaxPassenger { get; set; }
    }
}

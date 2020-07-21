using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Gate
{
    [DataContract]
    public class GateGetEventTypeRsp : GateRsp
    {
        [DataMember(Name = "data")]
        public List<GateGetEventTypeData> Data { get; set; }
    }

    [DataContract]
    public class GateGetEventTypeData
    {
        [DataMember(Name = "subSystemUuid")]
        public string SubSystemUuid { get; set; }

        [DataMember(Name = "eventType")]
        public int EventType { get; set; }

        [DataMember(Name = "eventTypeName")]
        public string EventTypeName { get; set; }
    }
}

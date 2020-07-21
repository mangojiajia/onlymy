using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Gate
{
    [DataContract]
    public class GateSubscribeEventsRsp : GateRsp
    {
        [DataMember(Name ="")]
        public GateSubscribeEventsData Data { get; set; }
    }

    [DataContract]
    public class GateSubscribeEventsData
    {
        [DataMember(Name = "mqURL")]
        public string MqURL { get; set; }

        [DataMember(Name = "destination")]
        public string Destination { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Gate
{
    [DataContract]
    public class GateDelCarRsp : GateRsp
    {
        [DataMember(Name = "data")]
        public string Data { get; set; }
    }
}

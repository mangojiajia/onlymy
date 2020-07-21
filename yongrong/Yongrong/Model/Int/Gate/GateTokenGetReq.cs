using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Gate
{
    [DataContract]
    public class GateTokenGetReq
    {
        [DataMember(Name = "appkey")]
        public string Appkey { get; set; }

        [DataMember(Name = "time")]
        public string Time { get; set; }

        //[DataMember(Name = "opUserUuid")]
        public string OpUserUuid { get; set; }

        //[DataMember(Name = "cameraUuid")]
        public string CameraUuid { get; set; }

        //[DataMember(Name = "netZoneUuid")]
        public string NetZoneUuid { get; set; }
    }
}

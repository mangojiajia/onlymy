using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Gate
{
    [DataContract]
    public class GateAddCarRsp : GateRsp
    {
        [DataMember(Name = "data")]
        public GateCardData Data { get; set; }
    }

    [DataContract]
    public class GateCardData : GateAddCarReq
    {
        [DataMember(Name = "preBillUuid")]
        public string PreBillUuid { get; set; }

        //[DataMember(Name = "plateNo")]
        //public string PlateNo { get; set; }

        [DataMember(Name = "parkUuid")]
        public string ParkUuid { get; set; }
    }
}

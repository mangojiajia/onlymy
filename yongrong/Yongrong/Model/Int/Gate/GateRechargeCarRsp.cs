﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Gate
{
    [DataContract]
    public class GateRechargeCarRsp : GateRsp
    {
        [DataMember(Name = "data")]
        public RechargeData Data { get; set; }

    

    }

    [DataContract]
    public class RechargeData
    {
        [DataMember(Name = "billUuid")]
        public string BillUuid { get; set; }

        [DataMember(Name = "parkUuid")]
        public string ParkUuid { get; set; }

        [DataMember(Name = "carUuid")]
        public string CarUuid { get; set; }

        [DataMember(Name = "startTime")]
        public long StartTime { get; set; }

        [DataMember(Name = "endTime")]
        public long EndTime { get; set; }

        [DataMember(Name = "money")]
        public float Money { get; set; }

        [DataMember(Name = "operator")]
        public string Operator { get; set; }

        [DataMember(Name = "remark")]
        public string Remark { get; set; }
    }
}

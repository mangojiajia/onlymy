using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Gate
{
    [DataContract]
    public class GateGetParkingRsp : GateRsp
    {
        [DataMember(Name = "data")]
        public GateParkingData Data { get; set; }
    }

    [DataContract]
    public class GateParkingData
    {
        [DataMember(Name = "parkUuid")]
        public string ParkUuid { get; set; }

        [DataMember(Name = "parkName")]
        public string ParkName { get; set; }

        [DataMember(Name = "totalPlot")]
        public int TotalPlot { get; set; }

        [DataMember(Name = "leftPlot")]
        public int LeftPlot { get; set; }

        [DataMember(Name = "totalFiexdPlot")]
        public int TotalFiexdPlot { get; set; }

        [DataMember(Name = "leftFiexdPlot")]
        public int LeftFiexdPlot { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}

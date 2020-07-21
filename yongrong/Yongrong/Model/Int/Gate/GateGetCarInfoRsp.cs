using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Gate
{
    [DataContract]
    public class GateGetCarInfoRsp : GateRsp
    {
        [DataMember(Name = "data")]
        public GetCarRsp Data { get; set; }
    }

    [DataContract]
    public class GetCarRsp
    {
        [DataMember(Name = "total")]
        public int Total { get; set; }

        [DataMember(Name = "pageNo")]
        public int PageNo { get; set; } = 1;

        [DataMember(Name = "pageSize")]
        public int PageSize { get; set; } = 10;

        [DataMember(Name = "list")]
        public List<GetCarObj> ObjList { get; set; }
    }

    [DataContract]
    public class GetCarObj
    {
        [DataMember(Name = "carUuid")]
        public string CarUuid { get; set; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        [DataMember(Name = "plateNo")]
        public string PlateNo { get; set; }

        [DataMember(Name = "plateType")]
        public int? PlateType { get; set; }

        [DataMember(Name = "plateColor")]
        public int? PlateColor { get; set; }

        [DataMember(Name = "carType")]
        public int? CarType { get; set; }

        [DataMember(Name = "ownerId")]
        public int? OwnerId { get; set; }

        [DataMember(Name = "carColor")]
        public int? CarColor { get; set; }

        [DataMember(Name = "personName")]
        public string PersonName { get; set; }

        [DataMember(Name = "phoneNo")]
        public string PhoneNo { get; set; }

        [DataMember(Name = "cardNo")]
        public string CardNo { get; set; }

        [DataMember(Name = "maxPassenger")]
        public int? MaxPassenger { get; set; }

        [DataMember(Name = "plateStart")]
        public long? PlateStart { get; set; }
    }

}

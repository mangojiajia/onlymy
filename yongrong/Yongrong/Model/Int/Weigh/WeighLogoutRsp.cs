using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Weigh
{
    [DataContract]
    public class WeighLogoutRsp
    {
        // {"status":0,"message":"ok","rows_affected":1}
        [DataMember(Name = "status")]
        public string Status { get; set; }
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "rows_affected")]
        public int RowAffected { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Yongrong.Model.Int.Weigh
{
    [DataContract]
    public class WeighLogoutReq
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }

        public string Version { get; set; }
    }
}

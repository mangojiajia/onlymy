using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseLoadingplaceReq : BasePageReq
    {
        public string Placeid { get; set; }
        public string Placename { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string Check()
        {
            return "0";
        }
    }
}

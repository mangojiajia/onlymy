using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class BaseLoadingplaceAddReq : BaseInfoReq
    {
        /// <summary>
        /// 装卸区对象
        /// </summary>
        public BaseLoadingplace Place { get; set; }

        /// <summary>
        /// 自检
        /// </summary>
        /// <returns></returns>
        public override string Check()
        {
            if ("del".Equals(Op))
            {
                return Success;
            }
            if (null == Place)
            {
                return "参数为空";
            }
            if(string.IsNullOrWhiteSpace(Place.Placename))
            {
                return "装卸车区名称为空";
            }
            if(string.IsNullOrWhiteSpace(Place.Placeid))
            {
                return "装卸车区编号为空";
            }

            return "0";
        }
    }
}

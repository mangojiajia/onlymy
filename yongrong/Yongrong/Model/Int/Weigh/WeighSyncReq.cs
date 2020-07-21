using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.Weigh
{
    public class WeighSyncReq : ApiWeigh
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Check()
        {

            return "0";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ApiWeigh ToApiWeigh()
        {
            return new ApiWeigh()
            {

            };
        }
    }
}

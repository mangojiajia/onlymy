using System;
using System.Collections.Generic;
using System.Text;
using BaseS.Serialization;

namespace Yongrong.Model.Int
{
    public class BaseRsp
    {
        public string Stat { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseRsp()
        {

        }

        /// <summary>
        /// 对象转换成字节码
        /// </summary>
        /// <returns></returns>
        public byte[] ToBytes()
        {
            byte[] rspbytes = null;

            this.ObjToJson(ref rspbytes, null, null, false);

            return rspbytes;
        }
    }
}

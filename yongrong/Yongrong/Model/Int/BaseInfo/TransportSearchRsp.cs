using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;

namespace Yongrong.Model.Int.BaseInfo
{
    public class TransportSearchRsp : BaseRsp
    {
        /// <summary>
        /// 司机列表
        /// </summary>
        public List<BaseDriver> Drivers;

        /// <summary>
        /// 牵引车列表
        /// </summary>
        public List<BaseTractor> Tractors;

        /// <summary>
        /// 挂车列表
        /// </summary>
        public List<BaseTrailer> Trailers;

        /// <summary>
        /// 押运员列表
        /// </summary>
        public List<BaseSupercargo> Supercargos;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Yongrong.Model.Db
{
    public partial class GateLog
    {
        /// <summary>
        /// 序号
        /// </summary>
        public decimal Id { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string Tractorid { get; set; }

        /// <summary>
        /// 车牌类型  0-其他车 1-小型车 2-大型车
        /// </summary>
        public string Cartype { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Reqparameter { get; set; }

        /// <summary>
        /// 应答结果 0-成功，其他失败
        /// </summary>
        public string Rspcode { get; set; }

        /// <summary>
        /// 门禁返回信息
        /// </summary>
        public string Rspmessage { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string Createtime { get; set; }


        public void Upd(GateLog old)
        {
            old.Tractorid = this.Tractorid;
            old.Cartype = this.Cartype;
            old.Operation = this.Operation;
            old.Reqparameter = this.Reqparameter;
        }

    }
}

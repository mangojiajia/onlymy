using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Db
{
    /// <summary>
    /// Pda日志表
    /// </summary>
    public partial class PdaLog
    {

        /// <summary>
        /// 序号
        /// </summary>
        public decimal Id { get; set; }

        /// <summary>
        /// 预约码
        /// </summary>
        public string Orderid { get; set; }

        /// <summary>
        /// 车牌号/登陆者账号
        /// </summary>
        public string Tractorid { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public string Operation { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Reqparameter { get; set; }

        /// <summary>
        /// 应答结果 0-成功，-1失败
        /// </summary>
        public string Rspcode { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public string Rspmessage { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string Createtime { get; set; }


        public void Upd(PdaLog old)
        {
            old.Operation = this.Operation;
            old.Reqparameter = this.Reqparameter;
            old.Rspmessage = this.Rspmessage;
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Yongrong.Model.Db
{
    public partial class Abnormal
    {
        /// <summary>
        /// 序号
        /// </summary>
        public decimal Id { get; set; }
        /// <summary>
        /// 异常账号
        /// </summary>
        public string Abnormalname { get; set; }
        /// <summary>
        /// 异常类型 
        /// </summary>
        public string Abnormaltype { get; set; }
        /// <summary>
        /// 异常详情
        /// </summary>
        public string Abnormalcase { get; set; }
        /// <summary>
        /// 是否已处理 0-未处理 1-已处理
        /// </summary>
        public string Isdispose { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Createuser { get; set; }
        /// <summary>
        /// 异常时间
        /// </summary>
        public string Createtime { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        public string Updateuser { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public string Updatetime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [NotMapped]
        public string Starttime { get; set; }


        /// <summary>
        /// 结束时间
        /// </summary>
        [NotMapped]
        public string Endtime { get; set; }


        public void Upd(Abnormal old)
        {
            old.Abnormalname = this.Abnormalname;
            old.Abnormalcase = this.Abnormalcase;
            old.Isdispose = this.Isdispose;
            old.Remark = this.Remark;
            old.Updateuser = this.Updateuser;
            old.Updatetime = this.Updatetime;
        }

    }
}

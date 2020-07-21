﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Db
{
    public partial class SysConfig
    {
        /// <summary>
        /// 序号
        /// </summary>
        public decimal Id { get; set; }
        /// <summary>
        /// 英文名
        /// </summary>
        public string Ekey { get; set; }
        /// <summary>
        /// 中文名
        /// </summary>
        public string Ckey { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string Price { get; set; }


        public void Upd(SysConfig old)
        {
            old.Price = this.Price;

        }
    }
}

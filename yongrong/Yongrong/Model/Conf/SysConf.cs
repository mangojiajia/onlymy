﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Conf
{
    public class SysConf
    {
        /// <summary>
        /// 
        /// </summary>
        public bool ReleaseFlag { get; set; } = false;


        public bool JPushFlag { get; set; } = false;

        /// <summary>
        /// 拦截的IP地址
        /// </summary>
        public List<string> BlockIp { get; set; } = new List<string>();

        /// <summary>
        /// 延迟启动定时任务
        /// </summary>
        public int TimeTaskDueTime { get; set; } = 3000;

        /// <summary>
        /// 定时任务调度周期
        /// </summary>
        public int TimeTaskPeriod { get; set; } = 3000;

        /// <summary>
        /// 每隔12小时刷新Token
        /// </summary>
        public double TokenValidHours { get; set; } = 12.0;

        /// <summary>
        /// 时间戳 间隔1个小时内有效
        /// </summary>
        public double SysTimeSpan { get; set; } = 3600000.0;

        /// <summary>
        /// IC制卡客户端机器的IP地址
        /// </summary>
        public List<string> ICClients { get; set; } = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        public List<string> SaleRole { get; set; } = new List<string>(){ "销售采购部" };

        /// <summary>
        /// 
        /// </summary>
        public List<string> CustomRole { get; set; } = new List<string>() { "销售客户", "供应商" };

        /// <summary>
        /// 
        /// </summary>
        public string PushAppKey { get; set; } = "3c1bcf34f468aeda6456945f";

        /// <summary>
        /// 
        /// </summary>
        public string PushMastSecret { get; set; } = "8b043f9fc8f8e5431cfa27b6";

        /// <summary>
        /// 未进厂的预约,返回预扣量
        /// 2.0是前天
        /// </summary>
        public double UndoOrderReturn { get; set; } = -2.0;

        /// <summary>
        /// 空excel文件路径
        /// </summary>
        public string ExcelTemplet { get; set; } = "";
    }
}

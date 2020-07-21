﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Base
{
    public class CostBean 
    {
        /// <summary>
        /// 进入标志
        /// </summary>
        public const string Enter = "I";


        /// <summary>
        /// 退出标志
        /// </summary>
        public const string Exit = "O";

        /// <summary>
        /// 成功应答
        /// </summary>
        public const string Success = "0";

        /// <summary>
        /// TOKEN
        /// </summary>
        public const string TOKEN = "Token";

        public const string OP_Content = "Opcontent";

        public const string OP_Detail = "Opdetail";

        public const string OP_UserDest = "opuser_dest";

        public const string OP_User = "opuser";
        /// <summary>
        /// 
        /// </summary>
        public const string JsonErr = "请求解析失败";

        public const string ADD = "add";

        public const string UPD = "upd";

        public const string DEL = "del";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string GetOPName(string op)
        {
            switch(op)
            {
                case ADD:
                    return "添加";
                case UPD:
                    return "更新";
                case DEL:
                    return "删除";
                default:
                    return "未知操作";
            }    
        }

        public static string GetCmdName(string cmd)
        {
            switch (cmd)
            {
                case "0202":
                    return "原辅料进厂预约管理";
                case "0204":
                    return "产品出厂预约管理";                    
                case "0206":
                    return "产品退货预约管理";
                case "0301":
                    return "预约信息核实";
                case "0401":
                    return "卸车作业单";
                case "0402":
                    return "装车作业单";
                case "0403":
                    return "退货作业单";
                case "0501":
                    return "预约后台管理";
                case "0502":
                    return "发货区最大车辆数设置";
                case "0503":
                    return "物流环节间隔时间设置";
                case "0601":
                    return "过磅单数据汇总";
                default:
                    return "未知命令";
            }
        }

        public const string ParamNull = "输入参数为空";

        public const string UpdNoRecord = "记录不存在,无法更新";

        public const string AddRecord = "记录已存在,无法新增";

        public const string DelNoRecord = "记录不存在,无法删除";

        public const string OpEmpty = "操作类型不明";
    }
}

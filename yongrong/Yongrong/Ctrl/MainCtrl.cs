﻿using System;
using System.Collections.Generic;
using System.Text;
using BaseS.File.Log;
using Yongrong.Srvc.BaseInfo;
using Yongrong.Srvc.Gate;
using Yongrong.Srvc.Sys;

namespace Yongrong.Ctrl
{
    class MainCtrl : BaseCtrl
    {
        /// <summary>
        /// 
        /// </summary>
        internal static void InitPre()
        { 
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void InitAfter()
        {
            if (Gate.GateFlag && Gate.InitMq)
            {
                GateSrvc.InitConsumer("", "");
            }

            SysConfigSrvc.Init();
            BaseTractorSrvc.Init();
        }

        /// <summary>
        /// 主控制开启
        /// </summary>
        internal static void Start()
        {
            "系统初始化开始".Notice();
            InitPre();
            "初始化完毕,平台接口准备开启!".Warn();

            HttpCtrl.Start();
            TimerCtrl.Start();

            InitAfter();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        internal static void Stop()
        {
            TimerCtrl.Stop();
            HttpCtrl.Stop();

            "平台接口已关闭!".Warn();
        }
    }
}

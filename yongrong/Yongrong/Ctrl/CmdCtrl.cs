﻿using System;
using System.Collections.Generic;
using System.Text;
using BaseS.File.Log;
using BaseS.File.Log.Model;
using BaseS.Security;
using BaseS.String;
using Yongrong.Srvc.Gate;
using Yongrong.Srvc.Push;
using Yongrong.Srvc.Weighbridge;

namespace Yongrong.Ctrl
{
    class CmdCtrl : BaseCtrl
    {
        static CmdCtrl()
        {
            HandleCmdTab.TryAdd("showerror", ShowErr);
            HandleCmdTab.TryAdd("log", SetConsoleLog);
            HandleCmdTab.TryAdd("file", SetFileLog);
            HandleCmdTab.TryAdd("help", ShowHelp);
            HandleCmdTab.TryAdd("gateadd", (a, b) => { GateSrvc.AddCarInfo(new Model.Int.Gate.GateAddCarReq() { PlateNo = "闽A37173", CarType = 1  }); });
            HandleCmdTab.TryAdd("gatedel", (a, b) => { GateSrvc.DelCarInfo(new Model.Int.Gate.GateDelCarReq { PlateNo = "闽A37173" }); });
            HandleCmdTab.TryAdd("gategetparking", (a, b) => { GateSrvc.GetParking(new Model.Int.Gate.GateGetParkingReq ()); });
            HandleCmdTab.TryAdd("gaterecharge1", (a, b) =>
            {
                GateSrvc.RechargeCarInfo(new Model.Int.Gate.GateRechargeCarReq()
                {
                    EndTime = BString.Get1970ToNowMilliseconds(DateTime.Now.AddHours(1.0)),
                    Money = "3.00",
                    ParkUuid = Gate.GateSouthUuid, // 南门出入口
                    PlateNo = "闽A37173",
                    Remark = "智能物流测试_1",
                    StartTime = BString.Get1970ToNowMilliseconds(DateTime.Now)
                });
            });

            HandleCmdTab.TryAdd("gategetcarinfos", (a, b) =>
            {
                GateSrvc.GetCarInfos( new Model.Int.Gate.GateGetCarInfoReq()
                {
                    PlateNo = "闽A37173"
                });
            });

            HandleCmdTab.TryAdd("gaterecharge2", (a, b) =>
            {
                GateSrvc.RechargeCarInfo(new Model.Int.Gate.GateRechargeCarReq()
                {
                    EndTime = BString.Get1970ToNowMilliseconds(DateTime.Now.AddHours(1.0)),
                    Money = "3.00",
                    ParkUuid = Gate.GateNorthUuid, // 北门出入库
                    PlateNo = "闽A37173",
                    Remark = "智能物流测试_2",
                    StartTime = BString.Get1970ToNowMilliseconds(DateTime.Now)
                });
            });

            HandleCmdTab.TryAdd("gategeteventtype", (a, b) => { GateSrvc.GetEventTypes(new Model.Int.Gate.GateGetEventTypeReq()); });

            HandleCmdTab.TryAdd("gatesubscribe", (a, b) =>
            {
                GateSrvc.SubscribeEventsFromMQEx(new Model.Int.Gate.GateSubscribeEventsReq() { EventTypes = "524545,524546" }, out var rsp);
            });

            //
            HandleCmdTab.TryAdd("gateinitmq", (a, b) =>
            {
                GateSrvc.InitConsumer("192.168.1.160:61618", "openapi.pms.topic");
            });

            HandleCmdTab.TryAdd("addbatch", (a, b) =>
             {
                 var dpext = new Yongrong.Model.Int.Weigh.DataParamExt()
                 {
                     Driver = "任苗根",
                     Endtime = DateTime.Now.AddMinutes(20).ToString("yyyy-MM-dd HH:mm:ss"),
                     Msg_id = DateTime.Now.Ticks.ToString(),
                     Order_id = "0004",
                     Order_type = "送货",
                     Starttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                     Tractor_id = "闽A30K52",
                     Trailer_id = "闵B8888挂",
                     Weigh_time = "1"
                 };

                 dpext.Sign = BMD5.ToMD5String(dpext.Order_id + dpext.Msg_id + dpext.Starttime, 32);

                 bool ret = WeighbridgeSrvc.BatchInsert(dpext);
                 Console.WriteLine("addbatch ret=" + ret, ConsoleColor.Red);
             });
            //HandleCmdTab.TryAdd("msgpush", (a, b) => { JPushSrvc.SendPush(string.Empty, IEnumerable<string> useridList); });
        }

        /// <summary>
        /// 监控命令.手动输入
        /// </summary>
        internal static void MonitorCmd()
        {
            string cmd;

            do
            {
                cmd = Console.ReadLine();

                Console.WriteLine("输入命令:" + cmd);

                if (string.IsNullOrWhiteSpace(cmd))
                {
                    Console.WriteLine("命令为空");
                    continue;
                }

                string[] cmdarr = cmd.Trim().Split(' ');

                if (null == cmdarr || 0 == cmdarr.Length)
                {
                    continue;
                }

                ExcProgram(cmd);

            } while (!"exit".Equals(cmd));

            Console.WriteLine("----------------------------------------系统正在结束，清理资源-----------------------------------");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        public static void ExcProgram(string cmd)
        {
            cmd = cmd.ToLower().Trim();

            string[] subcmd = cmd.Split(' ');

            if (HandleCmdTab.TryGetValue(subcmd[0], out Action<string, string> func))
            {
                try
                {
                    func(subcmd[0], cmd);
                }
                catch (Exception e)
                {
                    e.Message.Warn();
                }
            }
            else
            {
                string.Format(@"            showerror:显示错误信息 例如showerror 100
                    log0-6:设置控制台界面输出日志级别
                    file0-5:设置文件输出日志级别
                    sysinf:刷新系统配置文件
                    exit:关闭程序
                    help:获取命令帮助"
                    ).Log(LogLevel.MANUAL);
            }
        }

        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="cmd"></param>
        private static void ShowErr(string cmd, string value)
        {
            value = value.Replace(" ", "");

            int.TryParse(value.Substring(cmd.Length), out int tmp);

            $"错误原因：{tmp}".Log(LogLevel.MANUAL);
        }

        /// <summary>
        /// 设置控制台日志级别
        /// </summary>
        /// <param name="cmd"></param>
        private static void SetConsoleLog(string cmd, string value)
        {
            // 控制台界面级别调整
            value = value.Replace(" ", "");

            byte.TryParse(value.Substring(cmd.Length), out byte level);

            BLog.Level_Console = level;
            $"控制台界面日志级别调整成:{BLog.Level_Console}".Log(LogLevel.MANUAL);
        }

        /// <summary>
        /// 设置文件日志级别
        /// </summary>
        /// <param name="cmd"></param>
        private static void SetFileLog(string cmd, string value)
        {
            // 文件日志级别调整
            value = value.Replace(" ", "");

            byte.TryParse(value.Substring(cmd.Length), out byte level);

            BLog.Level_File = level;

            $"文件日志级别调整成:{BLog.Level_File}".Log(LogLevel.MANUAL);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="value"></param>
        private static void ShowHelp(string cmd, string value)
        {
            StringBuilder tip = new StringBuilder();

            tip.Append("支持的命令:\t");

            foreach (var k in HandleCmdTab.Keys)
            {
                tip.Append(k + "\n\t");
            }

            tip.ToString().Log(LogLevel.MANUAL);
        }

    }
}

﻿using BAspNetCore;
using System;
using System.Collections.Generic;
using System.Text;
using BaseS.File.Log;
using BaseS.Net.Http;
using System.Diagnostics;
using System.Web;
using BaseS.Collection;
using BaseS.String;
using Yongrong.Model.Int;

namespace Yongrong.Ctrl
{
    class HttpCtrl : BaseCtrl
    {
        /// <summary>
        /// http服务对象
        /// </summary>
        static readonly List<BHttpServer5> httplist = new List<BHttpServer5>();

        /// <summary>
        /// 开启Http服务
        /// </summary>
        internal static void Start()
        {
            if (!Http.HttpFlag)
            {
                "Http服务已经关闭!HttpFlag = false".Warn();
                return;
            }

            foreach (var port in Http.Ports ?? new List<int>())
            {
                BHttpServer5 ser = new BHttpServer5()
                {
                    ExecOutAction2 = RecvHttpReq,
                    NginxIpRoute = Http.NginxIpRoute,
                    Port = port
                };

                if (ser.Start())
                {
                    httplist.Add(ser);
                }
                else
                {
                    $"Http服务：{port}开启失败！".Error();
                    ser.Stop();
                    ser = null;
                }
            }
        }

        /// <summary>
        /// 关闭Http服务
        /// </summary>
        internal static void Stop()
        {
            foreach (var ser in httplist)
            {
                ser.Stop();
            }

            httplist.Clear();

            "Http服务已经关闭".Warn();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpBean"></param>
        public static void RecvHttpReq(BHttpBean http)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string cmdStr = http.UrlMap.GetDicStr(Http.CmdName);

            try
            {
                // 需要跨域
                http.IsCross = true;

                // $"客户端:({http.Ip}-{http.IpNginx}) {http.HttpMethod}方式访问URL:({HttpUtility.UrlDecode(http.Url)})".Info(string.Empty, Enter);
                /*
                if ("GET".Equals(http.HttpMethod))
                {
                    http.RspBytes = Encoding.UTF8.GetBytes("");
                    // http.RspBytes = new BaseRsp() { Stat = "HttpMethod 不支持Get" }.ToBytes();
                    return;
                }
                */
                //添加消息来源
                http.UrlMap.TryAdd(Http.ClientIPName, string.IsNullOrWhiteSpace(http.IpNginx) ? http.Ip : http.IpNginx);

                http.RspBytes = ExecInt(cmdStr, http.UrlMap, http.ReqBytes);
            }
            catch (Exception e)
            {
                $"{e.Message} \n {e.StackTrace}".Warn();
            }
            finally
            {
                sw.Stop();

                $"客户端:({http.Ip}-{http.IpNginx}) {http.HttpMethod}方式访问URL:({HttpUtility.UrlDecode(http.Url)}) \n 请求:{http.ReqBytes.B2String()}\n\t应答:{http.RspStr}".Tip($"方法:{cmdStr} 耗时:{sw.ElapsedMilliseconds}ms <<<<<<<", Exit);
            }
        }
    }
}

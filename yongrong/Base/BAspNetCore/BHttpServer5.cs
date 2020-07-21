﻿using BaseS.File;
using BaseS.File.Log;
using BaseS.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using BaseS.Collection;
using System.Web;

namespace BAspNetCore
{
    public class BHttpServer5
    {
        private IWebHost host;

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 服务地址
        /// </summary>
        public IPAddress Address { get; set; }

        /// <summary>
        /// req, rsp, httpmethod, ip, routeip, reqbytes
        /// </summary>
        public Action<BHttpBean> ExecOutAction2 { get; set; }

        /// <summary>
        /// 最大的请求流长度,默认1M
        /// </summary>
        public static int MaxReqContentLen { get; set; } = 1048576;

        /// <summary>
        /// 通过nignx路由的参数名称
        /// </summary>
        public string NginxIpRoute { get; set; }

        /// <summary>
        /// 开启http服务
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            if (null == Address)
            {
                Address = IPAddress.Any;
            }

            host = new WebHostBuilder().UseKestrel((option) =>
            {
                option.Listen(this.Address, this.Port,
                    listenOption =>
                    {
                    });
            }).Configure(app =>
            {
                app.Run(ProcessAsync);
            }).Build();

            host.Start();

            $"Http服务 IPAddress.Any Port:{Port} 启动".Warn();

            return true;
        }

        /// <summary>
        /// 关闭Http服务
        /// </summary>
        public void Stop()
        {
            // 强制关闭内部执行空任务
            ExecOutAction2 = null;

            // 等待之前接收的请求处理
            Thread.Sleep(1000);

            host?.StopAsync().Wait();
        }

        /// <summary>
        /// 处理异步请求
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        async Task ProcessAsync(HttpContext context)
        {
            try
            {
                byte[] reqBytes = null;

                // 特殊跨域处理
                if ("OPTIONS".Equals(context.Request.Method))
                {
                    context.Response.StatusCode = 200;

                    context.Request.Headers.TryGetValue("Origin", out var origin);

                    //跨域 返回参数
                    context.Response.Headers.Add("Access-Control-Allow-Method", "POST, GET, OPTIONS, DELETE");
                    context.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, No-Cache, X-Requested-With, If-Modified-Since, Pragma, Last-Modified, Cache-Control, Expires, Content-Type, X-E4M-With, Authorization, Token, Uid");
                    context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                    context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    context.Response.Headers.Add("Access-Control-Max-Age", "3600");
                    context.Response.Headers.Add("Access-Control-Expose-Headers", "token,uid");

                    context.Response.ContentType = "text/html; charset=utf-8";

                    return;
                }

                // post方式先开始读取请求流中的数据,最大支持MaxReqContentLen
                if ("POST".Equals(context.Request.Method))
                {
                    int cLen = MaxReqContentLen >= (context.Request.ContentLength ?? 0) ? (int)(context.Request.ContentLength ?? 0) : MaxReqContentLen;

                    reqBytes = new byte[cLen];

                    if (1 <= cLen)
                    {
                        reqBytes = BStream.ReadStreamBytes(context.Request.Body);
                    }
                }

                BHttpBean httpObj = new BHttpBean()
                {
                    HttpMethod = context.Request.Method,
                    Ip = context.Connection.RemoteIpAddress?.ToString(),
                    Url = context.Request.Host + context.Request.Path + context.Request.QueryString.Value
                };

                // 是否nginx代理,代理中传递原始IP的参数名称是否为空
                if (!string.IsNullOrWhiteSpace(NginxIpRoute))
                {
                    context.Request?.Headers?.TryGetValue(NginxIpRoute, out var routeIp);

                    httpObj.IpNginx = routeIp;
                }

                // 获取请求Url地址中的参数
                if (null != context.Request.Query && 1 <= context.Request.Query.Count)
                {
                    foreach (var k in context.Request.Query.Keys)
                    {
                        context.Request.Query.TryGetValue(k, out var v);
                        httpObj.UrlMap.TryAdd(k, v);
                    }
                }

                // 请求包头部分
                if (null != context.Request.Headers)
                {
                    foreach (var kv in context.Request.Headers)
                    {
                        httpObj.UrlMap.TryAdd(kv.Key, kv.Value);
                    }
                }

                if ("POST".Equals(httpObj.HttpMethod))
                {
                    httpObj.ReqBytes = reqBytes;
                }

                if (null != ExecOutAction2)
                {
                    ExecOutAction2(httpObj);

                    if (httpObj.IsCross)
                    {
                        context.Request.Headers.TryGetValue("Origin", out var origin);

                        //跨域 返回参数
                        context.Response.Headers.Add("Access-Control-Allow-Method", "OPTIONS,POST,GET");
                        context.Response.Headers.Add("Access-Control-Allow-Headers", "x-requested-with");
                        if (httpObj.UrlMap.ContainsKey("Access-Control-Allow-Origin"))
                        {
                            context.Response.Headers.Add("Access-Control-Allow-Origin", httpObj.UrlMap.GetDicStr("Access-Control-Allow-Origin", "*"));
                        }
                        else
                        {
                            context.Response.Headers.Add("Access-Control-Allow-Origin", origin);
                        }

                        context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                        context.Response.ContentType = "text/html; charset=utf-8";
                    }

                    // 导出文件方式
                    if (!string.IsNullOrWhiteSpace(httpObj.UrlMap.GetDicStr(BHttpBean.ContentDisposition)))
                    {
                        context.Response.Headers.Add(BHttpBean.ContentDisposition, httpObj.UrlMap.GetDicStr(BHttpBean.ContentDisposition));
                    }

                    // 设置ContentType
                    if (!string.IsNullOrWhiteSpace(httpObj.UrlMap.GetDicStr(BHttpBean.ContentType)))
                    {
                        context.Response.ContentType = httpObj.UrlMap.GetDicStr(BHttpBean.ContentType);
                    }

                    if (null == httpObj.RspBytes)
                    {
                        await context.Response.WriteAsync("");
                    }
                    else
                    {
                        await context.Response.Body.WriteAsync(httpObj.RspBytes, 0, httpObj.RspBytes.Length);
                    }
                }
                else
                {
                    await context.Response.WriteAsync("");
                }
            }
            catch (Exception e)
            {
                $"Err:{e.Message} {e.StackTrace}".Warn();
            }
        }
    }
}

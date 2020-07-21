﻿using BaseS.File;
using BaseS.File.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace BaseS.Net.Http
{
    public class BHttpServerBase
    {
        /// <summary>
        /// HTTP 协议侦听器 
        /// </summary>
        private HttpListener httpListener;

        /// <summary>
        /// 待处理的请求数
        /// </summary>
        protected int httpRecvedNum = 0;

        /// <summary>
        /// http请求缓存
        /// </summary>
        protected ConcurrentQueue<HttpListenerContext> contextQueue
            = new ConcurrentQueue<HttpListenerContext>();

        /// <summary>
        /// 当前http处理的线程数
        /// </summary>
        protected int currentParallel = 0;

        /// <summary>
        /// 准备关闭标志
        /// </summary>
        protected bool isClosing = false;

        /// <summary>
        /// 
        /// </summary>
        protected Action Started { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected Action Received { get; set; }

        /// <summary>
        /// 正在停止的操作
        /// </summary>
        protected Action Stoped { get; set; }

        /// <summary>
        /// 最大并发数 默认8
        /// </summary>
        public static int HttpMaxParallel { get; set; } = 8;

        /// <summary>
        /// 初始线程数
        /// </summary>
        public int HttpMinParaller { get; set; } = 4;

        /// <summary>
        /// 通过nignx路由的参数名称
        /// </summary>
        public string NginxIpRoute { get; set; }

        /// <summary>
        /// 外部的执行方法
        /// </summary>
        public Action<HttpListenerContext> ExecOutAction { get; set; }

        /// <summary>
        /// req, rsp, httpmethod, ip, routeip, reqbytes
        /// </summary>
        public Action<BHttpBean> ExecOutAction2 { get; set; }

        /// <summary>
        /// 最大空闲连接超时秒数
        /// </summary>
        public int HttpMaxIdleConnection { get; set; } = 30;

        /// <summary>
        /// 
        /// </summary>
        public BHttpServerBase()
        {
            ServicePointManager.DefaultConnectionLimit = 256;
        }

        /// <summary>
        /// 开启接收http请求
        /// </summary>
        /// <param name="isSync">是否同步</param>
        private void BeginRecvHttp(bool isSync = false)
        {
            if (isSync)
            {
                HttpListenerContext context = httpListener.GetContext();

                // 存入请求队列中
                if (null != context)
                {
                    contextQueue.Enqueue(context);
                    Interlocked.Increment(ref httpRecvedNum);
                }

                BeginRecvHttp(true);
            }
            else
            {
                // 异常情况下不需要进行获取侦听
                if (isClosing || null == httpListener || !httpListener.IsListening)
                {
                    "Http服务停止!".Error();
                    return;
                }

                try
                {
                    httpListener.BeginGetContext(new AsyncCallback(EndRecvHttp), httpListener);
                }
                catch (HttpListenerException he)
                {
                    if (null != httpListener && httpListener.IsListening)
                    {
                        ("Http服务异常停止!Message:" + he.Message + " ErrorCode:" + he.ErrorCode).Error();
                    }
                }
                catch (ObjectDisposedException oe)
                {
                    if (null != httpListener && httpListener.IsListening)
                    {
                        ("Http服务异常停止!" + oe.Message + oe.StackTrace).Error();
                    }
                }
                catch (InvalidOperationException ie)
                {
                    if (null != httpListener && httpListener.IsListening)
                    {
                        ("Http服务异常停止!" + ie.Message + ie.StackTrace).Error();
                    }
                }
                catch (Exception e)
                {
                    if (null != httpListener && httpListener.IsListening)
                    {
                        ("Http服务异常停止!" + e.Message + e.StackTrace).Error();
                    }
                }
            }
        }

        /// <summary>
        /// 接收完成一个异步http请求
        /// </summary>
        /// <param name="result"></param>
        private void EndRecvHttp(IAsyncResult result)
        {
            HttpListenerContext context = null;

            try
            {
                context = httpListener.EndGetContext(result);

                $"接收完成Http请求{context.Request.RemoteEndPoint.Address}".Info();
            }
            catch (Exception e)
            {
                if (!isClosing && null != httpListener && httpListener.IsListening)
                {
                    ("Message:" + e.Message + " StackTrace:" + e.StackTrace).Warn();
                }
            }

            BeginRecvHttp(false);

            if (null == context)
            {
                return;
            }

            // 存入请求队列中
            contextQueue.Enqueue(context);

            Interlocked.Increment(ref httpRecvedNum);

            Received?.Invoke();

        }

        /// <summary>
        /// Http服务开始
        /// </summary>
        /// <param name="url">URL地址</param>
        public bool Start(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                "url地址为空，请配置url地址".Warn();
                return false;
            }

            // 已经开启，不启动
            if (null != httpListener && httpListener.IsListening)
            {
                "Http服务已经开启，不要重复开启！".Warn();
                return false;
            }

            if (null == ExecOutAction && null == ExecOutAction2)
            {
                "Http执行方法 ExecOutAction 为空!".Warn();
                return false;
            }

            isClosing = false;

            string urltotal = string.Empty;

            if (null == httpListener)
            {
                httpListener = new HttpListener
                {
                    // 匿名方式
                    AuthenticationSchemes = AuthenticationSchemes.Anonymous,
                };

                // 设置最大空闲链接时间
                httpListener.TimeoutManager.IdleConnection = new TimeSpan(0, 0, HttpMaxIdleConnection);

                foreach (string singleurl in url.Split(',').Where(a => !string.IsNullOrWhiteSpace(a)))
                {
                    try
                    {
                        httpListener.Prefixes.Add(singleurl);
                        urltotal += singleurl;
                    }
                    catch (Exception e)
                    {
                        (singleurl + " - " + e.Message + e.StackTrace).Warn();
                    }
                }
            }

            // 开启http服务
            try
            {
                httpListener.Start();

                string.Format("Http服务：{0}开启成功", urltotal).Tip();

                Started?.Invoke();

                BeginRecvHttp(false);
            }
            catch (HttpListenerException he)
            {
                if (!isClosing)
                {
                    string errmsg = "ErrorCode:" + he.ErrorCode + " Message:" + he.Message + " StackTrace:" + he.StackTrace;
                    errmsg.Error();
                }

                string.Format("Http服务：{0}开启失败", urltotal).Warn();
                return false;
            }
            catch (ObjectDisposedException oe)
            {
                if (!isClosing)
                {
                    oe.Message.Error();
                }
                string.Format("Http服务：{0}开启失败", urltotal).Warn();
                return false;
            }
            catch (InvalidOperationException ie)
            {
                if (!isClosing)
                {
                    (ie.Message + ie.StackTrace).Error();
                }
                string.Format("Http服务：{0}开启失败", urltotal).Warn();
                return false;
            }
            catch (Exception e)
            {
                if (!isClosing)
                {
                    (e.Message + e.StackTrace).Error();
                }

                string.Format("Http服务：{0}开启失败", urltotal).Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 停止Http服务
        /// </summary>
        public void Stop()
        {
            string urltotal = string.Empty;

            try
            {
                foreach (string url in httpListener.Prefixes)
                {
                    urltotal += url;
                }
            }
            catch (Exception e)
            {
                ("Message:" + e.Message + " StackTrace:" + e.StackTrace).Warn();
            }

            (urltotal + " http服务准备关闭").Warn();

            if (null != httpListener && httpListener.IsListening)
            {
                try
                {
                    httpListener.Stop();
                }
                catch (Exception)
                {

                }

                Thread.Sleep(300);

                try
                {
                    httpListener.Close();
                }
                catch (Exception)
                {

                }
            }

            isClosing = true;

            Thread.Sleep(100);

            Stoped?.Invoke();


            "主线程已经完成清理".Tip();

            if (null != httpListener)
            {
                try
                {
                    httpListener.Abort();
                }
                catch (Exception)
                {
                }

                Thread.Sleep(70);
                httpListener = null;
            }

            ("http服务已经关闭").Warn();
        }

        /// <summary>
        /// 关闭HttpListenerRequest
        /// </summary>
        /// <param name="req"></param>
        protected static void CloseReq(HttpListenerRequest req)
        {
            /*
            if (null == req)
            {
                return;
            }

            req = null;
            */
        }

        /// <summary>
        /// 关闭HttpListenerResponse
        /// </summary>
        /// <param name="rsp"></param>
        protected static void CloseRsp(HttpListenerResponse rsp)
        {
            if (null == rsp)
            {
                return;
            }

            try
            {
                rsp.Close();
            }
            catch (Exception e)
            {
                ("HttpListenerResponse close Message:" + e.Message + " StackTrace:" + e.StackTrace).Warn();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void RunHttpTask(object _)
        {
            HttpListenerContext httpContext = null;

            while (!contextQueue.IsEmpty)
            {
                try
                {
                    if (!contextQueue.TryDequeue(out httpContext)
                        || null == httpContext)
                    {
                        continue;
                    }

                    Interlocked.Decrement(ref httpRecvedNum);

                    if (null != ExecOutAction)
                    {
                        ExecOutAction(httpContext);
                    }
                    else if (null != ExecOutAction2) // 第二种模式
                    {
                        HttpListenerRequest req = httpContext.Request;
                        HttpListenerResponse rsp = httpContext.Response;
                        string origin = string.Empty;

                        BHttpBean reqbean = new BHttpBean()
                        {
                            HttpMethod = req.HttpMethod,
                            Ip = req.RemoteEndPoint.Address.ToString(),
                            Url = req.Url.ToString()
                        };

                        //获取url中带有参数
                        foreach (string key in req.QueryString.AllKeys)
                        {
                            if (!reqbean.UrlMap.ContainsKey(key))
                            {
                                reqbean.UrlMap.Add(key, req.QueryString[key]);
                                continue;
                            }

                            reqbean.UrlMap[key] = req.QueryString[key];
                        }

                        if (!string.IsNullOrWhiteSpace(NginxIpRoute))
                        {
                            reqbean.IpNginx = req?.Headers?.GetValues(NginxIpRoute)?.First();
                        }

                        if (!"GET".Equals(reqbean.HttpMethod))
                        {
                            reqbean.ReqBytes = BStream.ReadStreamBytes(req.InputStream);
                        }

                        ExecOutAction2(reqbean);

                        if (reqbean.IsCross)
                        {
                            origin = httpContext.Request.Headers?.Get("Origin");
                        }

                        rsp.SendHttpRsp(reqbean.RspBytes, origin, reqbean.IsCross);
                    }
                }
                catch (Exception e)
                {
                    $"Message:{e.Message} StackTrace:{e.StackTrace}".Warn();
                }
                finally
                {
                    try
                    {
                        BStream.CloseStream(httpContext?.Request?.InputStream);

                        if (null != httpContext?.Response)
                        {
                            BStream.CloseStream(httpContext.Response.OutputStream);
                            CloseRsp(httpContext.Response);
                        }
                    }
                    catch (Exception e)
                    {
                        $"Message:{e.Message} StackTrace:{e.StackTrace}".Warn();
                    }
                }
            }

            if (contextQueue.IsEmpty)
            {
                Interlocked.Exchange(ref httpRecvedNum, 0);
            }

            Interlocked.Decrement(ref currentParallel);
        }

    }

    public class BHttpBean
    {
        /// <summary>
        /// 系统默认的
        /// </summary>
        public static Encoding DefCoding { get; set; } = Encoding.UTF8;

        private static byte[] EmptyBytes = new byte[0];

        private byte[] rspBytes;

        private string rspStr;

        public string HttpMethod { get; set; }

        /// <summary>
        /// 调用方IP
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 通过nginx透传的ip,原始ip
        /// </summary>
        public string IpNginx { get; set; }

        /// <summary>
        /// 请求访问地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 请求对象流
        /// </summary>
        public byte[] ReqBytes { get; set; }

        /// <summary>
        /// 应答对象
        /// </summary>
        public byte[] RspBytes
        {
            get
            {
                return rspBytes;
            }
            set
            {
                rspBytes = value;

                if (null != rspBytes)
                {
                    rspStr = DefCoding.GetString(value);
                }
                else
                {
                    rspStr = string.Empty;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public const string ContentDisposition = "Content-Disposition";

        /// <summary>
        /// 
        /// </summary>
        public const string ContentType = "contenttype";

        /// <summary>
        /// 
        /// </summary>
        public string RspStr 
        {
            get 
            {
                return rspStr; 
            } 
            set
            {
                rspStr = value;

                if (!string.IsNullOrWhiteSpace(value))
                {
                    rspBytes = DefCoding.GetBytes(value);
                }
                else
                {
                    rspBytes = EmptyBytes;
                }
            }
        }

        /// <summary>
        /// 是否需要跨域
        /// </summary>
        public bool IsCross { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> UrlMap { get; set; } = new Dictionary<string, string>();
    }
}

using BaseS.File;
using BaseS.File.Log;
using BaseS.String;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaseS.Net.Websock
{
    public class BWebSocket
    {
        /// <summary>
        /// HTTP 协议侦听器
        /// </summary>
        private HttpListener httpListener;

        /// <summary>
        /// 等待关闭
        /// </summary>
        private bool isClosing = false;

        /// <summary>
        /// websocket 当前的序号
        /// </summary>
        private int websocketSeq = 0;

        /// <summary>
        /// 待处理的queue
        /// </summary>
        private readonly ConcurrentQueue<HttpListenerContext> contextQueue = new ConcurrentQueue<HttpListenerContext>();

        /// <summary>
        /// 接收到的WebSocket对象表
        /// key: seq 序号
        /// </summary>
        private readonly ConcurrentDictionary<int, WsBean> wsTab = new ConcurrentDictionary<int, WsBean>();

        /// <summary>
        /// 队列有效长度
        /// </summary>
        private int queueLen = 0;

        /// <summary>
        /// WebSocket 主线程锁对象
        /// </summary>
        private readonly object threadlocker = new object();

        /// <summary>
        /// WebSocket 主线程
        /// </summary>
        private Thread contextThread;

        /// <summary>
        /// Url地址
        /// </summary>
        public List<string> Urls { get; set; } = new List<string>();

        /// <summary>
        /// nginx 路由ip
        /// </summary>
        public string NginxIpRoute { get; set; }

        /// <summary>
        /// 接收到WebSocket 数据
        /// </summary>
        public Action<WsBean, byte[], int> RecvWsDataAction { get; set; }

        /// <summary>
        /// WebSocket的客户端接入时间
        /// </summary>
        public Action<WsBean> RecvWsConnectAction { get; set; }

        /// <summary>
        /// 关闭WebSocket链接
        /// </summary>
        public Action<WsBean> CloseWsAction { get; set; }

        /// <summary>
        /// 接收包最大长度
        /// </summary>
        public int RecvMaxPackSize { get; set; } = 1024 * 16;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public BWebSocket(params string[] url)
        {
            contextThread = new Thread(StartWebSocket);
            Urls.AddRange(url);
        }

        /// <summary>
        /// 开启WebSocket
        /// </summary>
        public void Start()
        {
            httpListener = new HttpListener();

            if (null == RecvWsDataAction)
            {
                "启动失败,RecvWsAction未设置".Error();
                return;
            }

            string urls = string.Empty;

            foreach (string u in Urls)
            {
                httpListener.Prefixes.Add(u);
                urls += u + " ";
            }

            $"WebSocket服务端开启侦听:{urls}等待客户端接入".Tip();

            try
            {
                httpListener.Start();
            }
            catch (HttpListenerException he)
            {
                he.Message.Warn();
            }

            contextThread.Start();

            BeginRecvHttp();
        }

        /// <summary>
        /// 关闭WebSocket
        /// </summary>
        public void Stop()
        {
            isClosing = true;

            Thread.Sleep(300);

            try
            {
                httpListener.Stop();
            }
            catch(ObjectDisposedException ode)
            {
                ode.Message.Warn();
            }
            catch(Exception e)
            {
                e.Message.Warn();
            }
        }

        /// <summary>
        /// 开启接收http请求
        /// </summary>
        /// <param name="isSync">是否同步</param>
        private void BeginRecvHttp()
        {
            httpListener.BeginGetContext(EndRecvHttp, null);
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

                contextQueue.Enqueue(context);

                if (1 == Interlocked.Increment(ref queueLen))
                {
                    lock (threadlocker)
                    {
                        Monitor.Pulse(threadlocker);
                    }
                }
            }
            catch (Exception e)
            {
                if (!isClosing)
                {
                    $"Message:{e.Message} StackTrace:{e.StackTrace}".Warn();
                }
            }
            finally
            {
                if (!isClosing)
                {
                    BeginRecvHttp();
                }
            }
        }

        /// <summary>
        /// 开启侦听WebSocket
        /// </summary>
        private void StartWebSocket()
        {
            "开启侦听WebSocket".Debug(string.Empty, "Enter");

            HttpListenerContext context = null;

            try
            {
                do
                {
                    if (contextQueue.TryDequeue(out context))
                    {
                        Interlocked.Decrement(ref queueLen);

                        WsBean ws = new WsBean()
                        {
                            Seq = Interlocked.Increment(ref websocketSeq),
                            //Wsocket = wscontext.WebSocket,
                            Context = context,
                            RemoteIp = context.Request.RemoteEndPoint.Address.ToString()
                        };

                        // 如果是代理ip,则获取真实ip地址
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(NginxIpRoute)
                                && null != context.Request.Headers.GetValues(NginxIpRoute)
                                && ws.Context.Request.Headers.GetValues(NginxIpRoute).Any())
                            {
                                ws.RemoteIp = ws.Context.Request.Headers.GetValues(NginxIpRoute).First();
                            }
                        }
                        catch (Exception e)
                        {
                            e.Message.Warn();
                        }

                        $"接收到编号:[{ws.Seq}] Http客户端:{ws.RemoteIp}".Info();

                        wsTab.AddOrUpdate(ws.Seq, ws, (k, v) => ws);

                        ThreadPool.QueueUserWorkItem(AcceptWebSocket, ws);
                    }

                    if (contextQueue.IsEmpty)
                    {
                        if (!isClosing)
                        {
                            lock (threadlocker)
                            {
                                Monitor.Wait(threadlocker);
                            }
                        }
                    }
                } while (!isClosing);


            }
            catch (Exception e)
            {
                e.Message.Warn();
            }
           
            "结束侦听WebSocket".Debug(string.Empty, "Exit");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        private async void AcceptWebSocket(object obj)
        {
            WsBean ws = obj as WsBean;

            try
            {
                HttpListenerWebSocketContext wscontext = await ws.Context.AcceptWebSocketAsync(null);

                ws.Wsocket = wscontext.WebSocket;

                RecvWsConnectAction?.Invoke(ws);

                RecvBytes(ws);
            }
            catch (WebSocketException we)
            {
                CloseWs(ws);
                we.Message.Warn();
            }
            catch (Exception e)
            {
                CloseWs(ws);
                e.Message.Warn();
            }
            finally
            {

            }
        }

        /// <summary>
        /// 接收WebSocket的数据
        /// </summary>
        /// <param name="ws"></param>
        private async void RecvBytes(WsBean ws)
        {
            byte[] reqbytes = new byte[RecvMaxPackSize]; // 包体最大长度8K 不能超过该包长度
            ArraySegment<byte> recvbuff = new ArraySegment<byte>(reqbytes);

            WebSocketReceiveResult result = null;

            try
            {
                result = await ws.Wsocket.ReceiveAsync(recvbuff, CancellationToken.None);
            }
            catch (Exception e)
            {
                $"Err:{e.Message} StackTrace:{e.StackTrace}".Debug();

                wsTab.TryRemove(ws.Seq, out WsBean tmp);

                CloseWs(tmp);
                return;
            }

            // 状态不正常
            if (WebSocketMessageType.Close == result.MessageType)
            {
                "WebSocket接收到关闭消息".Info();
                CloseWs(ws);
                return; //获取的消息是关闭，跳出循环
            }

            RecvWsDataAction(ws, reqbytes, result.Count);

            // 系统不关闭下,循环接收
            if (!isClosing)
            {
                RecvBytes(ws);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="ws"></param>
        public void CloseWs(WsBean ws)
        {
            if (null == ws)
            {
                return;
            }

            $"断开 编号:{ws.Seq} IP:{ws.RemoteIp} 的链接".Tip();

            CloseWsAction?.Invoke(ws);

            wsTab.TryRemove(ws.Seq, out WsBean tmp);

            if (null != ws.Wsocket)
            {
                if (WebSocketState.Closed != ws.Wsocket.State)
                {
                    try
                    {
                        Task t = ws.Wsocket.CloseAsync(WebSocketCloseStatus.Empty, string.Empty, CancellationToken.None);
                        t.Wait(300);
                    }
                    catch (Exception e)
                    {
                        $"Err:{e.Message} StackTrace:{e.StackTrace}".Warn();
                    }

                    try
                    {
                        ws.Wsocket.Abort();
                    }
                    catch (Exception e)
                    {
                        $"Err:{e.Message} StackTrace:{e.StackTrace}".Warn();
                    }

                    try
                    {
                        ws.Wsocket.Dispose();
                    }
                    catch (Exception e)
                    {
                        $"Err:{e.Message} StackTrace:{e.StackTrace}".Warn();
                    }
                }
            }

            if (null != ws.Context)
            {
                if (null != ws.Context.Response)
                {
                    try
                    {
                        ws.Context.Response.Close();
                    }
                    catch (Exception e)
                    {
                        $"Err:{e.Message} StackTrace:{e.StackTrace}".Warn();
                    }
                }
            }
        }
    }
}

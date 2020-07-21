using BaseS.File;
using BaseS.File.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace BaseS.Net.Http
{
    public class BHttpServer : BHttpServerBase
    {
        /// <summary>
        /// 主线程
        /// </summary>
        private readonly Thread mainThread;

        /// <summary>
        /// 主线程锁
        /// </summary>
        private readonly object mainlocker = new object();

        /// <summary>
        /// 
        /// </summary>
        private readonly ManualResetEvent manualReset = new ManualResetEvent(false);

        /// <summary>
        /// 
        /// </summary>
        public BHttpServer()
        {
            mainThread = new Thread(RunHttpTask1)
            {
                IsBackground = true
            };

            base.Started = StartBody;
            base.Received = ReceivedBody;
            base.Stoped = StopedBody;
        }
        
        /// <summary>
        /// 开启主线程
        /// </summary>
        private void StartBody()
        {
            mainThread.Start();
        }

        /// <summary>
        /// 接收到完整Http请求
        /// </summary>
        private void ReceivedBody()
        {
            if (1 == httpRecvedNum)
            {
                lock (mainlocker)
                {
                    Monitor.Pulse(mainlocker);
                }
            }

            // 队列中还有http请求,开启额外线程处理
            if (currentParallel < HttpMaxParallel)
            {
                Interlocked.Increment(ref currentParallel);
                ThreadPool.QueueUserWorkItem(RunHttpTask);
            }
        }

        /// <summary>
        /// 关闭时执行的方法
        /// </summary>
        private void StopedBody()
        {
            lock (mainlocker)
            {
                Monitor.Pulse(mainlocker);
            }

            "等待主线程处理完请求".Tip();

            manualReset.WaitOne();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RunHttpTask1()
        {
            do
            {
                RunHttpTask(null);

                if (contextQueue.IsEmpty)
                {
                    if (1 <= httpRecvedNum)
                    {
                        Interlocked.Exchange(ref httpRecvedNum, 0);
                    }

                    try
                    {
                        if (!isClosing)
                        {
                            lock (mainlocker)
                            {
                                Monitor.Wait(mainlocker);
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        (e.Message + e.StackTrace).Warn();
                    }
                }
            } while (!isClosing);

            manualReset.Set();
        }  
    }
}

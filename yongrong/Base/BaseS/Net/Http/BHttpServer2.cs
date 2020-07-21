using BaseS.File;
using BaseS.File.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace BaseS.Net.Http
{
    public class BHttpServer2 : BHttpServerBase
    {
        public BHttpServer2()
        {
            this.Received = ReceivedBody;
        }

        /// <summary>
        /// 接收到完整Http请求
        /// </summary>
        private void ReceivedBody()
        {
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
        }
    }
}

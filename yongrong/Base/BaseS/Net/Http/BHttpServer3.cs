using BaseS.File.Log;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BaseS.Net.Http
{
    public class BHttpServer3 : BHttpServerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private static List<Thread> threads = new List<Thread>();

        /// <summary>
        /// 
        /// </summary>
        private ManualResetEvent manualReset = new ManualResetEvent(false);

        /// <summary>
        /// 主线程锁
        /// </summary>
        private readonly object mainlocker = new object();

        /// <summary>
        /// 
        /// </summary>
        public BHttpServer3()
        {
            for (int i = 1; i <= HttpMaxParallel; i++)
            {
                Thread thread = new Thread(RunHttpTask1)
                {
                    Name = $"Http{i}",
                    IsBackground = true
                    //Priority = ThreadPriority.AboveNormal
                };

                thread.Start();
                threads.Add(thread);
            }

            this.Received = ReceivedBody;
            this.Stoped = StopBody;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ReceivedBody()
        {
            if (currentParallel < HttpMaxParallel)
            {
                lock (mainlocker)
                {
                    Monitor.Pulse(mainlocker);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void RunHttpTask1()
        {
            do
            {
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
                    catch (Exception e)
                    {
                        (e.Message + e.StackTrace).Warn();
                    }
                }

                Interlocked.Increment(ref currentParallel);

                RunHttpTask(null);
            } while (!isClosing);
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopBody()
        {
            lock (mainlocker)
            {
                Monitor.PulseAll(mainlocker);
            }

            foreach (var t in threads)
            {
                try
                {
                    if (ThreadState.Stopped != t.ThreadState)
                    {
                        t.Abort();
                    }
                }
                catch (Exception)
                {
                    // e.Message.Warn();
                }
            }
        }
    }
}

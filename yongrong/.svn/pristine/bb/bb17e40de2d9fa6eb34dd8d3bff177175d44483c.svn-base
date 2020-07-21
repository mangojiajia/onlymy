using BaseS.File.Log.Model;
using BaseS.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;

namespace BaseS.File.Log
{
    public static partial class BLog
    {
        /// <summary>
        /// 保存到缓存队列
        /// </summary>
        /// <param name="level"></param>
        /// <param name="msg"></param>
        /// <param name="obj"></param>
        /// <param name="member"></param>
        /// <param name="file"></param>
        /// <param name="line"></param>
        private static void SaveToBuffer(byte level, string msg, object obj, string member, string file, int line, object id = null, string titleext = "", string msgext = "", string outfile = "")
        {
            if (!IsShow(level))
            {
                return;
            }

            if (!logInQueue.TryDequeue(out LogBean log))
            {
                log = new LogBean();
            }

            if (file.Contains("\\"))
            {
                file = file.Substring(file.LastIndexOf("\\") + 1);
            }

            log.Time = DateTime.Now;
            log.Level = level;
            log.Content = msg;
            log.Member = member;
            log.ThreadId = Thread.CurrentThread.ManagedThreadId;
            log.IsThreadPool = Thread.CurrentThread.IsThreadPoolThread;
            log.Priority = Thread.CurrentThread.Priority.ToString();
            log.ThreadName = Thread.CurrentThread.Name;
            log.File = file;
            log.Line = line;
            log.Id = id ?? string.Empty;
            log.Obj = obj;
            log.TitileExt = titleext;
            log.ContentExt = msgext;
            log.OutFile = outfile;

            logOutQueue.Enqueue(log);

            if (stat == LogStat.Running || stat == LogStat.Busy)
            {
                return;
            }

            lock (loglocker)
            {
                Monitor.Pulse(loglocker);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="message"></param>
        private static bool Log2File(this string message)
        {
            // 按小时记录日志
            if (!IsDayFile)
            {
                if ((DateTime.Now.Hour != curHour) || string.IsNullOrWhiteSpace(curFilePath))
                {
                    curFilePath = LogRoot + "/" + DateTime.Now.ToString("yyyyMMddHH") + ".log";
                    curHour = DateTime.Now.Hour;
                }
            }

            // 按天记录日志
            if (IsDayFile)
            {
                if (DateTime.Now.Day != curDay || string.IsNullOrWhiteSpace(curFilePath))
                {
                    curFilePath = LogRoot + "/" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                    curDay = DateTime.Now.Day;
                    curFileSize = 0;
                }

                if (0 == curFileSize)
                {
                    try
                    {
                        if (System.IO.File.Exists(curFilePath))
                        {
                            FileInfo fs = new FileInfo(curFilePath);

                            curFileSize = fs.Length;
                        }
                        else
                        {
                            curFileSize = 0;
                        }
                    }
                    catch (Exception)
                    {
                        curFileSize = 0;
                    }
                }

                // 创建一个新文件
                if (DayFileMaxSize <= curFileSize)
                {
                    curFileSize = 0;

                    try
                    {
                        System.IO.File.Move(curFilePath, curFilePath + "." + DateTime.Now.ToString("HHmmss"));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + e.StackTrace);
                    }
                }

                curFileSize += message.Length;
            }

            try
            {
                System.IO.File.AppendAllText(curFilePath, message, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Start()
        {
            if (!Directory.Exists(LogRoot))
            {
                Directory.CreateDirectory(LogRoot);
            }

            fileThread.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Stop()
        {
            stat = LogStat.Closing;

            // 强制关闭日志屏幕输出
            if (LogLevel.OFF > Level_Console)
            {
                Level_File = LogLevel.OFF;
                Level_Console = LogLevel.OFF;
                Level_Out = LogLevel.OFF;

                Console.WriteLine("强制关闭Console输出");
            }

            if (!logOutQueue.IsEmpty)
            {
                lock (loglocker)
                {
                    Monitor.Pulse(loglocker);
                }

                "日志正在关闭中".Warn();
                Thread.Sleep(300);

                if (!logOutQueue.IsEmpty)
                {
                    Thread.Sleep(700);

                    if (!logOutQueue.IsEmpty)
                    {
                        "日志强制退出,可能存在日志丢失情况".Warn();
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">不使用</param>
        private static void BeginToSaveLog(object _)
        {
            StringBuilder singleSB = new StringBuilder(MAX_SINGLE_SIZE);
            StringBuilder fileSB = new StringBuilder(File_Output_Size + MAX_SINGLE_SIZE);

            Thread.Sleep(137);

            while (!logOutQueue.IsEmpty)
            {
                if (!logOutQueue.TryDequeue(out LogBean tmplog))
                {
                    continue;
                }

                tmplog.ToStringBuilder(singleSB);

                // 文件日志输出
                if (tmplog.Level >= Level_File)
                {
                    fileSB.Append(singleSB);

                    if (File_Output_Size < fileSB.Length)
                    {
                        Log2File(fileSB.ToString());
                        fileSB.Clear();
                        Thread.Sleep(23);
                    }
                }

                CheckBusy();

                if ((LogStat.Busy != stat))
                {
                    // 控制台日志输出
                    if (tmplog.Level >= Level_Console)
                    {
                        ShowConsole(tmplog, singleSB);
                    }

                    // 外部日志
                    if (tmplog.Level >= Level_Out && null != ShowLogExt)
                    {
                        ShowLogExt.BeginInvoke(tmplog.Level, tmplog.Id, singleSB.ToString(), null, null);
                    }   
                }

                if (logInQueue.Count <= LogBufferMaxSize)
                {
                    tmplog.Clear();
                    logInQueue.Enqueue(tmplog);
                }
            }

            // 文件剩余日志输出
            if (1 <= fileSB.Length)
            {
                Log2File(fileSB.ToString());
                fileSB.Clear();
            }
        }

        /// <summary>
        /// 写日志线程
        /// </summary>
        private static void WriteLogTask()
        {
            do
            {
                stat = LogStat.Waiting;

                Thread.Sleep(10);

                lock (loglocker)
                {
                    Monitor.Wait(loglocker);
                }

                stat = LogStat.Running;

                try
                {
                    BeginToSaveLog(null);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"{e.Message} {e.StackTrace}");
                }
            } while (LogStat.Closing != stat);
        }

        /// <summary>
        /// 检查系统是否忙碌
        /// </summary>
        private static void CheckBusy()
        {
            if ((LogStat.Busy == stat) && 3.0 >= (DateTime.Now - lastBusyTime).TotalSeconds)
            {
                return;
            }

            // 忙碌状态下定期提示日志已经被放通
            if (Max_ConsoleLine <= logOutQueue.Count(a => a.Level > Level_Console))
            {
                stat = LogStat.Busy;

                if (ConsoleColor.Magenta != Console.ForegroundColor)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                }

                Console.WriteLine($"{DateTime.Now.ToString()} {lastBusyTime.ToString()} {stat} - 输出日志过多!进入放通模式!具体日志请查看文件! 队列长度:{logOutQueue.Count} Thread:{Thread.CurrentThread.ManagedThreadId}");

                lastBusyTime = DateTime.Now;
            }
            else
            {
                stat = LogStat.Running;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tmplog"></param>
        /// <param name="lastConsoleTime"></param>
        private static void ShowConsole(LogBean tmplog, StringBuilder msgSb)
        {
            // 关键字匹配
            if (!string.IsNullOrWhiteSpace(FilterId) && FilterId.Equals(tmplog.Id))
            {
                switch (tmplog.Level)
                {
                    case LogLevel.DEBUG:
                    case LogLevel.INFO:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case LogLevel.TIP:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case LogLevel.NOTICE:
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        break;
                    case LogLevel.WARN:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogLevel.ERROR:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogLevel.MANUAL:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    case LogLevel.FORCE:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }

                Console.Write(msgSb.ToString());
                return;
            }

            // 过滤内容 模糊匹配
            if (!string.IsNullOrWhiteSpace(FilterContent)
                && msgSb.ToString().Contains(FilterContent))
            {
                switch (tmplog.Level)
                {
                    case LogLevel.DEBUG:
                    case LogLevel.INFO:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case LogLevel.TIP:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case LogLevel.NOTICE:
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        break;
                    case LogLevel.WARN:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogLevel.ERROR:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogLevel.MANUAL:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                    case LogLevel.FORCE:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }

                Console.Write(msgSb.ToString());
                return;
            }

            switch (tmplog.Level)
            {
                case LogLevel.DEBUG:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.INFO:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.TIP:
                case LogLevel.TIP2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.NOTICE:
                case LogLevel.NOTICE2:
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case LogLevel.WARN:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.MANUAL:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case LogLevel.ERROR:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case LogLevel.FORCE:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }

            Console.Write(msgSb.ToString());
        }
    }
}

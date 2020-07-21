using BaseS.File.Log.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace BaseS.File.Log
{
    public static partial class BLog
    {
        /// <summary>
        /// 文件字符集
        /// </summary>
        public static Encoding FileCoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 过滤ID,全匹配中情况下显示
        /// </summary>
        public static string FilterId { get; set; } = string.Empty;

        /// <summary>
        /// 过滤内容,模糊匹配
        /// </summary>
        public static string FilterContent { get; set; } = string.Empty;

        /// <summary>
        /// 输出信息给外部的方法
        /// </summary>
        public static Action<byte, object, string> ShowLogExt { get; set; } = null;

        /// <summary>
        /// 控制台输出级别
        /// </summary>
        public static byte Level_Console { get; set; } = LogLevel.DEBUG;

        /// <summary>
        /// 日志文件输出级别
        /// </summary>
        public static byte Level_File { get; set; } = LogLevel.DEBUG;

        /// <summary>
        /// 日志输出路径
        /// </summary>
        public static string LogRoot { get; set; }

        /// <summary>
        /// 外部日志级别
        /// </summary>
        public static byte Level_Out { get; set; } = byte.MaxValue;

        /// <summary>
        /// 是否是单日一个日志文件
        /// </summary>
        public static bool IsDayFile { get; set; } = true;

        /// <summary>
        /// 单个日文件大小
        /// </summary>
        public static int DayFileMaxSize { get; set; } = 97517568;

        /// <summary>
        /// 文件单次最大输出 
        /// 默认2M
        /// </summary>
        public static int File_Output_Size { get; set; } = 2097152;

        /// <summary>
        /// 最大控制台显示记录条数
        /// </summary>
        public static int Max_ConsoleLine { get; set; } = 173;

        /// <summary>
        /// 最大缓存对象
        /// </summary>
        public static int LogBufferMaxSize { get; set; } = 3000;

        /// <summary>
        /// 日志对象缓存
        /// </summary>
        private static readonly ConcurrentQueue<LogBean> logInQueue = new ConcurrentQueue<LogBean>();

        /// <summary>
        /// 日志输出缓存
        /// </summary>
        private static readonly ConcurrentQueue<LogBean> logOutQueue = new ConcurrentQueue<LogBean>();

        /// <summary>
        /// 单次内容长度
        /// </summary>
        private const int MAX_SINGLE_SIZE = 1024;

        /// <summary>
        /// 日志当前并行度1
        /// </summary>
        private static readonly object loglocker = new object();

        /// <summary>
        /// 日志模块当前状态
        /// </summary>
        private static LogStat stat = LogStat.Init;

        /// <summary>
        /// 当前文件路径
        /// </summary>
        private static string curFilePath;

        /// <summary>
        /// 最近一次小时
        /// </summary>
        private static int curHour = DateTime.Now.Hour;

        /// <summary>
        /// 
        /// </summary>
        private static int curDay = DateTime.Now.Day;

        /// <summary>
        /// 当前文件长度
        /// </summary>
        private static long curFileSize = 0;

        /// <summary>
        /// 
        /// </summary>
        private static readonly Thread fileThread = new Thread(WriteLogTask) { IsBackground = true, Name = "BLogThread" };

        /// <summary>
        /// 
        /// </summary>
        private static DateTime lastBusyTime = DateTime.Now;

        /// <summary>
        /// 
        /// </summary>
        static BLog()
        {
            IsWinOS = System.Environment.OSVersion.Platform.ToString().Contains("Win");

            if (string.IsNullOrWhiteSpace(LogRoot))
            {
                LogRoot = Environment.CurrentDirectory + (IsWinOS ? @"\log" : @"/log");
            }

            for (int i = 0; i < 3000; i++)
            {
                logInQueue.Enqueue(new LogBean());
            }
        }

        /// <summary>
        /// 是否是Windows系统
        /// </summary>
        public static bool IsWinOS { get; private set; }

        /// <summary>
        /// 是否需要显示
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static bool IsShow(byte level)
        {
            return (level >= Level_File) || (level >= Level_Console) || (level >= Level_Out);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Debug(this string msg,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.DEBUG, msg, null, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Debug<T>(this T obj,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
             [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.DEBUG, null, obj, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Info(this string msg,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.INFO, msg, null, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Info<T>(this T obj,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
             [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.INFO, null, obj, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 重要提示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Tip(this string msg,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.TIP, msg, null, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 重要提示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Tip<T>(this T obj,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
             [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.TIP, null, obj, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 重要提示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Notice(this string msg,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.NOTICE, msg, null, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 重要提示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Notice<T>(this T obj,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
             [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.NOTICE, null, obj, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 告警类型 系统出错，但还可以运行
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Warn(this string msg,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.WARN, msg, null, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 告警类型 系统出错，但还可以运行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Warn<T>(this T obj,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
             [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.WARN, null, obj, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 错误类型 导致系统无法正常运行级别
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Error(this string msg,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.ERROR, msg, null, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 错误类型 导致系统无法正常运行级别
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Error<T>(this T obj,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
             [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.ERROR, null, obj, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 强制类型 导致系统无法正常运行级别
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Force(this string msg,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.FORCE, msg, null, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 错误类型 导致系统无法正常运行级别
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Force<T>(this T obj,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
             [CallerFilePath] string sourceFilePath = "",
             [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(LogLevel.FORCE, null, obj, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="level"></param>
        /// <param name="id"></param>
        /// <param name="titleext"></param>
        /// <param name="msgext"></param>
        /// <param name="outfile"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Log(this string msg,
            byte level = LogLevel.DEBUG,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(level, msg, null, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }

        /// <summary>
        /// 错误类型 导致系统无法正常运行级别
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="id"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public static void Log<T>(this T obj,
            byte level = LogLevel.DEBUG,
            object id = null,
            string titleext = "",
            string msgext = "",
            string outfile = "",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            SaveToBuffer(level, null, obj, memberName, sourceFilePath, sourceLineNumber, id, titleext, msgext, outfile);
        }
    }
}

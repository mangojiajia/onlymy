using System;
using System.Collections.Generic;
using System.Text;

namespace BaseS.File.Log.Model
{
    public static class LogLevel
    {
        /// <summary>
        /// 程序正常部分日志
        /// </summary>
        public const byte DEBUG = 0;
        public const byte INFO = 1;
        public const byte TIP = 2;
        public const byte TIP2 = 3;

        /// <summary>
        /// 重要通知部分
        /// </summary>
        public const byte NOTICE = 4;
        public const byte NOTICE2 = 5;

        /// <summary>
        /// 告警\错误\人为操作
        /// </summary>
        public const byte WARN = 6;
        public const byte MANUAL = 7;
        public const byte ERROR = 8;

        public const byte FORCE = byte.MaxValue;
        public const byte OFF = byte.MaxValue;

        /// <summary>
        /// 日志级别数组
        /// </summary>
        public static string[] Tips { get; set; }
        
        static LogLevel()
        {
            Tips = new string[byte.MaxValue + 1];

            for (int i = 0; i < Tips.Length; i++)
            {
                Tips[i] = string.Empty;
            }

            Tips[DEBUG] = "<debug>  ";
            Tips[INFO] = "<info>   ";
            Tips[TIP] = "<tip>   ";
            Tips[TIP2] = "<tip>   ";

            Tips[NOTICE] = "<notice>  ";
            Tips[NOTICE2] = "<notice>  ";

            Tips[WARN] = "<warn>  ";
            Tips[MANUAL] = "<manual>";
            Tips[ERROR] = "<error>  ";
            
            Tips[FORCE] = "<force>  ";
        }
    }

    
}

using BaseS.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseS.File.Log.Model
{
    internal class LogBean
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Time;

        /// <summary>
        /// 日志等级
        /// </summary>
        public byte Level;

        /// <summary>
        /// 调用文件
        /// </summary>
        public string File;

        /// <summary>
        /// 线程编号
        /// </summary>
        public int ThreadId;

        /// <summary>
        /// 是否是线程池
        /// </summary>
        public bool IsThreadPool;

        /// <summary>
        /// 优先级
        /// </summary>
        public string Priority;

        /// <summary>
        /// 
        /// </summary>
        public string ThreadName;

        /// <summary>
        /// 调用方法名
        /// </summary>
        public string Member;

        /// <summary>
        /// 所在行数
        /// </summary>
        public int Line;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content;

        /// <summary>
        /// 日志对象
        /// </summary>
        public object Obj;

        /// <summary>
        /// 日志的身份 用于过滤
        /// </summary>
        public object Id;

        /// <summary>
        /// 标题扩展
        /// </summary>
        public string TitileExt;

        /// <summary>
        /// 内容扩展
        /// </summary>
        public string ContentExt;

        /// <summary>
        /// 输出文件路径
        /// </summary>
        public string OutFile;

        /// <summary>
        /// 重置对象内容
        /// </summary>
        public void Clear()
        {
            Id = string.Empty;
            Time = DateTime.MinValue;
            File = string.Empty;
            Member = string.Empty;
            Content = string.Empty;
            Level = LogLevel.DEBUG;
            Obj = null;
            ThreadId = -1;
            Line = -1;
            IsThreadPool = false;
            Priority = string.Empty;
            ThreadName = string.Empty;
            TitileExt = string.Empty;
            ContentExt = string.Empty;
            OutFile = string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void ToStringBuilder(StringBuilder singleSB)
        {
            if (null == singleSB)
            {
                return;
            }

            string jsonStr = string.Empty;

            if (null != Obj)
            {
                if (!BJson.ObjToJson(Obj, ref jsonStr))
                {
                    $"{(File.TrimEnd('c', 's') + Member).PadRight(44) + ("L:" + Line).PadRight(7)}------异常json".Warn(string.Empty);
                }
            }

            singleSB.Clear();

            singleSB.Append(LogLevel.Tips[Level])
                .Append(" [").Append(Time.ToString("MM-dd HH:mm:ss fff")).Append("] ")
                .Append((File.TrimEnd('c', 's') + Member).PadRight(44))
                .Append(("L:" + Line).PadRight(7))
                .Append(("<" + TitileExt + ">"))
                
                .Append(" T:").Append(ThreadId).Append('-').Append(Priority).Append('-').Append(IsThreadPool).Append('-').Append(ThreadName)

                .Append(" ID:(").Append(Id).Append(")")
                
                .AppendLine()                       
                .Append('\t').Append(Content).AppendLine(jsonStr);

            if (!string.IsNullOrWhiteSpace(ContentExt))
            {
                singleSB.Append("\t[").Append(ContentExt).AppendLine("]");
            }
        }
    }
}

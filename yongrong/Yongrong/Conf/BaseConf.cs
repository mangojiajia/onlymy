using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Base;
using Yongrong.Model.Conf;
using BaseS.File.Log;
using BaseS.File;
using BaseS.Serialization;

namespace Yongrong.Conf
{
    public class BaseConf : CostBean
    {
        /// <summary>
        /// 手动处理命令字表
        /// </summary>
        protected static Dictionary<string, Action<string, string>> HandleCmdTab { get; set; }
            = new Dictionary<string, Action<string, string>>();

        /// <summary>
        /// 系统配置
        /// </summary>
        public static SysConf Sys { get; set; }
        
        /// <summary>
        /// http配置
        /// </summary>
        public static HttpConf Http { get; set; }

        /// <summary>
        /// 地磅配置
        /// </summary>
        public static WeighbridgeConf Weigh { get; set; }

        /// <summary>
        /// 门禁配置
        /// </summary>
        public static GateConf Gate { get; set; }

        /// <summary>
        /// 初始化任务
        /// </summary>
        protected static Action InitAction { get; set; }

        static BaseConf()
        {
            if (null == Sys)
            {
                FlashSys();
            }

            if (null == Http)
            {
                FlashHttp();
            }

            if (null == Weigh)
            {
                FlashWeigh();
            }

            if (null == Gate)
            {
                FlashGate();
            }

            HandleCmdTab.TryAdd("flashsys", (c1, c) => { FlashSys(); });
            HandleCmdTab.TryAdd("flashhttp", (c1, c) => { FlashHttp(); });
            HandleCmdTab.TryAdd("flashweight", (c1, c) => { FlashWeigh(); }); 
            HandleCmdTab.TryAdd("flashgate", (c1, c) => { FlashGate(); });
        }

        /// <summary>
        /// 刷新配置文件基本方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="bean"></param>
        protected static bool FlashConfig<T>(string filePath, ref T bean)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                $"文件不存在!路径:{filePath}".Notice(string.Empty, Exit);
                return false;
            }

            string filecontent = string.Empty;

            BFile.Txt2String(filePath, ref filecontent);

            BJson.JsonToObjT<T>(filecontent, ref bean);

            return null != bean;
        }

        /// <summary>
        /// 刷新系统配置项
        /// </summary>
        /// <returns></returns>
        public static bool FlashSys()
        {
            SysConf t = null;

            bool ret = FlashConfig<SysConf>(@"Conf/SysConf.txt", ref t);

            if (null != t)
            {
                Sys = t;
            }

            return ret;
        }

        /// <summary>
        /// 刷新http配置
        /// </summary>
        /// <returns></returns>
        public static bool FlashHttp()
        {
            HttpConf t = null;

            bool ret = FlashConfig<HttpConf>(@"Conf/HttpConf.txt", ref t);

            if (null != t)
            {
                Http = t;
            }

            return ret;
        }

        /// <summary>
        /// 刷新地磅配置
        /// </summary>
        /// <returns></returns>
        public static bool FlashWeigh()
        {
            WeighbridgeConf t = null;

            bool ret = FlashConfig<WeighbridgeConf>(@"Conf/WeighbridgeConf.txt", ref t);

            if (null != t)
            {
                if (null == t.WeighIp)
                {
                    t.WeighIp = new List<string>();
                }

                Weigh = t;
            }

            return ret;
        }

        /// <summary>
        /// 刷新门禁配置
        /// </summary>
        /// <returns></returns>
        public static bool FlashGate()
        {
            GateConf t = null;

            bool ret = FlashConfig<GateConf>(@"Conf/GateConf.txt", ref t);

            if (null != t)
            {
                Gate = t;
            }

            return ret;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Yongrong.Model.Base
{
    public class GateCode
    {
        private readonly static Dictionary<int, string> codeTab
             = new Dictionary<int, string>();

        /// <summary>
        /// 
        /// </summary>
        static GateCode()
        {
            codeTab.TryAdd(0, "接口调用成功，并正常返回");
            codeTab.TryAdd(1000, "系统异常");
            codeTab.TryAdd(1001, "API参数无效");
            codeTab.TryAdd(1002, "appkey无效");
            codeTab.TryAdd(1003, "time时间戳无效");
            codeTab.TryAdd(1004, "token无效");
            codeTab.TryAdd(1005, "平台服务异常");

            codeTab.TryAdd(1006, "API接口异常");
            codeTab.TryAdd(1007, "版本不兼容");
            codeTab.TryAdd(1008, "内部接口调用超时");
            codeTab.TryAdd(1009, "内部接口不存在");
        }

        /// <summary>
        /// 获取门禁返回码说明
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string Get(int code)
        {
            codeTab.TryGetValue(code, out var result);

            return result;
        }
    }
}

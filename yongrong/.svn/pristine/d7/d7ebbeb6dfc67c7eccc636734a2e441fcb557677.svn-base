using BaseS.File.Log;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseS.Collection
{
    public static class BDic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqMap"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string GetDicStr(this Dictionary<string, string> reqMap, string paramName, string defVal = "")
        {
            if (null == reqMap || string.IsNullOrWhiteSpace(paramName))
            {
                $"请求参数错误,reqMap paramName:{paramName} 其中之一为空".Info("", "O");
                return string.Empty;
            }

            if (!reqMap.TryGetValue(paramName, out var paramVal))
            {
                return defVal;
            }

            return paramVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqMap"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static long GetDicLong(this Dictionary<string, string> reqMap, string paramName, long defVal = 0,bool isLog=false)
        {
            if (null == reqMap || string.IsNullOrWhiteSpace(paramName))
            {
                $"请求参数错误,reqMap paramName:{paramName} 其中之一为空".Info("", "O");
                return defVal;
            }

            if (!reqMap.TryGetValue(paramName, out var paramVal))
            {
                $"获取参数:{paramName} 获取值失败,使用默认值:{defVal}".Info("", "O");
                return defVal;
            }

            if (!long.TryParse(paramVal, out var paramLong))
            {
                if (isLog)
                {
                    $"获取参数:{paramName} 值为:{paramVal} 转换成long型失败,使用默认值:{defVal}".Info("", "O");
                }
                return defVal;
            }

            return paramLong;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqMap"></param>
        /// <param name="paramName"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static int GetDicInt(this Dictionary<string, string> reqMap, string paramName, int defVal = 0)
        {
            if (null == reqMap || string.IsNullOrWhiteSpace(paramName))
            {
                $"请求参数错误,reqMap paramName:{paramName} 其中之一为空".Info("", "O");
                return defVal;
            }

            if (!reqMap.TryGetValue(paramName, out var paramVal))
            {
                $"获取参数:{paramName} 获取值失败,使用默认值:{defVal}".Info("", "O");
                return defVal;
            }

            if (!int.TryParse(paramVal, out var paramInt))
            {
                $"获取参数:{paramName} 值为:{paramVal} 转换成int型失败,使用默认值:{defVal}".Info("", "O");

                return defVal;
            }

            return paramInt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqMap"></param>
        /// <param name="paramName"></param>
        /// <param name="defVal"></param>
        /// <returns></returns>
        public static bool GetDicBool(this Dictionary<string, string> reqMap, string paramName, bool defVal = false)
        {
            if (null == reqMap || string.IsNullOrWhiteSpace(paramName))
            {
                $"请求参数错误,reqMap paramName:{paramName} 其中之一为空".Info("", "O");
                return defVal;
            }

            if (!reqMap.TryGetValue(paramName, out var paramVal))
            {
                $"获取参数:{paramName} 获取值失败,使用默认值:{defVal}".Info("", "O");
                return defVal;
            }

            if (!bool.TryParse(paramVal, out var paramBool))
            {
                $"获取参数:{paramName} 值为:{paramVal} 转换成bool型失败,使用默认值:{defVal}".Info("", "O");

                return defVal;
            }

            return paramBool;
        }
    }
}

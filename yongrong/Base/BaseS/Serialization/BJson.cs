﻿using BaseS.Const;
using BaseS.File.Log;
using BaseS.String;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace BaseS.Serialization
{
    public static class BJson
    {
        /// <summary>
        /// json默认字符集
        /// </summary>
        public static Encoding JsonCoding = Encoding.UTF8;

        /// <summary>
        /// 日常格式的时间
        /// </summary>
        public static Regex comDateReg = new Regex(@"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}");

        /// <summary>
        /// json格式的时间
        /// </summary>
        public static Regex jsonDateReg = new Regex(@"\\/Date\((\d+)\+\d+\)\\/");

        const string NewLine = "\n";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public static string String2JSON(this string tmp)
        {
            StringBuilder info = new StringBuilder();

            for (int i = 0; i < tmp.Length; i++)
            {
                char t = tmp[i];

                switch (t)
                {
                    case '\"':
                        info.Append("\\\"");
                        break;
                    case '\\':
                        info.Append("\\\\");
                        break;
                    case '/':
                        info.Append("\\/");
                        break;
                    case '\b':
                        info.Append("\\b");
                        break;
                    case '\f':
                        info.Append("\\f");
                        break;
                    case '\n':
                        info.Append("\\n");
                        break;
                    case '\r':
                        info.Append("\\r");
                        break;
                    case '\t':
                        info.Append("\\t");
                        break;
                    default:
                        info.Append(t);
                        break;
                }
            }

            return info.ToString();
        }

        public static string JsonTime(this string json)
        {
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);

            string jsonString = comDateReg.Replace(json, matchEvaluator);

            return jsonString;
        }

        #region 反序列化
        /// <summary>
        /// 将Json字符串转换成T类型对象
        /// </summary>
        /// <typeparam name="T">模板</typeparam>
        /// <param name="szJson"></param>
        /// <returns></returns>
        public static bool JsonToObjT<T>(this string szJson, ref T bean, Type type = null, Encoding encoder = null, bool timeChange = false)
        {
            if (string.IsNullOrWhiteSpace(szJson))
            {
                return false;
            }

            if (szJson.Contains(NewLine))
            {
                szJson = szJson.Replace(NewLine, string.Empty);
            }

            encoder = encoder ?? JsonCoding;

            return JsonToObjT<T>(encoder.GetBytes(szJson), ref bean, type,timeChange);
        }

        public static bool Json2ObjT<T>(this string json, out T obj, Type type = null, Encoding encoding = null, bool timeChange = false)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                obj = default;
                return false;
            }

            if (null == type)
            {
                type = typeof(T);
            }

            encoding = encoding ?? JsonCoding;

            if (json.Contains(NewLine))
            {
                json = json.Replace(NewLine, string.Empty);
            }

            byte[] jsonBytes = encoding.GetBytes(json);

            object tmp = null;

            bool ret = JsonToObj(jsonBytes, ref tmp, 0, jsonBytes.Length, type, timeChange);
            
            obj = (T)tmp;

            return ret;
        }

        /// <summary>
        /// 将Json字符串转换成T类型对象
        /// </summary>
        /// <param name="szJson"></param>
        /// <param name="bean"></param>
        /// <returns></returns>
        public static bool JsonToObjT<T>(this string szJson, ref dynamic bean, Type type = null, Encoding encoder = null, bool timeChange = false)
        {
            if (string.IsNullOrWhiteSpace(szJson))
            {
                return false;
            }

            if (szJson.Contains(NewLine))
            {
                szJson = szJson.Replace(NewLine, string.Empty);
            }

            encoder = encoder ?? JsonCoding;

            return JsonToObjT<T>(encoder.GetBytes(szJson), ref bean, type, timeChange);
        }

        public static bool Json2ObjT<T>(this string szJson, out dynamic bean, Type type = null, Encoding encoder = null, bool timeChange = false)
        {
            if (string.IsNullOrWhiteSpace(szJson))
            {
                bean = default;
                return false;
            }

            if (null == type)
            {
                type = typeof(T);
            }

            encoder = encoder ?? JsonCoding;

            if (szJson.Contains(NewLine))
            {
                szJson = szJson.Replace(NewLine, string.Empty);
            }

            byte[] jsonBytes = encoder.GetBytes(szJson);
            object tmp = null;

            bool ret = JsonToObj(jsonBytes, ref tmp, 0, jsonBytes.Length, type, timeChange);

            bean = (T)tmp;

            return ret;
        }


        /// <summary>
        /// 将Json字符串转换成T类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteArr"></param>
        /// <param name="Bean"></param>
        /// <returns></returns>
        public static bool JsonToObj(this string szJson, ref dynamic bean, Type type = null, Encoding encoder = null, bool timeChange = false)
        {
            if (string.IsNullOrWhiteSpace(szJson))
            {
                return false;
            }

            if (szJson.Contains(NewLine))
            {
                szJson = szJson.Replace(NewLine, string.Empty);
            }

            encoder = encoder ?? JsonCoding;

            bool ret = JsonToObj(encoder.GetBytes(szJson), ref bean, type, timeChange);

            return ret;
        }
        
        /// <summary>
        /// 将Json字符串转换成T类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteArr"></param>
        /// <param name="Bean"></param>
        /// <returns></returns>
        public static bool JsonToObjT<T>(this byte[] byteArr, ref T bean, Type type = null, bool timeChange = false)
        {
            object tmp = bean;

            if (null == type)
            {
                type = typeof(T);
            }

            if (null == byteArr || 0 == byteArr.Length)
            {
                return false;
            }

            if (byteArr.B2String().Contains(NewLine))
            {
                byteArr = JsonCoding.GetBytes(byteArr.B2String(JsonCoding).Replace(NewLine, string.Empty));
            }

            bool ret = JsonToObj(byteArr, ref tmp, 0, byteArr.Length, type, timeChange);

            bean = (T)tmp;

            return ret;
        }
      
        public static bool Json2ObjT<T>(this byte[] byteArr, out T obj, Type type = null, Encoding encoding = null, bool timeChange = false)
        {
            if (null == type)
            {
                type = typeof(T);
            }

            if (byteArr.B2String().Contains(NewLine))
            {
                byteArr = JsonCoding.GetBytes(byteArr.B2String(JsonCoding).Replace(NewLine, string.Empty));
            }

            object tmp = null;

            bool ret = JsonToObj(byteArr, ref tmp, 0, byteArr.Length, type, timeChange);

            obj = (T)tmp;

            return ret;
        }

        /// <summary>
        /// 将Json字符串转换成T类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteArr"></param>
        /// <param name="Bean"></param>
        /// <returns></returns>
        public static bool JsonToObjT<T>(this byte[] byteArr, ref dynamic bean, Type type = null, bool timeChange = false)
        {
            if (null == type)
            {
                type = typeof(T);
            }

            if (byteArr.B2String().Contains(NewLine))
            {
                byteArr = JsonCoding.GetBytes(byteArr.B2String(JsonCoding).Replace(NewLine, string.Empty));
            }

            return JsonToObj(byteArr, ref bean, 0, byteArr.Length, type, timeChange);
        }

        public static bool Json2ObjT<T>(this byte[] byteArr, out dynamic bean, Type type = null, Encoding encoder = null, bool timeChange = false)
        {
            if (null == type)
            {
                type = typeof(T);
            }
            object obj = null;

            encoder = encoder ?? JsonCoding;

            if (byteArr.B2String(encoder).Contains(NewLine))
            {
                byteArr = encoder.GetBytes(byteArr.B2String(encoder).Replace(NewLine, string.Empty));
            }

            bool ret = JsonToObj(byteArr, ref obj, 0, byteArr.Length, type, timeChange);

            bean = obj;

            return ret;
        }

        /// <summary>
        /// 将Json字符串转换成T类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteArr"></param>
        /// <param name="Bean"></param>
        /// <returns></returns>
        public static bool JsonToObj(this byte[] byteArr, ref dynamic bean, Type type = null, bool timeChange = false)
        {
            if (null == type && bean is null)
            {
                return false;
            }

            if (byteArr.B2String(JsonCoding).Contains(NewLine))
            {
                byteArr = JsonCoding.GetBytes(byteArr.B2String(JsonCoding).Replace(NewLine, string.Empty));
            }

            return JsonToObj(byteArr, ref bean, 0, byteArr.Length, type, timeChange);
        }

        /// <summary>
        /// 将Json字符串转换成T类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteArr"></param>
        /// <param name="startPos"></param>
        /// <param name="count"></param>
        /// <param name="bean"></param>
        /// <returns></returns>
        public static bool JsonToObj(this byte[] byteArr, ref object bean, int startPos = 0, int count = -1, Type type = null, bool timeChange = false)
        {
            bool ret = false;
            
            if (null == byteArr)
            {
                bean = null;
                return false;
            }

            if (0 >= count)
            {
                count = byteArr.Length;
            }

            if (-1 >= startPos
                || startPos + count > byteArr.Length)
            {
                bean = null;
                return false;
            }

            if (null == type)
            {
                if (null != bean)
                {
                    type = bean.GetType();
                }
                else
                {
                    return false;
                }
            }

            try
            {
#if COMMON
                using (MemoryStream ms = new MemoryStream(byteArr, startPos, count))
                 {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);

                    bean = serializer.ReadObject(ms);

                    ret = true;
                 }
#else
                byte[] byteArr2 = byteArr;

                if (timeChange)
                {
                    string jsonString = JsonTime(Encoding.UTF8.GetString(byteArr, startPos, count));

                    byteArr2 = Encoding.UTF8.GetBytes(jsonString);

                    startPos = 0;
                    count = byteArr2.Length;
                }

                try
                {
                    // json串种含有大量非ANSI的字符
                    using (var jsonReader = JsonReaderWriterFactory.CreateJsonReader(byteArr2, startPos, count, XmlDictionaryReaderQuotas.Max))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);

                        bean = serializer.ReadObject(jsonReader);

                        ret = true;
                    }
                }
                catch(Exception e)
                {
                    e.Message.Warn();

                    var str = Encoding.UTF8.GetString(byteArr2);
                    
                    if (str.Contains(@"\"))
                    {
                        $"包含json的特殊字符,准备二次转换".Notice();

                        str = str.Replace(@"\", "");

                        byteArr2 = Encoding.UTF8.GetBytes(str);
                        count = byteArr2.Length;

                        using (var jsonReader = JsonReaderWriterFactory.CreateJsonReader(byteArr2, startPos, count, XmlDictionaryReaderQuotas.Max))
                        {
                            DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);

                            bean = serializer.ReadObject(jsonReader);

                            ret = true;
                        }
                    }

                }
#endif
            }
            catch (Exception e)
            {
                bean = null;

                string err = $"Exception:{e.Message} StackTrace:{e.StackTrace} Json:{byteArr.B2String()}";

                // 需要分开写,因为使用了反射,会导致问题
                err.Warn();
            }

            return ret;
        }
#endregion

#region 序列化
        /// <summary>
        /// 将T类型对象生成Json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ObjToJsonT<T>(this T obj, ref string jsonString, Type t = null, Encoding encoder = null, bool timeChange = false)
        {
            byte[] byteArr = null;

            bool ret = ObjToJsonT<T>(obj, ref byteArr, t, encoder, timeChange);

            encoder = encoder ?? JsonCoding;

            if (ret && null != byteArr)
            {
                jsonString = byteArr.B2String(encoder);
            }

            return ret;
        }

        public static bool Obj2JsonT<T>(this T obj, out string jsonString, Type t = null, Encoding encoder = null, bool timeChange = false)
        {
            byte[] byteArr = null;

            bool ret = ObjToJsonT<T>(obj, ref byteArr, t,encoder, timeChange);

            encoder = encoder ?? JsonCoding;

            jsonString = byteArr.B2String(encoder);

            return ret;
        }

        /// <summary>
        /// 将T类型对象生成Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static bool ObjToJson(this object obj, ref string jsonString, Type t = null, Encoding encoder = null, bool timeChange = false)
        {
            bool ret = false;
            byte[] byteArr = null;

            ret = ObjToJson(obj, ref byteArr, t,encoder, timeChange);

            encoder = encoder ?? JsonCoding;

            if (ret && null != byteArr)
            {
                jsonString = byteArr.B2String(encoder);
            }

            return ret;
        }

        public static bool Obj2Json(this object obj, out string jsonString, Type t = null, Encoding encoder = null, bool timeChange = false)
        {
            bool ret = false;
            byte[] byteArr = null;

            ret = ObjToJson(obj, ref byteArr, t, encoder, timeChange);

            encoder = encoder ?? JsonCoding;

            jsonString = byteArr.B2String(encoder);
            
            return ret;
        }

        /// <summary>
        /// 将T类型对象生成Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="byteArr"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool ObjToJson(this object obj, ref byte[] byteArr, Type t = null, Encoding encoder = null, bool timeChange = false)
        {
            bool ret = false;

            if (obj is null)
            {
                return false;
            }

            if (null == t)
            {
                if (obj is null)
                {
                    return false;
                }

                t = obj.GetType();
            }

            if (null == encoder)
            {
                encoder = Encoding.UTF8;
            }

            DataContractJsonSerializer json = new DataContractJsonSerializer(t);


            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, obj);

                    byteArr = stream.ToArray();

                    if (timeChange)
                    {
                        //替换Json的Date字符串    
                        MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
                        string jsonString = jsonDateReg.Replace(encoder.GetString(byteArr), matchEvaluator);
                        byteArr = encoder.GetBytes(jsonString);
                    }

                    ret = true;
                }
            }
            catch (Exception e)
            {
                string err = "ObjToJson Err! " + e.Message + " dynamic:" + t.Name + "\n StackTrace:" + e.StackTrace;
                ret = false;
                err.Warn();
            }

            return ret;
        }

        /// <summary>
        /// 将T类型对象生成Json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="byteArr"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool ObjToJsonT<T>(this T obj, ref byte[] byteArr, Type t = null, Encoding encoder = null, bool timeChange = false)
        {
            if (null == t)
            {
                t = typeof(T);
            }

            return ObjToJson(obj, ref byteArr, t, encoder, timeChange);
        }

        public static bool Obj2JsonT<T>(this T obj, out byte[] byteArr, Type t = null, Encoding encoder = null, bool timeChange = false)
        {
            if (null == t)
            {
                t = typeof(T);
            }

            byte[] tmpBytes = null;

            bool ret = ObjToJson(obj, ref tmpBytes, t, encoder, timeChange);

            byteArr = tmpBytes;

            return ret;
        }
        #endregion

        /// <summary>
        /// json特殊字符串
        /// </summary>
        /// <param name="srcJson"></param>
        /// <returns></returns>
        public static string String2Json(string srcJson)
        {
            if (string.IsNullOrWhiteSpace(srcJson))
            {
                return string.Empty;
            }

            StringBuilder dstJson = new StringBuilder(377);

            foreach (char c in srcJson.ToCharArray())
            {
                switch (c)
                {
                    case '\"':
                        dstJson.Append("\\\"");
                        break;
                    case '\\':
                        dstJson.Append("\\\\");
                        break;
                    case '/':
                        dstJson.Append("\\/");
                        break;
                    case '\b':
                        dstJson.Append("\\b");
                        break;
                    case '\f':
                        dstJson.Append("\\f");
                        break;
                    case '\n':
                        dstJson.Append("\\n");
                        break;
                    case '\r':
                        dstJson.Append("\\r");
                        break;
                    case '\t':
                        dstJson.Append("\\t");
                        break;
                    default:
                        //在ASCⅡ码中，第0～31号及第127号(共33个)是控制字符或通讯专用字符
                        if ((c >= 0 && c <= 31) || c == 127)
                        {

                        }
                        else
                        {
                            dstJson.Append(c);
                        }
                        break;
                }
            }

            return dstJson.ToString();
        }

        /// <summary>    
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串    
        /// </summary>    
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);

            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString(BTip.TimeFormater);

            return result;
        }

        /// <summary>    
        /// 将时间字符串转为Json时间    
        /// </summary>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);

            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);

            return result;
        }
    }
}

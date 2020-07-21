using BaseS.File.Log;
using BaseS.String;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace BaseS.Serialization
{
    public static class BXml
    {
        /// <summary>
        /// xml默认字符集
        /// </summary>
        public static Encoding XmlCoding = Encoding.UTF8;

        /// <summary>
        /// 将xml字符串 转成T类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlStr"></param>
        /// <param name="bean"></param>
        /// <param name="isReplaceHead">是否替换xml的头部分</param>
        /// <param name="head"></param>
        /// <returns></returns>
        public static bool XmlToObj<T>(string xmlStr, ref T bean, bool isReplaceHead = false, string head = "")
        {
            bool ret = false;

            try
            {
                if (isReplaceHead)
                {
                    xmlStr = xmlStr.Trim();

                    if (xmlStr.StartsWith("<" + head + ">") && xmlStr.EndsWith("</" + head + ">"))
                    {
                        xmlStr = new StringBuilder().Append("<")
                            .Append(typeof(T).Name)
                            .Append(">")
                            .Append(xmlStr.Substring(head.Length + 2, xmlStr.Length - head.Length * 2 - 5))
                            .Append("</")
                            .Append(typeof(T).Name)
                            .AppendLine(">").ToString();
                    }
                }

                using (MemoryStream ms = new MemoryStream(XmlCoding.GetBytes(xmlStr)))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));

                    bean = (T)serializer.Deserialize(ms);
                    ret = true;
                }
            }
            catch (Exception e)
            {
                string err = "XmlToObj XML[" + xmlStr + "] Err:" + e.Message + e.StackTrace + " T:" + typeof(T).Name;
                err.Warn();
            }

            return ret;
        }

        /// <summary>
        /// T类型对象转成xml字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static bool ObjToXml<T>(T obj, ref string xmlStr, bool isReplace = false, string head = "")
        {
            bool ret = false;
            xmlStr = string.Empty;
            Type type = typeof(T);
            XmlSerializer xml = new XmlSerializer(type);
            string typeName = type.Name;

            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    xml.Serialize(stream, obj);
                    xmlStr = stream.ToArray().B2String();

                    if (isReplace)
                    {
                        xmlStr = xmlStr.Replace("xml", typeName);
                    }

                    ret = true;
                }
                catch (Exception e)
                {
                    string err = "ObjToXml Err:" + e.Message + e.StackTrace + " T:" + typeName;
                    err.Warn();
                }
            }

            return ret;
        }
    }
}

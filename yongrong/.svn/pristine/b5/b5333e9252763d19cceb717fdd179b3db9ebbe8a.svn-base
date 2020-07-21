using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ICcard32.Base.Http
{
    public class HttpReqBean
    {
        public string Url { get; set; }

        public byte[] ParamBytes;
        public string Param;
        public byte[] HttpRspBytes;
        public string HttpRsp;

        /// <summary>
        /// 错误内容
        /// </summary>
        public string HttpErr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// 请求字符集
        /// </summary>
        public Encoding Encoder { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 延时60秒 单位毫秒
        /// </summary>
        public int TimeOut { get; set; } = 60000;

        public string ContentType { get; set; } = string.Empty;
        /// <summary>
        /// 提交状态
        /// </summary>
        public bool Stat { get; set; } = false;

        public bool ExpThrow { get; set; } = false;

        public bool KeepActive { get; set; } = true;

        /// <summary>
        /// 2:采用HttpWebRequest
        /// </summary>
        public byte WebType { get; set; } = 2;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(777);

            sb.Append("Url:").Append(Url)
                .Append("\n\t Param:").Append(Param)
                .Append("\n\t ParamBytes:").Append(Encoding.UTF8.GetString(ParamBytes))
                .Append("\n\t KeepActive:").Append(KeepActive)
                .Append("\n\t WebType:").Append(WebType)
                .Append("\n\t TimeOut:").Append(TimeOut);

            return sb.ToString();
        }
    }

    public static class BHttp
    {
        public static void PostSync_WebClient(HttpReqBean httpreq)
        {
            try
            {
                if (null == httpreq.ParamBytes && !string.IsNullOrWhiteSpace(httpreq.Param))
                {
                    httpreq.ParamBytes = Encoding.UTF8.GetBytes(httpreq.Param);
                }

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        httpreq.HttpRspBytes = client.UploadData(httpreq.Url, httpreq.ParamBytes);

                        httpreq.HttpRsp = Encoding.UTF8.GetString(httpreq.HttpRspBytes);

                        httpreq.Stat = true;
                    }
                    catch (WebException we)
                    {
                        httpreq.HttpErr = we.Message + we.StackTrace;
                        httpreq.Stat = false;
                    }

                    client.Dispose();
                }
            }
            catch (Exception e)
            {
                httpreq.HttpErr = e.Message + e.StackTrace;
                httpreq.Stat = false;

                if (httpreq.ExpThrow)
                {
                    throw e;
                }
            }
            finally
            {
            }
        }
    }
}

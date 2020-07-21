using BaseS.Const;
using BaseS.File;
using BaseS.File.Log;
using BaseS.Net.Http.Bean;
using BaseS.String;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BaseS.Net.Http
{
    public static class BHttp
    {
        /// <summary>
        /// Http默认字符集
        /// </summary>
        public static Encoding HttpCoding = Encoding.UTF8;

        /// <summary>
        /// 
        /// </summary>
        static BHttp()
        {
            // 设置 ServicePoint 对象所允许的最大并发连接数
            ServicePointManager.DefaultConnectionLimit = 512;
            ServicePointManager.Expect100Continue = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpReq"></param>
        /// <returns></returns>
        public static bool Check(HttpReqBean httpreq)
        {
            if (null == httpreq)
            {
                httpreq.HttpErr = "HttpReqBean is NULL";
                httpreq.HttpErr.Warn();
                httpreq.Stat = false;

                return false;
            }

            if (null == httpreq.Encoder)
            {
                httpreq.Encoder = HttpCoding;
            }

            if (null == httpreq.ParamBytes && !string.IsNullOrWhiteSpace(httpreq.Param))
            {
                httpreq.ParamBytes = httpreq.Encoder.GetBytes(httpreq.Param);
            }

            if (string.IsNullOrWhiteSpace(httpreq.Param) && null != httpreq.ParamBytes)
            {
                httpreq.Param = httpreq.ParamBytes.B2String(httpreq.Encoder);
            }

            if (null == httpreq.ParamBytes)
            {
                httpreq.ParamBytes = new byte[0];
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpreq"></param>
        /// <returns></returns>
        public static void GetSync(HttpReqBean httpreq)
        {
            if (!Check(httpreq))
            {
                return;
            }

            switch (httpreq.WebType)
            {
                case 2:
                    GetSync_HttpWebRequest(httpreq);
                    break;
                case 0:
                default:
                    GetSync_WebRequest(httpreq);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpreq"></param>
        /// <returns></returns>
        public static bool GetSync_WebRequest(HttpReqBean httpreq)
        {
            string.Empty.Debug(string.Empty, BTip.Enter);

            WebRequest webreq = null;
            WebResponse webrsp = null;

            httpreq.HttpRspBytes = null;
            httpreq.HttpRsp = null;
            httpreq.HttpErr = string.Empty;

            try
            {
                webreq = WebRequest.Create(httpreq.Url);
                webreq.Timeout = httpreq.TimeOut;

                webrsp = webreq.GetResponse();

                httpreq.HttpRspBytes = BStream.ReadStreamBytes(webrsp.GetResponseStream());
                httpreq.HttpRsp = httpreq.HttpRspBytes.B2String(httpreq.Encoder);

                httpreq.Stat = true;
            }
            catch (Exception e)
            {
                httpreq.Stat = false;
                httpreq.HttpErr = e.Message + e.StackTrace;

                if (httpreq.ExpThrow)
                {
                    throw e;
                }
            }
            finally
            {
                try
                {
                    if (null != webrsp)
                    {
                        webrsp.Close();
                    }
                }
                catch (Exception e)
                {
                    httpreq.HttpErr = e.Message + e.StackTrace;
                }

                if (null == httpreq.HttpRspBytes)
                {
                    httpreq.HttpRspBytes = httpreq.Encoder.GetBytes(string.Empty);
                }

                if (httpreq.Stat)
                {
                    string.Format("Url:{0} Rsp:{1} Err:{2}", httpreq.Url, httpreq.HttpRsp, httpreq.HttpErr).Debug();
                }
                else
                {
                    string.Format("Url:{0} Rsp:{1} Err:{2}", httpreq.Url, httpreq.HttpRsp, httpreq.HttpErr).Warn();
                }
            }

            return httpreq.Stat;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpreq"></param>
        /// <returns></returns>
        public static bool GetSync_HttpWebRequest(HttpReqBean httpreq)
        {
            string.Empty.Debug(string.Empty, BTip.Enter);

            HttpWebRequest webreq = null;
            HttpWebResponse webrsp = null;

            httpreq.HttpRspBytes = null;
            httpreq.HttpRsp = null;
            httpreq.HttpErr = string.Empty;

            try
            {
                webreq = (HttpWebRequest)WebRequest.Create(httpreq.Url);
                webreq.Timeout = httpreq.TimeOut;
                webreq.KeepAlive = httpreq.KeepActive;

                webrsp = (HttpWebResponse)webreq.GetResponse();

                httpreq.HttpRspBytes = BStream.ReadStreamBytes(webrsp.GetResponseStream());
                httpreq.HttpRsp = httpreq.HttpRspBytes.B2String(httpreq.Encoder);
                httpreq.Stat = true;                
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
                try
                {
                    if (null != webrsp)
                    {
                        webrsp.Close();
                    }
                }
                catch (Exception e)
                {
                    httpreq.HttpErr = e.Message + e.StackTrace;
                }

                if (null == httpreq.HttpRspBytes)
                {
                    httpreq.HttpRspBytes = httpreq.Encoder.GetBytes(string.Empty);
                }

                if (httpreq.Stat)
                {
                    string.Format("Url:{0} Rsp:{1} Err:{2}", httpreq.Url, httpreq.HttpRsp, httpreq.HttpErr).Debug();
                }
                else
                {
                    string.Format("Url:{0} Rsp:{1} Err:{2}", httpreq.Url, httpreq.HttpRsp, httpreq.HttpErr).Warn();
                }
            }

            return httpreq.Stat;
        }

        /// <summary>
        /// http同步方式post数据
        /// </summary>
        /// <param name="httpreq"></param>
        /// <param name="webType">实现方式</param>
        /// <returns></returns>
        public static void PostSync(HttpReqBean httpreq)
        {
            if (!Check(httpreq))
            {
                return;
            }

            switch (httpreq.WebType)
            {
                case 2:
                    PostSync_HttpWebRequest(httpreq);
                    break;
                case 1:
                    PostSync_WebClient(httpreq);
                    break;
                case 0:
                default:
                    PostSync_WebRequest(httpreq);
                    break;
            }
        }

        /// <summary>
        /// http同步方式post数据 WebClient方式
        /// </summary>
        /// <param name="httpreq"></param>
        private static void PostSync_WebClient(HttpReqBean httpreq)
        {
            string.Empty.Debug(string.Empty, BTip.Enter);

            try
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        if (null != httpreq.Headers)
                        {
                            foreach (KeyValuePair<string, string> kv in httpreq.Headers)
                            {
                                client.QueryString[kv.Key] = kv.Value;
                            }
                        }

                        httpreq.HttpRspBytes = client.UploadData(httpreq.Url, httpreq.ParamBytes);

                        httpreq.HttpRsp = httpreq.HttpRspBytes.B2String(httpreq.Encoder);

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
            catch(Exception e)
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
                StringBuilder tipMsg = new StringBuilder(777);

                tipMsg.Append("Url:").AppendLine(httpreq.Url)
                    .Append("\t Param:").AppendLine(httpreq.Param)
                    .Append("\t Rsp:").AppendLine(httpreq.HttpRsp);

                if (httpreq.Stat)
                {
                    tipMsg.ToString().Debug("", BTip.Exit);
                }
                else
                {
                    tipMsg.ToString().Warn("", BTip.Exit);
                }
            }
        }

        /// <summary>
        /// http同步方式post数据 WebRequest方式
        /// </summary>
        /// <param name="httpreq"></param>
        private static void PostSync_WebRequest(HttpReqBean httpreq)
        {
            string.Empty.Debug(string.Empty, BTip.Enter);

            httpreq.HttpRsp = string.Empty;
            httpreq.HttpErr = string.Empty;

            try
            {
                WebRequest webreq = WebRequest.Create(httpreq.Url);

                if (httpreq.Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    webreq = WebRequest.Create(httpreq.Url);
                    //webreq.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    webreq = WebRequest.Create(httpreq.Url);
                }

                webreq.Proxy = null;
                webreq.Method = "POST";
                //webreq.KeepAlive = httpreq.KeepActive;
                webreq.ContentType = httpreq.ContentType;
                webreq.Timeout = httpreq.TimeOut;
                webreq.ContentLength = httpreq.ParamBytes.Length;

                if (null != httpreq.Headers)
                {
                    foreach (KeyValuePair<string, string> kv in httpreq.Headers)
                    {
                        webreq.Headers.Set(kv.Key, kv.Value);
                    }
                }

                using (Stream writer = webreq.GetRequestStream())
                {
                    // 写入输入流
                    writer.Write(httpreq.ParamBytes, 0, httpreq.ParamBytes.Length);
                    writer.Flush();
                    //BStream.CloseStream(writer);
                }

                // 获取输出流
                using (WebResponse webrsp = webreq.GetResponse())
                {
                    using (Stream reader = webrsp.GetResponseStream())
                    {
                        httpreq.HttpRspBytes = BStream.ReadStreamBytes(reader, true);
                        //BStream.CloseStream(reader);
                    }

                    CloseWebRsp(webrsp);
                }

                httpreq.HttpRsp = httpreq.HttpRspBytes.B2String(httpreq.Encoder);

                httpreq.Stat = true;
            }
            catch (WebException we)
            {
                httpreq.HttpErr = we.Message + we.StackTrace;
                httpreq.Stat = false;
                if (httpreq.ExpThrow)
                {
                    throw we;
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
                if (null == httpreq.HttpRsp)
                {
                    httpreq.HttpRsp = string.Empty;
                }

                if (httpreq.Stat)
                {
                    string.Format("Url:{0}\n\t Param:{1}\n\t Rsp:{2}\n\t Err:{3}", httpreq.Url, httpreq.Param, httpreq.HttpRsp, httpreq.HttpErr).Debug();
                }
                else
                {
                    string.Format("Url:{0}\n\t Param:{1}\n\t Rsp:{2}\n\t Err:{3}", httpreq.Url, httpreq.Param, httpreq.HttpRsp, httpreq.HttpErr).Warn();
                }
            }
        }

        /// <summary>
        /// http同步方式post数据 WebRequest方式
        /// </summary>
        /// <param name="httpreq"></param>
        private static void PostSync_HttpWebRequest(HttpReqBean httpreq)
        {
            string.Empty.Debug(string.Empty, BTip.Enter);
            DateTime start = DateTime.Now;

            httpreq.HttpRsp = string.Empty;
            httpreq.HttpErr = string.Empty;

            try
            {
                HttpWebRequest webreq = null;

                if (httpreq.Url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    webreq = WebRequest.Create(httpreq.Url) as HttpWebRequest;
                    //webreq.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    webreq = (HttpWebRequest)HttpWebRequest.Create(httpreq.Url);
                }

                webreq.Proxy = null;
                webreq.Method = "POST";
                webreq.KeepAlive = httpreq.KeepActive;
                webreq.ContentType = httpreq.ContentType;
                webreq.Timeout = httpreq.TimeOut;
                webreq.ContentLength = httpreq.ParamBytes.Length;

                if (null != httpreq.Headers)
                {
                    foreach (KeyValuePair<string, string> kv in httpreq.Headers)
                    {
                        webreq.Headers.Set(kv.Key, kv.Value);
                    }
                }

                using (Stream writer = webreq.GetRequestStream())
                {
                    writer.Write(httpreq.ParamBytes, 0, httpreq.ParamBytes.Length);
                    //BStream.CloseStream(writer);
                }

                ServicePointManager.DefaultConnectionLimit = 512;

                using (var webrsp = (HttpWebResponse)webreq.GetResponse())
                {
                    Stream reader;
                    using (reader = webrsp.GetResponseStream())
                    {
                        httpreq.HttpRspBytes = BStream.ReadStreamBytes(reader);
                        //BStream.CloseStream(reader);
                    }

                    CloseWebRsp(webrsp);
                }

                httpreq.HttpRsp = httpreq.HttpRspBytes.B2String(httpreq.Encoder);

                httpreq.Stat = true;
            }
            catch (WebException we)
            {
                httpreq.HttpErr = $"WebException:{we.Message} StackTrace:{we.StackTrace}";
                httpreq.Stat = false;
                if (httpreq.ExpThrow)
                {
                    throw we;
                }
            }
            catch (Exception e)
            {
                httpreq.HttpErr = $"Exception:{e.Message} StackTrace:{e.StackTrace}";
                httpreq.Stat = false;
                if (httpreq.ExpThrow)
                {
                    throw e;
                }
            }
            finally
            {
                if (null == httpreq.HttpRsp)
                {
                    httpreq.HttpRsp = string.Empty;
                }

                StringBuilder tipMsg = new StringBuilder(777);

                tipMsg.Append("Url:").AppendLine(httpreq.Url)
                    .Append("\t Param:").AppendLine(httpreq.Param)
                    .Append("\t Rsp:").AppendLine(httpreq.HttpRsp);

                if (!string.IsNullOrWhiteSpace(httpreq.HttpErr))
                {
                    tipMsg.Append("\t Err:").AppendLine(httpreq.HttpErr);
                }

                if (httpreq.Stat)
                {
                    tipMsg.ToString().Debug($"耗时:{(DateTime.Now - start).TotalMilliseconds}ms", BTip.Exit);
                }
                else
                {
                    tipMsg.ToString().Warn($"耗时:{(DateTime.Now - start).TotalMilliseconds}ms", BTip.Exit);
                }
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }

        private static void CloseWebReq(WebRequest req)
        {
            if (null == req)
            {
                return;
            }

            try
            {
                req.Abort();
            }
            catch(Exception e)
            {
                $"WebRequest Abort Message:{e.Message} StackTrace:{e.StackTrace}".Warn();
            }

            req = null;
        }

        private static void CloseWebRsp(WebResponse rsp)
        {
            if (null == rsp)
            {
                return;
            }

            try
            {
                rsp.Close();
            }
            catch (Exception e)
            {
                $"WebResponse close Message:{e.Message} StackTrace:{e.StackTrace}".Warn();
            }

            rsp = null;
        }

        /// <summary>
        /// 发送http应答
        /// </summary>
        /// <param name="context"></param>
        /// <param name="rspdata"></param>
        public static void SendHttpRsp(this HttpListenerResponse response, byte[] rspdata, string origin = "", bool isCross = false)
        {
            if (null == response?.OutputStream || !response.OutputStream.CanWrite)
            {
                "应答对象流无法写入".Warn();
                return;
            }

            if (null == rspdata)
            {
                "应答数据为null".Warn();
                return;
            }

            if (isCross)
            {
                //跨域 返回参数
                response.Headers.Add("Access-Control-Allow-Method:OPTIONS,POST,GET");
                response.Headers.Add("Access-Control-Allow-Headers:x-requested-with");
                response.Headers.Add($"Access-Control-Allow-Origin:{origin}");
                response.Headers.Add("Access-Control-Allow-Credentials:true");
                response.ContentEncoding = Encoding.UTF8;
                response.ContentType = "text/html; charset=utf-8";
            }

            response.OutputStream.Write(rspdata, 0, rspdata.Length);
            response.OutputStream.Flush();
        }
    }
}

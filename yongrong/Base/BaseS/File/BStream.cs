using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BaseS.File.Log;

namespace BaseS.File
{
    public static class BStream
    {
        const int BufferSize = 2048;

        /// <summary>
        /// 读取流中的字节
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadStreamBytes(Stream stream, bool closeFlag = false, bool async = false)
        {
            byte[] rspbytes = null;
            byte[] buffer = new byte[BufferSize];
            
            using (MemoryStream ms = new MemoryStream())
            {
                int readLen = 0;

                do
                {
                    try
                    {
                        if (async)
                        {
                            var task = stream.ReadAsync(buffer, 0, BufferSize);
                            task.Wait();
                            readLen = task.Result;
                        }
                        else
                        {
                            readLen = stream.Read(buffer, 0, BufferSize);
                        }
                    }
                    catch (Exception e)
                    {
                        ("Err:" + e.Message + " StackTrace:" + e.StackTrace).Warn();
                        readLen = -1;
                    }

                    if (1 <= readLen)
                    {
                        ms.Write(buffer, 0, readLen);
                    }
                } while (1 <= readLen);

                CloseStream(ms);

                rspbytes = ms.ToArray();
            }

            if (closeFlag)
            {
                CloseStream(stream);
            }

            return rspbytes;
        }

        /// <summary>
        /// 关闭流
        /// </summary>
        /// <param name="stream"></param>
        public static void CloseStream(Stream stream)
        {
            if (null == stream)
            {
                return;
            }

            try
            {
                stream.Close();
            }
            catch (Exception e)
            {
                $"Stream close Exception:{e.Message} {e.StackTrace}".Warn();
            }
        }
    }
}

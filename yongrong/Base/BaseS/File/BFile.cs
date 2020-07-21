using BaseS.File.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BaseS.File
{
    public static class BFile
    {
        /// <summary>
        /// 文件是否为空
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsFileEmpty(this string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                return true;
            }

            FileInfo fi = new FileInfo(fileName);

            if (0 >= fi.Length)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取文本文件编码格式
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool GetTxtEncoding(this string filename, ref Encoding encoding)
        {
            Byte[] buffer = null;

            encoding = Encoding.UTF8;

            try
            {
                using (FileStream fs = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    BinaryReader br = new BinaryReader(fs);

                    buffer = br.ReadBytes(2);

                    br.Dispose();
                    fs.Dispose();
                }
            }
            catch (Exception)
            {
                return false;
            }

            // 根据最前面两个字节获取到对应的编码字符集
            if (null != buffer && 0xEF <= buffer[0])
            {
                if (buffer[0] == 0xEF && buffer[1] == 0xBB)
                {
                    encoding = System.Text.Encoding.UTF8;
                }
                else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                {
                    encoding = System.Text.Encoding.BigEndianUnicode;
                }
                else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                {
                    encoding = System.Text.Encoding.Unicode;
                }
                else
                {
                    return false;
                }

                return true;
            }

            return false;

        }

        public static bool Txt2Bytes(this string filePath, ref byte[] bytes,Encoding coding = null)
        {
            if (null == coding)
            {
                coding = Encoding.UTF8;
            }
            
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        bytes = BStream.ReadStreamBytes(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                string.Format("Message:{0} StackTrace:{1}", ex.Message, ex.StackTrace).Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="msg"></param>
        /// <param name="coding"></param>
        /// <returns></returns>
        public static bool Txt2String(this string filePath, ref string msg, Encoding coding = null)
        {
            if (null == coding)
            {
                coding = Encoding.UTF8;
            }

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        StreamReader sr = new StreamReader(fs, coding);
                        msg = sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                string.Format("Message:{0} StackTrace:{1}", ex.Message, ex.StackTrace).Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="msg"></param>
        /// <param name="append"></param>
        /// <param name="coding"></param>
        /// <returns></returns>
        public static bool String2File(this string msg, string filePath, bool append = true, Encoding coding = null)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                return false;
            }

            string path = Path.GetDirectoryName(filePath);
            DirectoryInfo di = new DirectoryInfo(path);

            // 创建目录
            if (!di.Exists)
            {
                di.Create();
            }

            if (null == coding)
            {
                coding = Encoding.UTF8;
            }

            try
            {
                if (!append)
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, coding))
                        {
                            sw.WriteLine(msg);
                            sw.Flush();
                        }
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                    {
                        using (StreamWriter sw = new StreamWriter(fs, coding))
                        {
                            sw.WriteLine(msg);
                            sw.Flush();
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message + e.StackTrace);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="msg"></param>
        /// <param name="append"></param>
        /// <param name="coding"></param>
        /// <returns></returns>
        public static bool Bytes2File(this byte[] msgbytes, string filePath, bool append = true, Encoding coding = null)
        {
            return Bytes2File(msgbytes, filePath, append, coding, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgbytes"></param>
        /// <param name="filePath"></param>
        /// <param name="append"></param>
        /// <param name="coding"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static bool Bytes2File(this byte[] msgbytes, string filePath, bool append = true, Encoding coding = null, int start = 0, int count = 0)
        {
            if (null == msgbytes)
            {
                return false;
            }

            string path = Path.GetDirectoryName(filePath);

            DirectoryInfo di = new DirectoryInfo(path);

            if (!di.Exists)
            {
                di.Create();
            }

            if (null == coding)
            {
                coding = Encoding.UTF8;
            }

            if (0 >= count)
            {
                count = msgbytes.Length - start;
            }

            try
            {
                if (!append)
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        fs.Write(msgbytes, start, count);
                        fs.Flush();
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                    {
                        fs.Write(msgbytes, start, count);
                        fs.Flush();
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 将txt中每行放到List<string>中
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="list"></param>
        /// <param name="coding"></param>
        /// <returns></returns>
        public static bool Txt2StringList(this string filePath, out List<string> list, Encoding coding = null, bool isTrim = true)
        {
            list = new List<string>();

            if (null == coding)
            {
                coding = Encoding.UTF8;
            }

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    StreamReader sr = new StreamReader(fs, coding);

                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (isTrim)
                        {
                            line = line.Trim();
                        }

                        list.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                string.Format("Message:{0} StackTrace:{1}", ex.Message, ex.StackTrace).Warn();
                return false;
            }


            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="msg"></param>
        /// <param name="append"></param>
        /// <param name="coding"></param>
        /// <returns></returns>
        public static bool StringList2File(this IEnumerable<string> msgList, string filePath, bool append = true, Encoding coding = null)
        {
            if (null == msgList)
            {
                return false;
            }

            string path = Path.GetDirectoryName(filePath);

            DirectoryInfo di = new DirectoryInfo(path);

            if (!di.Exists)
            {
                di.Create();
            }

            if (null == coding)
            {
                coding = Encoding.UTF8;
            }

            try
            {
                if (!append)
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        StreamWriter sw = new StreamWriter(fs, coding);

                        foreach (string s in msgList)
                        {
                            sw.WriteLine(s);
                        }

                        sw.Flush();
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read))
                    {
                        StreamWriter sw = new StreamWriter(fs, coding);

                        foreach (string s in msgList)
                        {
                            sw.WriteLine(s);
                        }

                        sw.Flush();
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}

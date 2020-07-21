﻿using BaseS.Collection;
using BaseS.File.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace BaseS.File
{
    public static class BCsv
    {
        static readonly byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
        static readonly byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
        public static readonly byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="csvStr"></param>
        public static void DataTable2CsvString(DataTable dt, out string csvStr, Dictionary<string,string> cmap = null)
        {
            StringBuilder csvSb = new StringBuilder(102400);

            try
            {
                string colname = string.Empty;
                // 需要显示的列序号集合
                List<int> valList = new List<int>();

                for (int i = 0; i < dt.Columns.Count; i++)//写入列名
                {
                    colname = dt.Columns[i].ColumnName;

                    if (null != cmap)
                    {
                        // 该列需要展示,并且替换对应的value
                        if (cmap.ContainsKey(dt.Columns[i].ColumnName))
                        {
                            valList.Add(i);
                            colname = cmap.GetDicStr(dt.Columns[i].ColumnName, colname);

                            csvSb.Append(colname);

                            if (i < dt.Columns.Count - 1)
                            {
                                csvSb.Append(',');
                            }
                        }
                    }
                    else
                    {
                        csvSb.Append(colname);

                        if (i < dt.Columns.Count - 1)
                        {
                            csvSb.Append(',');
                        }
                    }                    
                }

                csvSb.AppendLine();

                for (int i = 0; i < dt.Rows.Count; i++) //写入各行数据
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        // 不在需要显示的列集合中,直接跳过
                        if(1 <= valList.Count && !valList.Contains(j))
                        {
                            continue;
                        }

                        string str = dt.Rows[i][j].ToString();

                        str = str.Replace("\"", "\"\""); //替换英文冒号 英文冒号需要换成两个冒号

                        if (str.Contains(",") || str.Contains("\"")
                            || str.Contains("\r") || str.Contains("\n")) //含逗号 冒号 换行符的需要放到引号中
                        {
                            str = string.Format("\"{0}\"", str);
                        }

                        csvSb.Append(str);

                        if (j < dt.Columns.Count - 1)
                        {
                            csvSb.Append(',');
                        }
                    }

                    csvSb.AppendLine();
                }
            }
            catch (Exception e)
            {
                $"Exception:{e.Message} StackTrace:{e.StackTrace}".Warn();
            }

            csvStr = csvSb.ToString();
        }

        /// <summary>
        /// table数据写入csv
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fullPath"></param>
        public static void DataTable2CsvFile(DataTable dt, string fullPath)
        {
            FileInfo fi = new FileInfo(fullPath);

            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }

            FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);

            try
            {
                DataTable2CsvString(dt, out string csvStr);

                sw.Write(csvStr);
            }
            catch (Exception e)
            {
                $"Exception:{e.Message} StackTrace:{e.StackTrace}".Warn();
            }
            finally
            {
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// 从csv读取数据返回table
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="isContainHead">第一行为标题</param>
        /// <returns></returns>
        public static DataTable Csv2DataTable(string filePath, bool isContainHead = true, Dictionary<string,string> columsMap = null)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return null;
            }

            Encoding encoding = GetFileEncoding(filePath);

            BFile.Txt2StringList(filePath, out List<string> lines, encoding, false);

            Csv2DataTable(lines, out DataTable table, isContainHead, columsMap);

            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="dt"></param>
        /// <param name="isContainHead"></param>
        /// <returns></returns>
        public static bool Csv2DataTable(IEnumerable<string> lines, out DataTable dt, bool isContainHead = true, Dictionary<string, string> columsMap = null)
        {
            dt = new DataTable();

            //记录每行记录中的各字段内容
            string[] colValues = null;
            string[] colNames = null;

            try
            {
                //逐行读取CSV中的数据
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    // 解析标题
                    if (isContainHead)
                    {
                        isContainHead = false;
                        colNames = line.Split(',');

                        for (int i = 0; i < colNames.Length; i++)
                        {
                            DataColumn dc = new DataColumn(colNames[i]);

                            if (null != columsMap)
                            {
                                columsMap.TryGetValue(colNames[i], out string newColName);

                                if (!string.IsNullOrWhiteSpace(newColName))
                                {
                                    dc.ColumnName = newColName;
                                }
                            }

                            dt.Columns.Add(dc);
                        }

                        continue;
                    }

                    colValues = line.Split(',');

                    // 补充固定的列名 C+开头序号
                    if (0 == dt.Columns.Count)
                    {
                        for (int i = 0; i < colValues.Length; i++)
                        {
                            DataColumn dc = new DataColumn("C" + i);
                            dt.Columns.Add(dc);
                        }
                    }

                    DataRow dr = dt.NewRow();

                    for (int j = 0; j < dt.Columns.Count && j < colValues.Length; j++)
                    {
                        dr[j] = colValues[j];
                    }

                    dt.Rows.Add(dr);
                }
            }
            catch (Exception e)
            {
                $"Exception:{e.Message} StackTrace:{e.StackTrace}".Warn();
                return false;
            }

            return true;
        }

        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// <param name="filePath">文件路径</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetFileEncoding(string filePath)
        {
            Encoding coding = Encoding.UTF8;

            using (FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                coding = GetFileEncoding(fs);
                fs.Close();
            }

            return coding;
        }

        /// 通过给定的文件流，判断文件的编码类型
        /// <param name="fs">文件流</param>
        /// <returns>文件的编码类型</returns>
        private static Encoding GetFileEncoding(FileStream fs, Encoding defCoding = null)
        {
            if (null == defCoding)
            {
                defCoding = Encoding.GetEncoding(936);
            }

            if (3 > fs.Length)
            {
                return defCoding;
            }

            using (BinaryReader r = new System.IO.BinaryReader(fs, System.Text.Encoding.Default))
            {
                byte[] startBytes = r.ReadBytes(3);

                if (IsUTF8Bytes(startBytes) || (startBytes[0] == UTF8[0] && startBytes[1] == UTF8[1] && startBytes[2] == UTF8[2]))
                {
                    defCoding = System.Text.Encoding.UTF8;
                }
                else if (startBytes[0] == UnicodeBIG[0] && startBytes[1] == UnicodeBIG[1] && startBytes[2] == UnicodeBIG[2])
                {
                    defCoding = System.Text.Encoding.BigEndianUnicode;
                }
                else if (startBytes[0] == Unicode[0] && startBytes[1] == Unicode[1] && startBytes[2] == Unicode[2])
                {
                    defCoding = System.Text.Encoding.Unicode;
                }

                r.Close();
            }

            return defCoding;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reqbytes"></param>
        /// <returns></returns>
        public static Encoding GetEncoding(byte[] reqbytes)
        {
            Encoding defCoding = Encoding.GetEncoding(936);

            if (null == reqbytes || 3 > reqbytes.Length)
            {
                return defCoding;
            }

            if (IsUTF8Bytes(reqbytes) || (reqbytes[0] == UTF8[0] && reqbytes[1] == UTF8[1] && reqbytes[2] == UTF8[2]))
            {
                defCoding = System.Text.Encoding.UTF8;
            }
            else if (reqbytes[0] == UnicodeBIG[0] && reqbytes[1] == UnicodeBIG[1] && reqbytes[2] == UnicodeBIG[2])
            {
                defCoding = System.Text.Encoding.BigEndianUnicode;
            }
            else if (reqbytes[0] == Unicode[0] && reqbytes[1] == Unicode[1] && reqbytes[2] == Unicode[2])
            {
                defCoding = System.Text.Encoding.Unicode;
            }

            return defCoding;
        }

        /// <summary>
        /// Csv转成字符串集合
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="lines"></param>
        /// <param name="isContainHead"></param>
        /// <returns></returns>
        public static bool Csv2List(string filePath, out List<string> lines, bool isContainHead = true)
        {
            if (!System.IO.File.Exists(filePath))
            {
                lines = new List<string>();
                return false;
            }

            Encoding encoding = GetFileEncoding(filePath);

            BFile.Txt2StringList(filePath, out lines, encoding, false);

            return true;
        }

        /// 判断是否是不带 BOM 的 UTF8 格式
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;  //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.

            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];

                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }

                    charByteCounter--;
                }
            }

            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }

            return true;
        }

    }
}

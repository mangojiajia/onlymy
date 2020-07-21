using BaseS.File.Log;
using System;
using System.Data.OleDb;
using System.Data;

namespace BaseS.File
{
    public static class BExcel
    {
        /// <summary>
        /// Excel转DataTable
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool ToDataTable(this string path, string tabName, out DataTable data)
        {
            data = new DataTable();

            if (string.IsNullOrWhiteSpace(path) || 4 >= path.Length)
            {
                "Excel文件地址为空".Warn();
                return false;
            }

            if (!(path.EndsWith(".xls", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase)))
            {
                "Excel文件后缀错误".Warn();
                return false;
            }

            var end = path.Substring(path.LastIndexOf('.')).ToLower();
            string conn = string.Empty;

            switch (end)
            {
                case ".xls":
                    conn = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={path};Extended Properties=\"Excel 8.0;HDR=NO;IMEX=1;\"";
                    break;
                case ".xlsx":
                    conn = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;\"";
                    break;
                default:
                    break;
            }

            try
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter($"select * from [{tabName}]", conn))
                {
                    adapter.Fill(data);
                }
            }
            catch (OleDbException oe)
            {
                $"Err:{oe.Message} \n StackTrace:{oe.StackTrace}".Warn();
            }
            catch (Exception e)
            {
                $"Err:{e.Message} \n StackTrace:{e.StackTrace}".Warn();
            }

            return true;
        }
    }
}

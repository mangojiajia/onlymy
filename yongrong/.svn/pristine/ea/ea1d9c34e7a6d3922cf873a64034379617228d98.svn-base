using System;
using System.Collections.Generic;
using System.Text;

namespace BaseS.Data.Database
{
    public class DbBase
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="sqltxt"></param>
        /// <param name="paramstr"></param>
        /// <param name="err"></param>
        /// <param name="stacktrace"></param>
        /// <returns></returns>
        public static string CreateErrMsg(string connStr, string sqltxt, string paramstr, string err, string stacktrace, int errcode = int.MinValue)
        {
            StringBuilder sb = new StringBuilder(370);

            if (!string.IsNullOrWhiteSpace(err))
            {
                sb.Append("Message:").Append(err);
            }

            if (int.MinValue != errcode)
            {
                sb.Append(" ErrorCode:").Append(errcode);
            }
            if (!string.IsNullOrWhiteSpace(stacktrace))
            {
                sb.Append(" StackTrace:").Append(stacktrace);
            }
            if (!string.IsNullOrWhiteSpace(sqltxt))
            {
                sb.Append(" Sql:").Append(sqltxt);
            }
            if (!string.IsNullOrWhiteSpace(paramstr))
            {
                sb.Append(" Param:").Append(paramstr);
            }

            if (0 == errcode && !string.IsNullOrWhiteSpace(connStr))
            {
                sb.Append(" ConnectionString:").Append(connStr);
            }

            return sb.ToString();
        }
    }
}

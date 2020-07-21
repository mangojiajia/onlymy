using BaseS.File.Log;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Conf;

namespace Yongrong.Db
{
    public class BaseDb : BaseConf
    {
        public static string Conn
        {
            get
            {
                if (Sys.ReleaseFlag)
                {
                    // 现网数据库
                    return "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=172.188.188.206)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id = yr; Password=test;";
                }
                else
                {
                    // 本地数据库
                    return "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.28)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id = yr; Password=123456;";
                }
            }
        }

        /// 
        /// </summary>
        /// <param name="seqName"></param>
        /// <returns></returns>
        public static int GetSeq(string seqName)
        {
            var obj = OracleHelper.ExecuteScalar($"select {seqName}.nextval from dual");

            int.TryParse(obj?.ToString(), out int result);

            return result;
        }

        /// <summary>
        /// 提交到数据库
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static int SaveChanges(YrContext db, string Err = "")
        {
            int ret = -1;

            try
            {
                ret = db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException dce)
            {
                $"Err: {Err} - {dce.Message} InnerErr:{dce?.InnerException?.Message} {dce.StackTrace}".Warn("DbUpdateConcurrencyException 提交异常");
            }
            catch (DbUpdateException de)
            {
                $"Err:{Err} - {de.Message} InnerErr:{de?.InnerException?.Message} {de.StackTrace}".Warn("DbUpdateException 提交异常");
            }
            
            return ret;
        }
    }
}

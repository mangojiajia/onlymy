using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BaseS.File.Log;
using BaseS.Data;

namespace Yongrong.Db
{
    public class OracleHelper : BaseDb
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static bool ExecuteQuery(string sql, out DataTable table)
        {
            table = new DataTable();

            try
            {
                using (var adapter = new OracleDataAdapter(sql, Conn))
                {
                    adapter.Fill(table);
                }
            }
            catch(OracleException oe)
            {
                sql.Warn("sql");
                $"{oe.Message} {oe.StackTrace}".Warn();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 查询sql获取对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool Query<T>(string sql, out List<T> data) where T : class, new()
        {
            data = new List<T>();
                
            if (!ExecuteQuery(sql, out var dt))
            {
                return false;
            }

            data = BDataTable.ToList_Old<T>(dt);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbIdx"></param>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        public static int ExecuteNoQuery(List<String> sqlList)
        {
            int count = 0;

            foreach (var sql in sqlList)
            {
                if (string.IsNullOrWhiteSpace(sql))
                {
                    continue;
                }

                count += ExecuteNoQuery(sql);
            }

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbIdx"></param>
        /// <param name="sqlList"></param>
        /// <returns></returns>
        public static int ExecuteNoQuery(string sql)
        {
            int ret = -1;
            using (OracleConnection conn = new OracleConnection(Conn))
            {
                using (OracleCommand cmd = new OracleCommand { Connection = conn })
                {
                    try
                    {
                        conn.Open();

                        cmd.CommandText = sql;

                        ret = cmd.ExecuteNonQuery();
                    }
                    catch (OracleException me)
                    {
                        sql.Warn("sql");
                        $"{me.Message} {me.StackTrace}".Warn();

                        return -1;
                    }
                    catch (Exception e)
                    {
                        sql.Warn("sql");
                        $"Sql:{sql}\n{e.Message} {e.StackTrace}".Warn();

                        return -1;
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbIdx"></param>
        /// <param name="sql"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql)
        {
            object ret = null;

            using (OracleConnection conn = new OracleConnection(Conn))
            {
                using (OracleCommand cmd = new OracleCommand { Connection = conn })
                {
                    try
                    {
                        conn.Open();

                        cmd.CommandText = sql;

                        ret = cmd.ExecuteScalar();
                    }
                    catch (OracleException me)
                    {
                        sql.Warn("sql");
                        $"{me.Message} {me.StackTrace}".Warn();
                    }
                    catch (Exception e)
                    {
                        sql.Warn("sql");
                        $"Sql:{sql}\n{e.Message} {e.StackTrace}".Warn();
                    }
                    finally
                    {
                        if (null != cmd.Parameters && 1 < cmd.Parameters.Count)
                        {
                            try
                            {
                                cmd.Parameters.Clear();
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }

                return ret;
            }
        }
    }
}

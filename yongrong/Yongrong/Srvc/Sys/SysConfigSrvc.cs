﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using Yongrong.Model.Db;
using Yongrong.Model.Int;
using BaseS.File.Log;
using System.Linq;
using Yongrong.Model.Conf;

namespace Yongrong.Srvc.Sys
{
    class SysConfigSrvc : BaseSrvc
    {
        /// <summary>
        /// 配置项内存表
        /// key:ekey
        /// </summary>
        static readonly ConcurrentDictionary<string, SysConfig> confTab
            = new ConcurrentDictionary<string, SysConfig>();

        /// <summary>
        /// 
        /// </summary>
        public static void Init()
        {
            GetAll(out var sysconfigs, false);

            foreach(var c in sysconfigs)
            {
                confTab.TryAdd(c.Ekey, c);
            }
        }

        /// <summary>
        /// 获取所有配置项
        /// </summary>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="baseTrailerslist"></param>
        /// <returns></returns>
        public static bool GetAll(out List<SysConfig> sysconfiglist, bool usemem = false)
        {
            sysconfiglist = new List<SysConfig>();

            if (usemem)
            {
                sysconfiglist.AddRange(confTab.Values);
            }
            else
            {
                using (var db = DbContext)
                {
                    sysconfiglist.AddRange(db.SysConfig);
                }
            }

            $"获取到所有配置项:{sysconfiglist.Count}个".Info();

            return true;
        }


        /// <summary>
        /// 获取单一配置项值
        /// </summary>
        /// <returns></returns>
        public static bool GetOne(string ek, out string val)
        {
            confTab.TryGetValue(ek, out var config);

            val = config?.Price ?? "";

            return !string.IsNullOrWhiteSpace(val);
        }

        /// <summary>
        /// 添加和更新 系统配置项
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(SysConfig addupdobj, string op)
        {
            if (string.IsNullOrWhiteSpace(addupdobj?.Ekey)
                || string.IsNullOrWhiteSpace(addupdobj?.Ckey))
            {
                return ParamNull;
            }

            if (ADD.Equals(op) && confTab.ContainsKey(addupdobj.Ekey))
            {
                return $"该配置项{addupdobj.Ekey}已经存在";
            }

            if (UPD.Equals(op) && !confTab.ContainsKey(addupdobj.Ekey))
            {
                return $"该配置项{addupdobj.Ekey}不存在,无法更新";
            }

            if (DEL.Equals(op) && !confTab.ContainsKey(addupdobj.Ekey))
            {
                return $"该配置项{addupdobj.Ekey}不存在,无法删除";
            }

            using (var db = DbContext)
            {
                switch (op)
                {
                    case ADD:
                        addupdobj.Id = GetSeq("SEQ_Sys_Config"); 
                        db.SysConfig.Add(addupdobj);

                        confTab.TryAdd(addupdobj.Ekey, addupdobj);
                        break;
                    case UPD:
                        var tmp = db.SysConfig.FirstOrDefault(a => addupdobj.Ckey.Equals(a.Ckey));

                        if (null == tmp)
                        {
                            $"SysConfig无法找到{addupdobj.Ckey}".Warn();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.SysConfig.Update(tmp);

                        confTab.AddOrUpdate(addupdobj.Ekey, addupdobj, (k, v) => addupdobj);
                        break;
                    case DEL:
                        var tmp1 = db.SysConfig.FirstOrDefault(a => addupdobj.Ckey.Equals(a.Ckey));

                        if (null == tmp1)
                        {
                            $"SysConfig无法找到{addupdobj.Id}".Warn();
                            return DelNoRecord;
                        }

                        db.SysConfig.Remove(tmp1);
                        confTab.TryRemove(tmp1.Ekey, out var _);
                        break;
                    default:
                        OpEmpty.Info();
                        return OpEmpty;
                }

                SaveChanges(db, "SysConfig" + op);
            }

            return Success;
        }
    }
}

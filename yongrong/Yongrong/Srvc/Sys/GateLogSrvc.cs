﻿using System;
using System.Collections.Generic;
using System.Text;
using Yongrong.Model.Db;
using BaseS.File.Log;
using System.Linq;
using Yongrong.Model.Int;

namespace Yongrong.Srvc.Sys
{
    public class GateLogSrvc : BaseSrvc
    {
        /// <summary>
        /// 添加和更新 门禁日志表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(GateLog addupdobj, string op)
        {
            if (null == addupdobj)
            {
                return ParamNull;
            }

            using (var db = DbContext)
            {
                switch (op)
                {
                    case ADD:
                        addupdobj.Id = GetSeq("SEQ_GATELOG");
                        addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        db.GateLog.Add(addupdobj);
                        break;
                    case UPD:
                        var tmp = db.GateLog.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"GateLog Id:{addupdobj.Id} 不存在".Warn();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.GateLog.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.GateLog.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"GateLog Id:{addupdobj.Id} 不存在".Warn();
                            return DelNoRecord;
                        }

                        db.GateLog.Remove(tmp1);
                        break;
                    default:
                        OpEmpty.Info();
                        return OpEmpty;
                }

                SaveChanges(db, "GateLog " + op);
            }
            return Success;
        }

    }
}

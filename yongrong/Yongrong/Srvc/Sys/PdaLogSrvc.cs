using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yongrong.Model.Db;
using BaseS.File.Log;

namespace Yongrong.Srvc.Sys
{
    public class PdaLogSrvc : BaseSrvc
    {
        /// <summary>
        /// 添加和更新 Pda日志表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string AddOrUpdate(PdaLog addupdobj, string op)
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
                        addupdobj.Id = GetSeq("SEQ_PDALOG");
                        addupdobj.Createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        db.PdaLog.Add(addupdobj);
                        break;
                    case UPD:
                        var tmp = db.PdaLog.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp)
                        {
                            $"PdaLog Id:{addupdobj.Id} 不存在".Warn();
                            return UpdNoRecord;
                        }

                        addupdobj.Upd(tmp);

                        db.PdaLog.Update(tmp);
                        break;
                    case DEL:
                        var tmp1 = db.PdaLog.FirstOrDefault(a => a.Id == addupdobj.Id);

                        if (null == tmp1)
                        {
                            $"PdaLog Id:{addupdobj.Id} 不存在".Warn();
                            return DelNoRecord;
                        }

                        db.PdaLog.Remove(tmp1);
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
